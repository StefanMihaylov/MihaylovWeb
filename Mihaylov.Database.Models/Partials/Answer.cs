using Mihaylov.Common.Extensions;
using Mihaylov.Database.Models.Enums;

namespace Mihaylov.Database
{
    public partial class Answer
    {
        public AnswerType Type
        {
            get
            {
                var type = this.Name.ToEnum<AnswerType>();
                return type;
            }
        }
    }
}
