using FluentValidation;
using LW_4_3_5_Daryev_PI231.DTOs;

namespace LW_4_3_5_Daryev_PI231.Validators
{
    public class CreateAssetDTOValidator : AbstractValidator<CreateAssetDTO>
    {
        public CreateAssetDTOValidator()
        {
            RuleFor(asset => asset.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(asset => asset.Type)
                .IsInEnum().WithMessage("Invalid device type.");
            RuleFor(asset => asset.HourlyRate).NotEmpty().WithMessage("Hourly rate is required.")
                .LessThanOrEqualTo(DateTime.MaxValue).WithMessage("Hourly rate must be a valid date time.");
        }
    }
}
