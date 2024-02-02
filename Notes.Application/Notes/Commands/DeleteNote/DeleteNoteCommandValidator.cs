using FluentValidation;

namespace Notes.Application.Notes.Commands.DeleteNote;

public class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteNoteCommandValidator()
    {
        RuleFor(note => note.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(note => note.Id)
            .NotEqual(Guid.Empty);
    }
}