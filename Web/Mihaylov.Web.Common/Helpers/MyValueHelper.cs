using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mihaylov.Web.Common.Helpers
{
    public class MyValue : TagHelper
    {
        public string Unit { get; set; }

       // public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //if (!string.IsNullOrWhiteSpace(this.Text))
            //{
            //    output.Content.Append(this.Text);
            //}
            //else
            //{
            //   // output.Content = ;
            //}

            if(!string.IsNullOrWhiteSpace(this.Unit) && !output.Content.IsEmptyOrWhiteSpace)
            {
                output.Content.AppendFormat(" {0}", this.Unit);
            }
        }
    }
}
