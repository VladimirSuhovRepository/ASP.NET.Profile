using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Profile.UI.Infrastructure
{
    public class MultiplePartialViewResult : ActionResult
    {
        private readonly IList<PartialViewResult> _partialViews;

        public MultiplePartialViewResult()
        {
            _partialViews = new List<PartialViewResult>();
        }

        public IList<PartialViewResult> PartialViews => _partialViews;

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            foreach (var partialView in _partialViews)
            {
                partialView.ExecuteResult(context);
            }
        }
    }
}