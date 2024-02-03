using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands;

public class DeleteNoteCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task DeleteNoteCommandHandler_Success()
    {
        var handler = new DeleteNoteCommandHandler(Context);

        await handler.Handle(new DeleteNoteCommand
        {
            Id = NotesContextFactory.NoteIdForDelete,
            UserId = NotesContextFactory.UserAId
        }, CancellationToken.None);

        Assert.NotNull(Context.Notes.SingleOrDefaultAsync(note =>
            note.Id == NotesContextFactory.NoteIdForDelete));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongId()
    {
        var handler = new DeleteNoteCommandHandler(Context);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new DeleteNoteCommand
            {
                Id = Guid.NewGuid(),
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
    {
        var deleteHandler = new DeleteNoteCommandHandler(Context);
        var createHandler = new CreateNoteCommandHandler(Context);
        var noteId = await createHandler.Handle(
            new CreateNoteCommand
            {
                Title = "NoteTitle",
                Details = "NoteDetails",
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await deleteHandler.Handle(
                new DeleteNoteCommand
                {
                    Id = noteId,
                    UserId = NotesContextFactory.UserBId
                }, CancellationToken.None));
    }
}