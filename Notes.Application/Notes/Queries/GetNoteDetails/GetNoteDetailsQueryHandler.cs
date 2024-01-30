using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandler(INotesDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
{
    public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Notes
            .FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);

        if (entity == null || entity.UserId != request.UserId) throw new NotFoundException(nameof(Note), request.Id);

        return mapper.Map<NoteDetailsVm>(entity);
    }
}