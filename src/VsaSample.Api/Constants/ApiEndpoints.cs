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

    internal static class Clients
    {
        public const string CreateClient = "CreateClient";
        public const string DeleteClient = "DeleteClient";
        public const string GetClientsPaged = "GetClientsPaged";
        public const string GetClientById = "GetClientById";
        public const string UpdateClient = "UpdateClient";
    }

    internal static class Appointments
    {
        public const string CreateAppointment = "CreateAppointment";
        public const string DeleteAppointment = "DeleteAppointment";
        public const string GetAppointmentsPaged = "GetAppointmentsPaged";
        public const string GetAppointmentById = "GetAppointmentById";
        public const string UpdateAppointment = "UpdateAppointment";
    }
    
    internal static class Templates
    {
        public const string CreateTemplate = "CreateTemplate";
        public const string DeleteTemplate = "DeleteTemplate";
        public const string GetTemplatesPaged = "GetTemplatesPage";
        public const string GetTemplateById = "GetTemplateById";
        public const string UpdateTemplate = "UpdateTemplate";
    }

    internal static class DietPlans
    {
        public const string CreateDietPlan = "CreateDietPlan";
        public const string DeleteDietPlan = "DeleteDietPlan";
        public const string GetDietPlansPaged = "GetDietPlansPaged";
        public const string GetDietPlanById = "GetDietPlanById";
        public const string UpdateDietPlan = "UpdateDietPlan";
    }

    internal static class Pokemons
    {
        public const string GetPokemons = "GetPokemons";
    }
}
