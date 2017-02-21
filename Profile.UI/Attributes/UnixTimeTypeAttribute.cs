using System;
using System.Web.Mvc;
using Profile.UI.Binders.PropertyBinders;

namespace Profile.UI.Attributes
{
    /// <summary>
    /// Converts unix time (in miliseconds) into DateTime and binds it with property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnixTimeBindAttribute : Attribute
    {
        public UnixTimeBindAttribute()
        {
            Binder = UnixDateBinder.GetBinder();
        }

        public IModelBinder Binder { get; set; }
    }
}
