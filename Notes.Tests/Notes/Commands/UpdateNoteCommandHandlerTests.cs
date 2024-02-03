using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands;

public class UpdateNoteCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task UpdateNoteCommandHandler_Success()
    {
        var handler = new UpdateNoteCommandHandler(Context);
        var updatedTitle = "new title";

        await handler.Handle(new UpdateNoteCommand
        {
            Id = NotesContextFactory.NoteIdForUpdate,
            UserId = NotesContextFactory.UserBId,
            Details = updatedTitle
        }, CancellationToken.None);

        Assert.NotNull(
            await Context.Notes.SingleOrDefaultAsync(note =>
                note.Id == NotesContextFactory.NoteIdForUpdate &&
                note.Details == updatedTitle
            ));
    }

    [Fact]
    public async Task UpdateNoteCommandHandler_FailOnWrongId()
    {
        var handler = new UpdateNoteCommandHandler(Context);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new UpdateNoteCommand
            {
                Id = Guid.NewGuid(),
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
    {
        var handler = new UpdateNoteCommandHandler(Context);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new UpdateNoteCommand
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None));
    }
}