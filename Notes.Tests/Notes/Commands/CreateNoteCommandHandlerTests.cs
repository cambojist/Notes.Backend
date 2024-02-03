using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands;

public class CreateNoteCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateNoteCommandHandler_Success()
    {
        var handler = new CreateNoteCommandHandler(Context);
        var noteTitle = "note name";
        var noteDetails = "note details";

        var noteId = await handler.Handle(
            new CreateNoteCommand
            {
                UserId = NotesContextFactory.UserAId,
                Title = noteTitle,
                Details = noteDetails
            },
            CancellationToken.None);

        Assert.NotNull(
            await Context.Notes.SingleOrDefaultAsync(note =>
                note.Id == noteId &&
                note.Title == noteTitle &&
                note.Details == noteDetails));
    }
}