using FluentValidation;
using MasterCrudOp.DTOs.Requests;

namespace MasterCrudOp.Validators;

public class CreateMovieValidator : AbstractValidator<CreateMovieDto>
{
    public CreateMovieValidator()
    {
        RuleFor(m => m.Title)
             .NotEmpty().WithMessage("Title is required")
             .MinimumLength(4).WithMessage("Title must be at least 4 characters long");

        
        RuleFor(m => m.genre)
            .NotEmpty()
            .WithMessage("Genre is required");
    }
}
