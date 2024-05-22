namespace Mihaylov.Api.Other.Contracts.Show.Models
{
    public class ConcertExtended : Concert
    {
        public string Location { get; set; }

        public string TicketProvider { get; set; }
    }
}
