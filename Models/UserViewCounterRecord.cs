using System;
using Orchard.ContentManagement.Records;

namespace NGM.ContentViewCounter.Models {
    public class UserViewCounterRecord {
        public virtual int Id { get; set; }
        public virtual DateTime? CreatedUtc { get; set; }
        public virtual ContentItemRecord ContentItemRecord { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string Username { get; set; }
        public virtual string Hostname { get; set; }
    }
}