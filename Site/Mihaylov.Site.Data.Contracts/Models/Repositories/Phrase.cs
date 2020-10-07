using System;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Phrase : IMapFrom<DAL.Phrase>
    {
        public Phrase()
        {
        }

        public Phrase(int id, string text, int? orderId)
        {
            this.Id = id;
            this.Text = text;
            this.OrderId = orderId;
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public int? OrderId { get; set; }


        public void Update(DAL.Phrase phrase)
        {
            if (!this.OrderId.HasValue)
            {
                throw new ArgumentNullException(nameof(this.OrderId));
            }

            phrase.Text = this.Text;
            phrase.OrderId = this.OrderId.Value;
        }
    }
}
