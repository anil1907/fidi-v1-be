namespace VsaSample.SharedKernel.Constants.Entities;

public static class CategoryConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "Category";
        private const string EntityNameTr = "Kategori";

        public static Error NotFoundById(Guid id) => Error.NotFound(
                "Category.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static Error Unauthorized() => Error.Failure(
                "Category.Unauthorized",
                CommonErrors.Unauthorized(Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.Unauthorized(Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.Unauthorized(Cultures.En));

        public static readonly Error NotFoundByName = Error.NotFound(
                "Category.NotFoundByName",
                CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En));
    }

    public static class ExcelImportErrors
    {
        private static class Messages
        {
            public const string CategoryNotFoundByNameEn = "Row {0}: category '{1}' not found";
            public const string CategoryNotFoundByNameTr = "Satır {0}: '{1}' kategorisi bulunamadı";
        }

        public static Error CategoryNotFoundByName(string categoryName, int row) => Error.NotFound(
                "Category.NotFoundByName",
                string.Format(Messages.CategoryNotFoundByNameEn, row, categoryName))
            .WithDescription(Cultures.Tr, string.Format(Messages.CategoryNotFoundByNameTr, row, categoryName))
            .WithDescription(Cultures.En, string.Format(Messages.CategoryNotFoundByNameEn, row, categoryName));
    }
}
