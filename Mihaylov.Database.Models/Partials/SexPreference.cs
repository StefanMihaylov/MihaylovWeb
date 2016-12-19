using Mihaylov.Common.Extensions;
using Mihaylov.Database.Models.Enums;

namespace Mihaylov.Database
{
    public partial class SexPreference
    {
        public SexPreferenceType Type
        {
            get
            {
                var type = this.Name.ToEnum<SexPreferenceType>();
                return type;
            }
        }
    }
}
