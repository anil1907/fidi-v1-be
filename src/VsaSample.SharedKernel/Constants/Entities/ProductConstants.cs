namespace VsaSample.SharedKernel.Constants.Entities;

public static class ProductConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "Product";
        private const string EntityNameTr = "Ürün";

        public static Error NotFoundById(long id) => Error.NotFound(
                "Product.NotFoundById",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static Error Unauthorized() => Error.Failure(
                "Product.Unauthorized",
                CommonErrors.Unauthorized(Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.Unauthorized(Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.Unauthorized(Cultures.En));

        public static readonly Error NotFoundByName = Error.NotFound(
                "Product.NotFoundByName",
                CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundByName(EntityNameEn, EntityNameTr, Cultures.En));
    }
}

