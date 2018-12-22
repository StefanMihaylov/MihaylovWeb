using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Mihaylov.Web.Common.Helper
{
    public static class MyDisplayForExtensions
    {
        public static IHtmlContent MyDisplayFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string wrapperClass = null, string unit = null, string text = null)
        {
            TagBuilder wrapper = new TagBuilder("div");
            wrapper.AddCssClass("my-display-for");

            if (!string.IsNullOrWhiteSpace(wrapperClass))
            {
                wrapper.AddCssClass(wrapperClass);
            }

            wrapper.InnerHtml.AppendHtml(htmlHelper.LabelFor(expression));
            wrapper.InnerHtml.AppendHtml(htmlHelper.MyValueFor(expression, unit, text));

            return new HtmlString(wrapper.ToString());
        }

        public static IHtmlContent MyValueFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string unit = null, string text = null)
        {
            string content = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                content = htmlHelper.DisplayFor(expression).ToString();
            }
            else
            {
                content = text;
            }

            if (!string.IsNullOrWhiteSpace(unit) && !string.IsNullOrWhiteSpace(content))
            {
                content = string.Format("{0} {1}", content, unit);
            }

            return new HtmlString(content);
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
