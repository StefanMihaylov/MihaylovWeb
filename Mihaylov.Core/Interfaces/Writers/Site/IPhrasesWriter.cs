using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPhrasesWriter
    {
        Phrase AddOrUpdate(Phrase inputPhrase);
    }
}