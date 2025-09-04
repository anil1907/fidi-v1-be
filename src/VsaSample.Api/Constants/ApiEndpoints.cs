namespace VsaSample.Api.Constants;

internal static class ApiEndpoints
{
    internal const int V1 = 1;
    internal const int V2 = 2;

    internal static string WithVersion(string endpointName, int version) => $"{endpointName}V{version}";

    internal static class Users
    {
        public const string Login = "Login";
        public const string Register = "Register";
    }

    internal static class Categories
    {
        public const string CreateCategory = "CreateCategory";
        public const string DeleteCategory = "DeleteCategory";
        public const string GetCategoriesPage = "GetCategoriesPage";
        public const string GetCategoryById = "GetCategoryById";
        public const string UpdateCategory = "UpdateCategory";
    }

    internal static class Products
    {
        public const string CreateProduct = "CreateProduct";
        public const string DeleteProduct = "DeleteProduct";
        public const string GetProductsPaged = "GetProductsPaged";
        public const string GetProductById = "GetProductById";
        public const string UpdateProduct = "UpdateProduct";
    }

    internal static class SubCategories
    {
        public const string CreateSubCategory = "CreateSubCategory";
        public const string DeleteSubCategory = "DeleteSubCategory";
        public const string GetSubCategoriesPage = "GetSubCategoriesPage";
        public const string GetSubCategoryById = "GetSubCategoryById";
        public const string UpdateSubCategory = "UpdateSubCategory";
        public const string ExportSubCategories = "ExportSubCategories";
        public const string ImportSubCategories = "ImportSubCategories";
    }

    internal static class Pokemons
    {
        public const string GetPokemons = "GetPokemons";
    }
}
