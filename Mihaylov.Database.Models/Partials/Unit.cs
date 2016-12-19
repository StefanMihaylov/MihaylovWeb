using Mihaylov.Common.Extensions;
using Mihaylov.Database.Models.Enums;

namespace Mihaylov.Database
{
    public partial class Unit
    {
        public UnitType Type
        {
            get
            {
                var type = this.Name.ToEnum<UnitType>();
                return type;
            }
        }
    }
}
