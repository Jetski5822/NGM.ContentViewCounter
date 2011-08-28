using Contrib.Voting.Services;
using NGM.ContentViewCounter.Models;
using Orchard.ContentManagement.Drivers;

namespace NGM.ContentViewCounter.Drivers {
    public class UserViewPartDriver : ContentPartDriver<UserViewPart> {
        private readonly IVotingService _votingService;

        public UserViewPartDriver(IVotingService votingService) {
            _votingService = votingService;
        }

        protected override DriverResult Display(UserViewPart part, string displayType, dynamic shapeHelper) {
            var resultRecord = _votingService.GetResult(part.ContentItem.Id, "sum", Constants.Dimension);
            part.TotalViews = resultRecord == null ? 0 : (int)resultRecord.Value;

            return Combined(ContentShape("Parts_UserView_Summary", () => shapeHelper.Parts_UserView_Summary(TotalViews: part.TotalViews)),
                ContentShape("Parts_UserView_SummaryAdmin", () => shapeHelper.Parts_UserView_SummaryAdmin(TotalViews: part.TotalViews)));
        }
    }
}