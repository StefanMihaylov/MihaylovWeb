namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class Level
    {
        public const int NameMaxLength = 50;

        public const int DescritionMaxLength = 100;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Descrition { get; set; }
    }
}
