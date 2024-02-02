using System.Reflection;
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
        Console.WriteLine(e);
    }
}

app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(corsPolitics);
app.MapControllers();
app.Run();