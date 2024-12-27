namespace Villa.Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public static string SessionToken = "JWTToken";
        public const string Role_Admin = "admin";
        public const string Role_Customer = "customer";

        /*public const string Admin = "Admin";
        public const string Customer = "Customer";*/
    }
}
