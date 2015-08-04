using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static void AddError(this ModelStateDictionary self, string errorText)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString DisplayErrors(this HtmlHelper self)
        {
            if (self.ViewData.ModelState.IsValid)
                return MvcHtmlString.Empty;

            string alertClass = "warning";

            if (self.ViewData.ModelState["type"] != null)
                alertClass = self.ViewData.ModelState["type"].Errors[0].ErrorMessage;

            StringBuilder toReturn = new StringBuilder();

            toReturn.Append("<div class='alert alert-" + alertClass + "'>");
            toReturn.Append("<ul class='list-unstyled'>");

            foreach (var key in self.ViewData.ModelState.Keys)
            {
                if (self.ViewData.ModelState[key].Errors.Count == 0 || key == "type")
                    continue;

                toReturn.Append("<li>");

                foreach (var error in self.ViewData.ModelState[key].Errors)
                {
                    toReturn.Append("<p>" + error.ErrorMessage + "</p>");
                }

                toReturn.Append("</li>");
            }

            toReturn.Append("</ul>");
            toReturn.Append("</div>");

            return new MvcHtmlString(toReturn.ToString());
        }

        public static MvcHtmlString DatepickerFor<TModel, TProp>(this HtmlHelper<TModel> self,
            Expression<Func<TModel, TProp>> forExpr)
        {
            var member = forExpr.Body as MemberExpression;
            if (member == null)
                return MvcHtmlString.Empty;
            var prop = member.Member as PropertyInfo;
            if (prop == null)
                return MvcHtmlString.Empty;

            // neda mi se


            return MvcHtmlString.Empty;
        }
    }
}