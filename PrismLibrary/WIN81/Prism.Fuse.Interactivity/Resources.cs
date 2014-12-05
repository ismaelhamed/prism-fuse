using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.Interactivity
{
    internal static class Resources
    {
        public static string CannotHostBehaviorMultipleTimesExceptionMessage
        {
            get
            {
                return Resources.GetString("CannotHostBehaviorMultipleTimesExceptionMessage");
            }
        }

        public static string TypeConstraintViolatedExceptionMessage
        {
            get
            {
                return Resources.GetString("TypeConstraintViolatedExceptionMessage");
            }
        }

        public static string GetString(string resourceName)
        {
            return ResourceLoader.GetForCurrentView("Prism.Fuse.Interactivity/Resources").GetString(resourceName);
        }
    }
}
