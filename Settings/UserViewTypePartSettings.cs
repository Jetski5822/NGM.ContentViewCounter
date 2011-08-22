using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace NGM.ContentViewCounter.Settings {
    public class UserViewTypePartSettings {
        private bool? _allowAnonymousViews;
        public bool AllowAnonymousViews {
            get {
                if (_allowAnonymousViews == null)
                    _allowAnonymousViews = false;
                return (bool)_allowAnonymousViews;
            }
            set { _allowAnonymousViews = value; }
        }

        private bool? _allowMultipleViewsFromSameUserToCount;
        public bool AllowMultipleViewsFromSameUserToCount {
            get {
                if (_allowMultipleViewsFromSameUserToCount == null)
                    _allowMultipleViewsFromSameUserToCount = false;
                return (bool)_allowMultipleViewsFromSameUserToCount;
            }
            set { _allowMultipleViewsFromSameUserToCount = value; }
        }

        public string DisplayType { get;set; }
    }

    public class ContainerSettingsHooks : ContentDefinitionEditorEventsBase {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition) {
            if (definition.PartDefinition.Name != "UserViewPart")
                yield break;

            var model = definition.Settings.GetModel<UserViewTypePartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.Name != "UserViewPart")
                yield break;

            var model = new UserViewTypePartSettings();
            updateModel.TryUpdateModel(model, "UserViewTypePartSettings", null, null);
            builder.WithSetting("UserViewTypePartSettings.DisplayType", model.DisplayType);
            builder.WithSetting("UserViewTypePartSettings.AllowAnonymousViews", model.AllowAnonymousViews.ToString());
            builder.WithSetting("UserViewTypePartSettings.AllowMultipleViewsFromSameUserToCount", model.AllowMultipleViewsFromSameUserToCount.ToString());

            yield return DefinitionTemplate(model);
        }
    }
}