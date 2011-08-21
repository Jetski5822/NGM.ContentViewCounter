using System.Linq;
using NGM.ContentViewCounter.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Services;

namespace NGM.ContentViewCounter.Services {
    public interface IViewCounterService : IDependency {
        void AddView(ContentItem contentItem, string userName, string hostname);
        bool HasViewed(ContentItem contentItem, string userName);
        int TotalViewsFor(ContentItem contentItem);
    }

    public class ViewCounterService : IViewCounterService {
        private readonly IRepository<UserViewCounterRecord> _viewCounterRepository;
        private readonly IClock _clock;

        public ViewCounterService(IRepository<UserViewCounterRecord> viewCounterRepository, IClock clock) {
            _viewCounterRepository = viewCounterRepository;
            _clock = clock;
        }

        public void AddView(ContentItem contentItem, string userName, string hostname) {
            var userViewCounterRecord = new UserViewCounterRecord {
                ContentItemRecord = contentItem.Record,
                ContentType = contentItem.ContentType,
                CreatedUtc = _clock.UtcNow,
                Hostname = hostname,
                Username = userName
            };

            _viewCounterRepository.Create(userViewCounterRecord);
        }

        public bool HasViewed(ContentItem contentItem, string userName) {
            if (_viewCounterRepository.Fetch(r => r.Username == userName && r.ContentItemRecord == contentItem.Record).FirstOrDefault() != null)
                return true;
            return false;
        }


        public int TotalViewsFor(ContentItem contentItem) {
            return _viewCounterRepository.Fetch(r => r.ContentItemRecord == contentItem.Record).Count();
        }
    }
}