using System;
using System.Web.Mvc;

namespace Profile.UI.Binders.PropertyBinders
{
    public class UnixDateBinder : IModelBinder
    {
        private static UnixDateBinder _binder;

        private UnixDateBinder()
        {
        }

        public static UnixDateBinder GetBinder()
        {
            if (_binder == null)
            {
                _binder = new UnixDateBinder();
            }

            return _binder;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProvider = bindingContext.ValueProvider;
            var dateValueResult = valueProvider.GetValue(bindingContext.ModelName);

            long unixTimeInMs;
            DateTime resultDate = DateTime.MinValue;
            var isUnixTimeInMs = long.TryParse(dateValueResult.AttemptedValue, out unixTimeInMs);

            if (isUnixTimeInMs)
            {
                resultDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTimeInMs);
            }

            return resultDate;
        }
    }
}