using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace Profile.UI.Identity
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUser CurrentUser => new AppUser(User);
    }

    [SuppressMessage(category: "StyleCop.CSharp.MaintainabilityRules", 
                     checkId: "SA1402:FileMayOnlyContainASingleClass", 
                     Justification = "We must have two classes with same names together to be able to specify these classes into Web.config")]
    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}