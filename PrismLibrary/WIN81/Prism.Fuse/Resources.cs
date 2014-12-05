using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.Properties
{
    internal static class Resources
    {
        public static string NullLoggerFacadeException
        {
            get
            {
                return Resources.GetString("NullLoggerFacadeException");
            }
        }

        public static string NullSuspensionManagerException
        {
            get
            {
                return Resources.GetString("NullSuspensionManagerException");
            }
        }

        public static string InitializingShell
        {
            get
            {
                return Resources.GetString("InitializingShell");
            }
        }

        public static string CreatingContainer
        {
            get
            {
                return Resources.GetString("CreatingContainer");
            }
        }

        public static string NullContainerException
        {
            get
            {
                return Resources.GetString("NullContainerException");
            }
        }

        public static string ConfiguringIocContainer
        {
            get
            {
                return Resources.GetString("ConfiguringIocContainer");
            }
        }

        public static string ConfiguringServiceLocatorSingleton
        {
            get
            {
                return Resources.GetString("ConfiguringServiceLocatorSingleton");
            }
        }

        public static string BootstrapperSequenceCompleted
        {
            get
            {
                return Resources.GetString("BootstrapperSequenceCompleted");
            }
        }

        public static string TypeMappingAlreadyRegistered
        {
            get
            {
                return Resources.GetString("TypeMappingAlreadyRegistered");
            }
        }

        public static string DelegateCommandDelegatesCannotBeNull
        {
            get
            {
                return Resources.GetString("DelegateCommandDelegatesCannotBeNull");
            }
        }

        public static string DelegateCommandInvalidGenericPayloadType
        {
            get
            {
                return Resources.GetString("DelegateCommandInvalidGenericPayloadType");
            }
        }

        public static string MemberExpression
        {
            get
            {
                return Resources.GetString("MemberExpression");
            }
        }

        public static string PropertyExpression
        {
            get
            {
                return Resources.GetString("PropertyExpression");
            }
        }

        public static string ConstantExpression
        {
            get
            {
                return Resources.GetString("ConstantExpression");
            }
        }

        public static string CannotRegisterCompositeCommandInItself
        {
            get
            {
                return Resources.GetString("CannotRegisterCompositeCommandInItself");
            }
        }

        public static string CannotRegisterSameCommandTwice
        {
            get
            {
                return Resources.GetString("CannotRegisterSameCommandTwice");
            }
        }

        public static string PropertySupportNotMemberAccessExpressionException
        {
            get
            {
                return Resources.GetString("PropertySupport_NotMemberAccessExpression_Exception");
            }
        }

        public static string PropertySupportExpressionNotPropertyException
        {
            get
            {
                return Resources.GetString("PropertySupport_ExpressionNotProperty_Exception");
            }
        }

        public static string PropertySupportStaticExpressionException
        {
            get
            {
                return Resources.GetString("PropertySupport_StaticExpression_Exception");
            }
        }

        public static string GetString(string resourceName)
        {
            return ResourceLoader.GetForCurrentView("Prism.Fuse/Resources").GetString(resourceName);
        }
    }
}
