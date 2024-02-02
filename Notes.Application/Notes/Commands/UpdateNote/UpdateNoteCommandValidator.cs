using FluentValidation;

namespace Notes.Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(note => note.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(note => note.Id)
            .NotEqual(Guid.Empty);
        RuleFor(note => note.Title)
            .NotEmpty()
            .MaximumLength(250);
    }
}