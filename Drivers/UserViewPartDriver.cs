using NGM.ContentViewCounter.Models;
using NGM.ContentViewCounter.Services;
using Orchard.ContentManagement.Drivers;

namespace NGM.ContentViewCounter.Drivers {
    public class UserViewPartDriver : ContentPartDriver<UserViewPart> {
        private readonly IViewCounterService _viewCounterService;

        public UserViewPartDriver(IViewCounterService viewCounterService) {
            _viewCounterService = viewCounterService;
        }

        protected override DriverResult Display(UserViewPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape(
                    "Parts_UserView_SummaryAdmin",
                        () => shapeHelper.Parts_UserView_SummaryAdmin(BuildUserView(part)))
                );
        }

        private UserViewPart BuildUserView(UserViewPart part) {
            part.TotalViews = _viewCounterService.TotalViewsFor(part.ContentItem);

            return part;
        }
    }
}