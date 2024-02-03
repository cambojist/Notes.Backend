using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Persistence;

namespace Notes.Tests.Common;

public class NotesContextFactory
{
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();

    public static Guid NoteIdForDelete = Guid.NewGuid();
    public static Guid NoteIdForUpdate = Guid.NewGuid();

    public static NotesDbContext Create()
    {
        var opt = new DbContextOptionsBuilder<NotesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new NotesDbContext(opt);
        context.Database.EnsureCreated();

        context.Notes.AddRange(
            new Note
            {
                Title = "Title1",
                Details = "Details1",
                CreationDate = DateTime.Today,
                EditDate = null,
                Id = Guid.Parse("B51A1977-3334-46E3-AA66-EF7B7BC56476"),
                UserId = UserAId
            },
            new Note
            {
                Title = "Title2",
                Details = "Details2",
                CreationDate = DateTime.Today,
                EditDate = null,
                Id = Guid.Parse("4D4CC26F-C27D-47E4-971F-85963A685CC7"),
                UserId = UserBId
            },
            new Note
            {
                Title = "Title3",
                Details = "Details3",
                CreationDate = DateTime.Today,
                EditDate = null,
                Id = NoteIdForDelete,
                UserId = UserAId
            },
            new Note
            {
                Title = "Title4",
                Details = "Details4",
                CreationDate = DateTime.Today,
                EditDate = null,
                Id = NoteIdForUpdate,
                UserId = UserBId
            }
        );
        context.SaveChanges();
        return context;
    }

    public static void Destroy(NotesDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}