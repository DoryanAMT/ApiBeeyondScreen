namespace ApiBeeyondScreen.Helpers
{
    public static class HelperSeguridad
    {
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static string SecretKey { get; set; }

        public static string Salt { get; set; }
        public static string Key { get; set; }
        public static string Iterate { get; set; }

    }
}
