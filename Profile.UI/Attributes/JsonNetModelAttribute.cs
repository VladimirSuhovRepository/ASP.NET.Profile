using System;
using System.Web.Mvc;
using Profile.UI.Binders;

namespace Profile.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface |
                    AttributeTargets.Struct | AttributeTargets.Property,
                    AllowMultiple = false, Inherited = true)]
    public class JsonNetModelAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonNetModelBinder();
        }
    }
}
