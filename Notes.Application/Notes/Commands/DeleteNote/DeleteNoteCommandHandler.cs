using MediatR;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandler(INotesDbContext dbContext) : IRequestHandler<DeleteNoteCommand>
{
    public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Notes
            .FindAsync([request.Id], cancellationToken);

        if (entity == null || entity.UserId != request.UserId) throw new NotFoundException(nameof(Note), request.Id);

        dbContext.Notes.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}