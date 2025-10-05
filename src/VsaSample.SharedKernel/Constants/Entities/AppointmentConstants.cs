namespace VsaSample.SharedKernel.Constants.Entities;

public static class AppointmentConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "Appointment";
        private const string EntityNameTr = "Randevu";

        public static Error NotFound(Guid id) => Error.NotFound(
                "Appointments.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static readonly Error InvalidTimeRange = Error.Problem(
                "Appointments.InvalidTimeRange",
                "Appointment end time must be after start time.")
            .WithDescription(Cultures.Tr, "Randevu bitiş zamanı başlangıç zamanından sonra olmalıdır.")
            .WithDescription(Cultures.En, "Appointment end time must be after start time.");
    }
}
