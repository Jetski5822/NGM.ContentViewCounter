using System;
using NGM.ContentViewCounter.Models;
using NGM.ContentViewCounter.Services;
using NGM.ContentViewCounter.Settings;
using Orchard;
using Orchard.ContentManagement.Handlers;

namespace NGM.ContentViewCounter.Handlers {
    public class UserViewPartHandler : ContentHandler {
        private readonly IViewCounterService _viewCounterServices;
        private readonly IOrchardServices _orchardServices;

        public UserViewPartHandler(IViewCounterService viewCounterServices,
            IOrchardServices orchardServices) {
            _viewCounterServices = viewCounterServices;
            _orchardServices = orchardServices;

            OnGetDisplayShape<UserViewPart>((context, part) => {
                var settings = part.Settings.GetModel<UserViewTypePartSettings>();
                if (!context.DisplayType.Equals(settings.DisplayType, StringComparison.InvariantCultureIgnoreCase))
                    return;

                var currentUser = _orchardServices.WorkContext.CurrentUser;

                if (currentUser != null) {
                    if (!_viewCounterServices.HasViewed(part.ContentItem, currentUser.UserName) || settings.AllowMultipleViewsFromSameUserToCount)
                        _viewCounterServices.AddView(part.ContentItem, currentUser.UserName, _orchardServices.WorkContext.HttpContext.Request.UserHostAddress);
                } else if (settings.AllowAnonymousViews) {
                    if (!_viewCounterServices.HasViewed(part.ContentItem, "Anonymous") || settings.AllowMultipleViewsFromSameUserToCount) {
                        var anonHostname = _orchardServices.WorkContext.HttpContext.Request.UserHostAddress;
                        if (!string.IsNullOrWhiteSpace(_orchardServices.WorkContext.HttpContext.Request.Headers["X-Forwarded-For"]))
                            anonHostname += "-" + _orchardServices.WorkContext.HttpContext.Request.Headers["X-Forwarded-For"];

                        _viewCounterServices.AddView(part.ContentItem, "Anonymous", anonHostname);
                    }
                }
            });

        }
    }
}