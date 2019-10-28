namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public static class ProviderRegistrationsConfigurationKeys
    {
        public const string Encoding = "SFA.DAS.Encoding";

        public const string ProviderRegistrations = "SFA.DAS.ProviderRegistrations";

        public static string AuthenticationSettings = $"{ProviderRegistrations}:AuthenticationSettings";
    }
}