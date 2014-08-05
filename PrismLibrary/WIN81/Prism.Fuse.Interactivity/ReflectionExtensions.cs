using System;
using System.Linq;
using System.Reflection;

namespace Windows.UI.Interactivity
{
    // Summary:
    //     Marks each type of member that is defined as a derived class of MemberInfo.
    [Flags]
    internal enum MemberTypes
    {
        // Summary:
        //     Specifies that the member is a constructor, representing a System.Reflection.ConstructorInfo
        //     member. Hexadecimal value of 0x01.
        Constructor = 1,
        //
        // Summary:
        //     Specifies that the member is an event, representing an System.Reflection.EventInfo
        //     member. Hexadecimal value of 0x02.
        Event = 2,
        //
        // Summary:
        //     Specifies that the member is a field, representing a System.Reflection.FieldInfo
        //     member. Hexadecimal value of 0x04.
        Field = 4,
        //
        // Summary:
        //     Specifies that the member is a method, representing a System.Reflection.MethodInfo
        //     member. Hexadecimal value of 0x08.
        Method = 8,
        //
        // Summary:
        //     Specifies that the member is a property, representing a System.Reflection.PropertyInfo
        //     member. Hexadecimal value of 0x10.
        Property = 16,
        //
        // Summary:
        //     Specifies that the member is a type, representing a System.Reflection.MemberTypes.TypeInfo
        //     member. Hexadecimal value of 0x20.
        TypeInfo = 32,
        //
        // Summary:
        //     Specifies that the member is a custom member type. Hexadecimal value of 0x40.
        Custom = 64,
        //
        // Summary:
        //     Specifies that the member is a nested type, extending System.Reflection.MemberInfo.
        NestedType = 128,
        //
        // Summary:
        //     Specifies all member types.
        All = 191,
    }

    // Summary:
    //     Specifies flags that control binding and the way in which the search for
    //     members and types is conducted by reflection.
    [Flags]
    internal enum BindingFlags
    {
        // Summary:
        //     No binding flag.
        Default = 0,
        //
        // Summary:
        //     The case of the member name should not be considered when binding.
        IgnoreCase = 1,
        //
        // Summary:
        //     Only members declared at the level of the supplied type's hierarchy should
        //     be considered. Inherited members are not considered.
        DeclaredOnly = 2,
        //
        // Summary:
        //     Instance members should be included in the search.
        Instance = 4,
        //
        // Summary:
        //     Static members should be included in the search.
        Static = 8,
        //
        // Summary:
        //     Public members should be included in the search.
        Public = 16,
        //
        // Summary:
        //     Non-public members should be included in the search.
        NonPublic = 32,
        //
        // Summary:
        //     Public and protected static members up the hierarchy should be returned.
        //     Private static members in inherited classes are not returned. Static members
        //     include fields, methods, events, and properties. Nested types are not returned.
        FlattenHierarchy = 64,
        //
        // Summary:
        //     A method is to be invoked. This must not be a constructor or a type initializer.
        InvokeMethod = 256,
        //
        // Summary:
        //     Reflection should create an instance of the specified type. This flag calls
        //     the constructor that matches the given arguments. The supplied member name
        //     is ignored. If the type of lookup is not specified, (Instance | Public) will
        //     apply. It is not possible to call a type initializer.
        CreateInstance = 512,
        //
        // Summary:
        //     The value of the specified field should be returned.
        GetField = 1024,
        //
        // Summary:
        //     The value of the specified field should be set.
        SetField = 2048,
        //
        // Summary:
        //     The value of the specified property should be returned.
        GetProperty = 4096,
        //
        // Summary:
        //     The value of the specified property should be set. For COM properties, specifying
        //     this binding flag is equivalent to specifying PutDispProperty and PutRefDispProperty.
        SetProperty = 8192,
        //
        // Summary:
        //     The PROPPUT member on a COM object should be invoked. PROPPUT specifies a
        //     property-setting function that uses a value. Use PutDispProperty if a property
        //     has both PROPPUT and PROPPUTREF and you need to distinguish which one is
        //     called.
        PutDispProperty = 16384,
        //
        // Summary:
        //     The PROPPUTREF member on a COM object should be invoked. PROPPUTREF specifies
        //     a property-setting function that uses a reference instead of a value. Use
        //     PutRefDispProperty if a property has both PROPPUT and PROPPUTREF and you
        //     need to distinguish which one is called.
        PutRefDispProperty = 32768,
        //
        // Summary:
        //     Types of the supplied arguments must exactly match the types of the corresponding
        //     formal parameters. Reflection throws an exception if the caller supplies
        //     a non-null Binder object, because that implies that the caller is supplying
        //     BindToXXX implementations that will pick the appropriate method. The default
        //     binder ignores this flag, whereas custom binders can implement the semantics
        //     of this flag.
        ExactBinding = 65536,
        //
        // Summary:
        //     Not implemented.
        SuppressChangeType = 131072,
        //
        // Summary:
        //     The set of members whose parameter count matches the number of supplied arguments
        //     should be returned. This binding flag is used for methods with parameters
        //     that have default values and methods with variable arguments (varargs). This
        //     flag should only be used with the System.Type.InvokeMember(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Object,System.Object[],System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[])
        //     method. Parameters with default values are used only in calls where trailing
        //     arguments are omitted. They must be the last arguments.
        OptionalParamBinding = 262144,
        //
        // Summary:
        //     Used in COM interop to specify that the return value of the member can be
        //     ignored.
        IgnoreReturn = 16777216,
    }

    internal static partial class ReflectionExtensions
    {
        public const BindingFlags Default = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
    }

    internal static partial class ReflectionExtensions
    {
        public static bool IsAssignableFrom(this Type type, Type anotherType)
        {
            return type.GetTypeInfo().IsAssignableFrom(anotherType.GetTypeInfo());
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            // TypeCode is not in metro ... f'in pain - do we need this stuff??
            if (type == null)
                return TypeCode.Empty;
            if (type.GetTypeInfo().IsEnum)
                return TypeCode.Object;
            if (type == typeof(byte))
                return TypeCode.Byte;
            if (type == typeof(sbyte))
                return TypeCode.SByte;
            if (type == typeof(Int16))
                return TypeCode.Int16;
            if (type == typeof(Int32))
                return TypeCode.Int32;
            if (type == typeof(Int64))
                return TypeCode.Int64;
            if (type == typeof(UInt16))
                return TypeCode.UInt16;
            if (type == typeof(UInt32))
                return TypeCode.UInt32;
            if (type == typeof(UInt64))
                return TypeCode.UInt64;
            if (type == typeof(Single))
                return TypeCode.Single;
            if (type == typeof(Double))
                return TypeCode.Double;
            if (type == typeof(Decimal))
                return TypeCode.Decimal;
            return TypeCode.Empty;      // ok not true, but too lazy to enumerate them all
        }

        public static MemberTypes GetMemberType(this MemberInfo member)
        {
            if (member is FieldInfo)
                return MemberTypes.Field;
            if (member is ConstructorInfo)
                return MemberTypes.Constructor;
            if (member is PropertyInfo)
                return MemberTypes.Property;
            if (member is EventInfo)
                return MemberTypes.Event;
            if (member is MethodInfo)
                return MemberTypes.Method;

            var typeInfo = member as TypeInfo;
            if (typeInfo != null && (!typeInfo.IsPublic && !typeInfo.IsNotPublic))
                return MemberTypes.NestedType;

            return MemberTypes.TypeInfo;
        }

        public static ConstructorInfo GetConstructor(this Type type, Type[] paramTypes)
        {
            return GetConstructors(type, ReflectionExtensions.Default).FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(paramTypes));
        }

        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            return GetConstructors(type, ReflectionExtensions.Default);
        }

        public static ConstructorInfo[] GetConstructors(this Type type, BindingFlags flags)
        {
            var props = type.GetTypeInfo().DeclaredConstructors;
            return props.Where(p =>
              ((flags.HasFlag(BindingFlags.Static) == p.IsStatic) ||
               (flags.HasFlag(BindingFlags.Instance) == !p.IsStatic)
              ) &&
              ((flags.HasFlag(BindingFlags.Public) == p.IsPublic) ||
                (flags.HasFlag(BindingFlags.NonPublic) == p.IsPrivate)
              )).ToArray();
        }

        public static EventInfo GetEvent(this Type type, string name)
        {
            return GetEvent(type, name, ReflectionExtensions.Default);
        }

        public static EventInfo GetEvent(this Type type, string name, BindingFlags flags)
        {
            return GetEvents(type, flags).FirstOrDefault(f => f.Name == name);
        }

        public static EventInfo[] GetEvents(this Type type)
        {
            return GetEvents(type, ReflectionExtensions.Default);
        }

        public static EventInfo[] GetEvents(this Type type, BindingFlags flags)
        {
            var props = flags.HasFlag(BindingFlags.DeclaredOnly) ? type.GetTypeInfo().DeclaredEvents : type.GetRuntimeEvents();
            //todo - this is probably not correct
            // also assumes only the getter matters ..
            return props.Where(p =>
              ((flags.HasFlag(BindingFlags.Static) == p.AddMethod.IsStatic) ||
               (flags.HasFlag(BindingFlags.Instance) == !p.AddMethod.IsStatic)
              ) &&
              ((flags.HasFlag(BindingFlags.Public) == p.AddMethod.IsPublic) ||
                (flags.HasFlag(BindingFlags.NonPublic) == p.AddMethod.IsPrivate)
              )).ToArray();
        }

        public static FieldInfo GetField(this Type type, string name)
        {
            return GetField(type, name, ReflectionExtensions.Default);
        }

        public static FieldInfo GetField(this Type type, string name, BindingFlags flags)
        {
            return GetFields(type, flags).FirstOrDefault(f => f.Name == name);
        }

        public static FieldInfo[] GetFields(this Type type)
        {
            return GetFields(type, ReflectionExtensions.Default);
        }

        public static FieldInfo[] GetFields(this Type type, BindingFlags flags)
        {
            var fields = flags.HasFlag(BindingFlags.DeclaredOnly) ? type.GetTypeInfo().DeclaredFields : type.GetRuntimeFields();
            //todo - this is probably not correct
            // also assumes only the getter matters ..
            return fields.Where(p =>
              ((flags.HasFlag(BindingFlags.Static) == p.IsStatic) || (flags.HasFlag(BindingFlags.Instance) == !p.IsStatic)
              ) &&
              ((flags.HasFlag(BindingFlags.Public) == p.IsPublic) || (flags.HasFlag(BindingFlags.NonPublic) == p.IsPrivate)
              )).ToArray();
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }

        public static MemberInfo[] GetMember(this Type type, string name)
        {
            return GetMember(type, name, ReflectionExtensions.Default);
        }
        public static MemberInfo[] GetMember(this Type type, string name, BindingFlags flags)
        {
            return GetMembers(type, flags).Where(m => m.Name == name).ToArray();
        }
        //public MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr);

        public static MemberInfo[] GetMembers(this Type type)
        {
            return GetMembers(type, ReflectionExtensions.Default);
        }
        public static MemberInfo[] GetMembers(this Type type, BindingFlags flags)
        {
            // Metro does have DeclaredMembers but nothing otherwise
            return GetEvents(type, flags).Cast<MemberInfo>()
              .Concat(GetFields(type, flags))
              .Concat(GetMethods(type, flags))
              .Concat(GetProperties(type, flags))
              .ToArray();
        }

        public static MethodInfo GetMethod(this Type type, string name)
        {
            return GetMethod(type, name, ReflectionExtensions.Default);
        }
        public static MethodInfo GetMethod(this Type type, string name, BindingFlags flags)
        {
            return GetMethods(type, flags).FirstOrDefault(m => m.Name == name);
        }

        public static MethodInfo[] GetMethods(this Type type)
        {
            return GetMethods(type, ReflectionExtensions.Default);
        }

        public static MethodInfo[] GetMethods(this Type type, BindingFlags flags)
        {
            var methods = flags.HasFlag(BindingFlags.DeclaredOnly) ? type.GetTypeInfo().DeclaredMethods : type.GetRuntimeMethods();
            //todo - this is probably not correct
            return methods.Where(m =>
              ((flags.HasFlag(BindingFlags.Static) == m.IsStatic) || (flags.HasFlag(BindingFlags.Instance) == !m.IsStatic)
              ) &&
              ((flags.HasFlag(BindingFlags.Public) == m.IsPublic) || (flags.HasFlag(BindingFlags.NonPublic) == m.IsPrivate)
              )).ToArray();
        }

        public static Type GetNestedType(this Type type, string name, BindingFlags flags)
        {
            // todo - flags are ignored
            var ti = type.GetTypeInfo().DeclaredNestedTypes.FirstOrDefault(t => t.Name == name);
            return ti == null ? null : ti.AsType();
        }

        public static PropertyInfo[] GetProperties(this Type type)
        {
            return GetProperties(type, ReflectionExtensions.Default);
        }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            var props = flags.HasFlag(BindingFlags.DeclaredOnly) ? type.GetTypeInfo().DeclaredProperties : type.GetRuntimeProperties();
            //todo - this is probably not correct
            // also assumes only the getter matters ..
            return props.Where(p =>
              ((flags.HasFlag(BindingFlags.Static) == (p.GetMethod != null && p.GetMethod.IsStatic)) ||
                (flags.HasFlag(BindingFlags.Instance) == (p.GetMethod != null && !p.GetMethod.IsStatic))
              ) &&
              ((flags.HasFlag(BindingFlags.Public) == (p.GetMethod != null && p.GetMethod.IsPublic)) ||
                (flags.HasFlag(BindingFlags.NonPublic) == (p.GetMethod != null && p.GetMethod.IsPrivate))
              )).ToArray();
        }

        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return GetProperty(type, name, ReflectionExtensions.Default);
        }

        public static PropertyInfo GetProperty(this Type type, string name, BindingFlags flags)
        {
            return GetProperties(type, flags).FirstOrDefault(p => p.Name == name);
        }
    }
}
