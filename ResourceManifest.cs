using Orchard.UI.Resources;

namespace NGM.ContentViewCounter {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("NGM.ContentViewCounter").SetUrl("NGM.ContentViewCounter.css");
        }
    }
}
