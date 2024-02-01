using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;

namespace Notes.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<NotesDbContext>(opt =>
            opt.UseSqlite(connectionString));
        services.AddScoped<INotesDbContext>(provider =>
            provider.GetRequiredService<NotesDbContext>());
        return services;
    }
}