using Mihaylov.Common.Extensions;
using Mihaylov.Database.Models.Enums;

namespace Mihaylov.Database
{
    public partial class Ethnicity
    {
        public EthnicityType Type
        {
            get
            {
                var type = this.Name.ToEnum<EthnicityType>();
                return type;
            }
        }
    }
}
