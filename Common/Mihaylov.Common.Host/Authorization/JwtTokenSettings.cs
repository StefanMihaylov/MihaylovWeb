namespace Mihaylov.Common.Host.Authorization
{
    public class JwtTokenSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }


        public void Copy(JwtTokenSettings other)
        {
            Secret = other.Secret;
            Issuer = other.Issuer;
            Audience = other.Audience;
        }
    }
}
