namespace VsaSample.Application.Features.Appointments.Update;

internal sealed class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
{
    public UpdateAppointmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(CommonErrors.PropNameNotEmpty("Title"))
            .MaximumLength(256).WithMessage(CommonErrors.PropNameMaxLength("Title", 256));

        RuleFor(x => x.EndsAt)
            .GreaterThan(x => x.StartsAt)
            .WithMessage("Appointment end time must be after start time.");

        RuleFor(x => x.Description)
            .MaximumLength(1024).When(x => x.Description is not null)
            .WithMessage(CommonErrors.PropNameMaxLength("Description", 1024));

        RuleFor(x => x.Status).IsInEnum();
    }
}
