using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Localization;

namespace NGM.ContentViewCounter {
    public class Migrations : DataMigrationImpl {
        public Migrations() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public int Create() {
            ContentDefinitionManager.AlterPartDefinition("UserViewPart", builder => builder
                .Attachable()
                .WithDescription(T("Attaches a view counter to a content type. When a user view that item the counter is increased.").Text));

            return 3;
        }

        public int UpdateFrom1() {
            SchemaBuilder.DropTable("UserViewCounterRecord");

            return 2;
        }

        public int UpdateForm2() {
            ContentDefinitionManager.AlterPartDefinition("UserViewPart", builder => builder
                .WithDescription(T("Attaches a view counter to a content type. When a user view that item the counter is increased.").Text));

            return 3;
        }
    }
}