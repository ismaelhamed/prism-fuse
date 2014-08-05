using System;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.Interactivity
{
    internal static class ResourceHelper
    {
        public static String CannotHostBehaviorMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CannotHostBehaviorMultipleTimesExceptionMessage");
            }
        }

        public static String TypeConstraintViolatedExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("TypeConstraintViolatedExceptionMessage");
            }
        }

        public static String GetString(String resourceName)
        {
            return ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactions/Strings").GetString(resourceName);
        }
    }
}
