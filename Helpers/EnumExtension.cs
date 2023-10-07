using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Group20_IoT.Helpers
{
    public static class EnumExtension
    {
        public static MvcHtmlString DisplayEnumFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = metadata.ModelType;

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("The specified type is not an enum type.");
            }

            var enumValue = (Enum)metadata.Model;
            var fieldInfo = enumType.GetField(enumValue.ToString());
            var displayAttribute = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false).OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute != null)
            {
                return MvcHtmlString.Create(displayAttribute.GetName());
            }
            else
            {
                return MvcHtmlString.Create(enumValue.ToString());
            }
        }
    }
}