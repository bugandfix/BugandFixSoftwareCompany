namespace BugandFixSoftwareCompany.Validations;

using BugandFixSoftwareCompany.Entity;
using FluentValidation;

public class SoftwareDeveloperValidator : AbstractValidator<SoftwareDeveloper>
{
    public SoftwareDeveloperValidator()
    {
        RuleFor(dev => dev.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(dev => dev.Specialization)
            .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters.");

        RuleFor(dev => dev.Title)
            .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

        RuleFor(dev => dev.Experience)
            .InclusiveBetween(0, 50).WithMessage("Experience must be between 0 and 50 years.");

        RuleFor(dev => dev.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(dev => dev.BirthDate)
            .LessThan(DateTime.Today).WithMessage("Birth date must be in the past.");

        RuleFor(dev => dev.LinkedInProfile)
            .MaximumLength(200).WithMessage("LinkedIn profile URL cannot exceed 200 characters.")
            .When(dev => !string.IsNullOrEmpty(dev.LinkedInProfile));
    }
}

