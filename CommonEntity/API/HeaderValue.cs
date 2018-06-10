namespace Framework.Entities.API
{
    public class HeaderValue
    {
        public string AuthorizationType { set; get; }

        public string AuthorizationValue { set; get; }

        public string SecretUser { set; get; }

        public string SecretPass { set; get; }
    }
}
