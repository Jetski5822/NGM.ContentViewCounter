using System;
using System.Linq;
using Contrib.Voting.Services;
using NGM.ContentViewCounter.Models;
using NGM.ContentViewCounter.Settings;
using Orchard;
using Orchard.ContentManagement.Handlers;

namespace NGM.ContentViewCounter.Handlers {
    public class UserViewPartHandler : ContentHandler {
        private readonly IVotingService _votingService;
        private readonly IOrchardServices _orchardServices;

        public UserViewPartHandler(IVotingService votingService,
            IOrchardServices orchardServices) {
            _votingService = votingService;
            _orchardServices = orchardServices;

            OnGetDisplayShape<UserViewPart>((context, part) => {
                var settings = part.Settings.GetModel<UserViewTypePartSettings>();
                if (!context.DisplayType.Equals(settings.DisplayType, StringComparison.InvariantCultureIgnoreCase))
                    return;

                RecordView(part, settings);
            });

        }

        private void RecordView(UserViewPart part, UserViewTypePartSettings settings) {
            var currentUser = _orchardServices.WorkContext.CurrentUser;

            if (currentUser != null) {
                Vote(currentUser.UserName, part, settings);
            } else if (settings.AllowAnonymousViews) {
                var anonHostname = _orchardServices.WorkContext.HttpContext.Request.UserHostAddress;
                if (!string.IsNullOrWhiteSpace(_orchardServices.WorkContext.HttpContext.Request.Headers["X-Forwarded-For"]))
                    anonHostname += "-" + _orchardServices.WorkContext.HttpContext.Request.Headers["X-Forwarded-For"];

                Vote("Anonymous" + anonHostname, part, settings);
            }
        }

        private void Vote(string userName, UserViewPart part, UserViewTypePartSettings settings) {
            var currentVote = _votingService.Get(vote => 
                vote.ContentItemRecord == part.ContentItem.Record && 
                vote.Username == userName && 
                vote.Dimension == Constants.Dimension).FirstOrDefault();

            if (currentVote != null && settings.AllowMultipleViewsFromSameUserToCount)
                _votingService.ChangeVote(currentVote, (currentVote.Value + 1));
            else if (currentVote == null)
                _votingService.Vote(part.ContentItem, userName, _orchardServices.WorkContext.HttpContext.Request.UserHostAddress, 1, Constants.Dimension);
        }
    }
}