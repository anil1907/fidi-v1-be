namespace VsaSample.SharedKernel.Constants.Entities;

public static class ClientConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "Client";
        private const string EntityNameTr = "Danışan";

        public static Error NotFound(Guid id) => Error.NotFound(
                "Clients.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static readonly Error EmailAlreadyExists = Error.Conflict(
                "Clients.EmailAlreadyExists",
                "Email is already in use")
            .WithDescription(Cultures.Tr, "E-posta zaten kullanımda")
            .WithDescription(Cultures.En, "Email is already in use");
    }
}
