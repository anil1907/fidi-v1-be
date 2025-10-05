namespace VsaSample.SharedKernel.Constants.Entities;

public static class TemplateConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "Template";
        private const string EntityNameTr = "Şablon";

        public static Error NotFound(Guid id) => Error.NotFound(
                "Templates.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static Error NameNotUnique(string name) => Error.Conflict(
                "Templates.NameNotUnique",
                $"Template name '{name}' is already in use")
            .WithDescription(Cultures.Tr, $"'{name}' adıyla bir şablon zaten mevcut")
            .WithDescription(Cultures.En, $"Template name '{name}' is already in use");
    }
}
