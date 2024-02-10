namespace Mihaylov.Users.Models.Responses
{
    public class SchemeModel
    {
        /// <summary>
        /// The name of the authentication scheme.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The display name for the scheme. Null is valid and used for non user facing schemes.
        /// </summary>
        public string DisplayName { get; set; }

        public string TypeName { get; set; }
    }
}
