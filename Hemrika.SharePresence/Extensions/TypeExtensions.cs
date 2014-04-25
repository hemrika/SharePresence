// -----------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static bool IsStruct(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive;
        }

        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }


        public static bool HasDefaultConstructor(this Type type)
        {
            var defaultCtor = GetDefaultConstructor(type);
            return defaultCtor != null;
        }


        public static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public;


            ConstructorInfo[] ctors = type.GetConstructors(flags);


            var defaultCtor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
            return defaultCtor;
        }
    }
}
