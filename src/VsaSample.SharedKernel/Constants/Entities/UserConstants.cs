namespace VsaSample.SharedKernel.Constants.Entities;

public static class UserConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "User";
        private const string EntityNameTr = "Kullanıcı";

        public static Error NotFound(Guid userId) => Error.NotFound(
                "Users.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, userId, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, userId, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, userId, Cultures.En));

        public static Error Unauthorized() => Error.Failure(
                "Users.Unauthorized",
                CommonErrors.Unauthorized(Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.Unauthorized(Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.Unauthorized(Cultures.En));

        public static readonly Error NotFoundByUsername = Error.NotFound(
                "Users.NotFoundByUsername",
                CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En));

        public static readonly Error EmailNotUnique = Error.Conflict(
                "Users.EmailOrUsernameNotUnique",
                "Provided email or username is not unique")
            .WithDescription(Cultures.Tr, "Sağlanan e-posta veya kullanıcı adı benzersiz değil")
            .WithDescription(Cultures.En, "Provided email or username is not unique");
    }
}
