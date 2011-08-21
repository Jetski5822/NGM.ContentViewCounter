using Orchard.ContentManagement;

namespace NGM.ContentViewCounter.Models {
    public class UserViewPart : ContentPart {
        public int TotalViews { get; set; }
    }
}