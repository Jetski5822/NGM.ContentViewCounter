using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace NGM.ContentViewCounter {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("UserViewCounterRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<DateTime>("CreatedUtc")
                    .Column<int>("ContentItemRecord_id")
                    .Column<string>("ContentType")
                    .Column<string>("Username")
                    .Column<string>("Hostname")
                );

            ContentDefinitionManager.AlterPartDefinition("UserViewPart", builder => builder.Attachable());

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.DropTable("UserViewCounterRecord");

            return 2;
        }
    }
}