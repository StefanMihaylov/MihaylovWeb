using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Concerts
{
    public class ConcertMainModel
    {
        public ConcertExtendedGrid Concerts { get; set; }

        public BandExtendedGrid Bands { get; set; }        

        public LocationGrid Locations { get; set; }

        public TicketProviderGrid TicketProviders { get; set; }

        public AddConcertVewModel Input { get; set;}
    }
}
