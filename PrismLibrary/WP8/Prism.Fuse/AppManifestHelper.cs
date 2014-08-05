using System.Linq;
using System.Xml.Linq;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// http://sorokoletov.com/2012/05/access-to-wmappmanifest-at-runtime-in-nuget/
    /// </summary>
    internal static class AppManifestHelper
    {
        public static AppManifest Read()
        {
            var data = new AppManifest();
            var manifestXml = XElement.Load("WMAppManifest.xml");

            var appElement = manifestXml.Descendants("App").FirstOrDefault();
            if (appElement != null)
            {
                data.ProductId = (string)appElement.Attribute("ProductID");
                data.Title = (string)appElement.Attribute("Title");
                data.RuntimeType = (string)appElement.Attribute("RuntimeType");
                data.Version = (string)appElement.Attribute("Version");
                data.Genre = (string)appElement.Attribute("Genre");
                data.Author = (string)appElement.Attribute("Author");
                data.Description = (string)appElement.Attribute("Description");
                data.Publisher = (string)appElement.Attribute("Publisher");
            }

            appElement = manifestXml.Descendants("DefaultTask").FirstOrDefault();
            if (appElement != null)
            {
                data.ActivationPolicy = (string)appElement.Attribute("ActivationPolicy");
            }

            appElement = manifestXml.Descendants("PrimaryToken").FirstOrDefault();
            if (appElement != null)
            {
                data.TokenId = (string)appElement.Attribute("TokenID");
            }

            return data;
        }
    }

    public class AppManifest
    {
        public string TokenId { get; internal set; }
        public string Genre { get; internal set; }
        public string Author { get; internal set; }
        public string Description { get; internal set; }
        public string Publisher { get; internal set; }
        public string Title { get; internal set; }
        public string Version { get; internal set; }
        public string RuntimeType { get; internal set; }
        public string ProductId { get; internal set; }
        public string ActivationPolicy { get; internal set; }
    }
}
