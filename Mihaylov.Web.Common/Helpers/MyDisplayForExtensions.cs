using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class MyDisplayForExtensions
    {
        public static IHtmlString MyDisplayFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string wrapperClass = null, string unit = null, string text = null)
        {
            TagBuilder wrapper = new TagBuilder("div");
            wrapper.AddCssClass("my-display-for");

            if (!string.IsNullOrWhiteSpace(wrapperClass))
            {
                wrapper.AddCssClass(wrapperClass);
            }

            string content = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                content = htmlHelper.DisplayFor(expression).ToHtmlString();
            }
            else
            {
                content = text;
            }

            if (!string.IsNullOrWhiteSpace(unit) && !string.IsNullOrWhiteSpace(content))
            {
                content = string.Format("{0} {1}", content, unit);
            }

            wrapper.InnerHtml = htmlHelper.LabelFor(expression).ToHtmlString();
            wrapper.InnerHtml += content;

            return new HtmlString(wrapper.ToString());
        }

        //public static IHtmlString MyDisplayForFormGroup<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
        //                Expression<Func<TModel, TValue>> expression, string optionLabel = null, string unit = null, string text = null)
        //{
        //    TagBuilder wrapper = new TagBuilder("div");
        //    wrapper.AddCssClass("form-group");

        //    TagBuilder container = new TagBuilder("div");
        //    container.AddCssClass(OrderGridSearchConstants.DataClass);

        //    TagBuilder element = new TagBuilder("span");
        //    element.AddCssClass(OrderGridSearchConstants.NonEditableElementClass);
        //    element.AddCssClass(ExpressionHelper.GetExpressionText(expression));

        //    Type valueType = typeof(TValue);
        //    if (!valueType.IsEnum)
        //    {
        //        if (string.IsNullOrWhiteSpace(text))
        //        {
        //            element.InnerHtml += htmlHelper.DisplayFor(expression).ToHtmlString();
        //        }
        //        else
        //        {
        //            element.InnerHtml += text;
        //        }
        //    }
        //    else
        //    {
        //        Func<TModel, TValue> method = expression.Compile();
        //        Enum value = method(htmlHelper.ViewData.Model) as Enum;
        //        element.InnerHtml += string.Format("{0}", value.GetDescription());
        //    }

        //    if (!string.IsNullOrWhiteSpace(unit))
        //    {
        //        element.InnerHtml += string.Format(" {0}", unit);
        //    }

        //    // container.InnerHtml = htmlHelper.HiddenFor(expression).ToHtmlString();
        //    container.InnerHtml += element;

        //    wrapper.InnerHtml = htmlHelper.LabelFor(expression, optionLabel, OrderGridSearchConstants.LabelHtmlAttributes).ToHtmlString();
        //    wrapper.InnerHtml += container;

        //    return new HtmlString(wrapper.ToString());
        //}
    }
}
