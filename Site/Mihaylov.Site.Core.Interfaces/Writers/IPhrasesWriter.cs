using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPhrasesWriter
    {
        Phrase AddOrUpdate(Phrase inputPhrase);
    }
}