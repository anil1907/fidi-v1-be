namespace VsaSample.SharedKernel.Constants.Entities;

public static class DietPlanConstants
{
    public static class Errors
    {
        private const string EntityNameEn = "Diet Plan";
        private const string EntityNameTr = "Beslenme Planı";

        public static Error NotFound(Guid id) => Error.NotFound(
                "DietPlans.NotFound",
                CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En))
            .WithDescription(Cultures.Tr, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.Tr))
            .WithDescription(Cultures.En, CommonErrors.EntityNotFoundById(EntityNameEn, EntityNameTr, id, Cultures.En));

        public static readonly Error InvalidDateRange = Error.Problem(
                "DietPlans.InvalidDateRange",
                "Diet plan end date must be on or after the start date.")
            .WithDescription(Cultures.Tr, "Beslenme planı bitiş tarihi başlangıç tarihinden sonra veya aynı olmalıdır.")
            .WithDescription(Cultures.En, "Diet plan end date must be on or after the start date.");
    }
}
