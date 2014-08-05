using System;
using System.Reflection;

namespace Microsoft.Practices.Prism
{
    public static class ViewLocator
    {
        /// <summary>
        /// Transforms a view type into a pack uri that can be resolved for navigation.
        /// </summary>
        public static Func<Type, string> DefaultViewTypeToPackUriResolver =
            viewType =>
            {
                var assemblyName = viewType.Assembly.GetAssemblyName();
                var defaultNamespace = viewType.Namespace != null
                    ? viewType.Namespace.Replace(".Views", string.Empty)
                    : viewType.Assembly.GetAssemblyName();

                var uri = viewType.FullName.Replace(defaultNamespace, string.Empty).Replace(".", "/") + ".xaml";
                return string.Format("/{0};component{1}", assemblyName, uri);
            };
    }
}
