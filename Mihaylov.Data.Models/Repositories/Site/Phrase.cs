using System;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Models.Site
{
    public class Phrase : IMapFrom<DAL.Phras>, IHaveCustomMappings
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


        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Phras, Phrase>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.PhraseId));
        }

        public void Update(DAL.Phras phrase)
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
