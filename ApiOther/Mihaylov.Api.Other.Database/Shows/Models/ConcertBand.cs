namespace Mihaylov.Api.Other.Database.Shows.Models
{
    public class ConcertBand
    {
        public int BandId { get; set; }

        public Band Band { get; set; }

        public int ConcertId { get; set; }

        public Concert Concert { get; set; }

        public int Order {  get; set; }
    }
}
