using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.WebApi.Middleware;

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

app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(corsPolitics);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();