namespace VsaSample.Application.Features.Appointments;

using VsaSample.SharedKernel.Features;

public sealed class AppointmentsFeature : FeatureDefinition
{
    private AppointmentsFeature() : base("Appointment", Permissions.Instance) { }

    public static readonly AppointmentsFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("Appointments") { }

        public static readonly Permissions Instance = new();
    }
}
