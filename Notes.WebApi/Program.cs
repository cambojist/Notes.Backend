using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.WebApi;
using Notes.WebApi.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var corsPolitics = "AllowAll";

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});
builder.Services.AddApplication();
builder.Services.AddPersistence(configuration);
builder.Services.AddControllers();
builder.Services.AddCors(
    opt =>
        opt.AddPolicy(corsPolitics, policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        }));

builder.Services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", opt =>
    {
        opt.Authority = "http://localhost:5130";
        opt.Audience = "NotesWebAPI";
        opt.RequireHttpsMetadata = false;
    });

builder.Services.AddApiVersioning(opt =>
    {
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(x =>
    {
        x.GroupNameFormat = "'v'VVV";
        x.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var ctx = serviceProvider.GetRequiredService<NotesDbContext>();
        DbInitializer.Initialize(ctx);
    }
    catch (Exception e)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while app initialization");
    }
}

var apiVersionDescriptionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();

app.UseCustomExceptionHandler();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(cfg =>
{
    foreach (var description in apiVersionDescriptionProvider?.ApiVersionDescriptions!)
    {
        cfg.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        cfg.RoutePrefix = string.Empty;
    }
});
app.UseHttpsRedirection();
app.UseCors(corsPolitics);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();