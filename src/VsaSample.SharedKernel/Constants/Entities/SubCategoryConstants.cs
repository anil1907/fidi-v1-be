namespace VsaSample.SharedKernel.Constants.Entities;

public static class SubCategoryConstants
{
    public static class Excel
    {
        public static class Columns
        {
            public const string CategoryName = "Kategori";
            public const string Name = "Alt Kategori";
            public const string Status = "Durum";
        }
    }

    public static class Errors
    {
        private const string EntityNameEn = "Sub category";
        private const string EntityNameTr = "alt kategori";

        public static Error NotFoundById(Guid id) => Error.NotFound(
                "SubCategory.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static Error Unauthorized() => Error.Failure(
                "SubCategory.Unauthorized",
                CommonErrors.Unauthorized(Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.Unauthorized(Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.Unauthorized(Cultures.En));

        public static readonly Error NotFoundByName = Error.NotFound(
                "SubCategory.NotFoundByName",
                CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En));
    }
}
