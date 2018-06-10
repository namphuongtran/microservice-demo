namespace IdentityServer.Entity
{
    public class Audience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        public int ExpireMinutes { get; set; }
    }
}
