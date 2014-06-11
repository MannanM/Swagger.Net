using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;

namespace Swagger.Net
{
    public class SwaggerSpec
    {
        private static readonly Dictionary<Type, string> SYSTEM_TYPE_NAMES = new Dictionary<Type, string>
        {
            { typeof(bool), "boolean" },
            { typeof(sbyte), "byte" },
            { typeof(byte), "byte" },
            { typeof(ushort), "integer" },
            { typeof(short), "integer" },
            { typeof(uint), "integer" },
            { typeof(int), "integer" },
            { typeof(ulong), "long" },
            { typeof(long), "long" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "double" },
            { typeof(char), "string" },
            { typeof(string), "string" },
            { typeof(DateTime), "Date" },
            { typeof(TimeSpan), "Date" },
            { typeof(object), "object" },
            { typeof(void), "void"},
            //I know this isn't correct but I don't want to return all the properties of this class to the user
            { typeof(HttpResponseMessage), "void"}
        };

        public static string GetDataTypeName(Type type)
        {
            if (IsEnumerable(type))
            {
                return string.Format("array[{0}]", GetDataTypeName(GetEnumerableType(type)));
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                    return string.Format("{0}?", GetDataTypeName(type.GetGenericArguments().First()));
                var genericlessName = type.Name.Remove(type.Name.IndexOf("`"));
                return string.Format("{0}[{1}]", genericlessName, string.Join(", ", type.GetGenericArguments().Select(GetDataTypeName)));
            }
            return GetNameFromSimpleType(type);
        }

        public static bool IsBasicObject(Type type) {
            return SYSTEM_TYPE_NAMES.ContainsKey(type);
        }
        public static bool IsEnumerable(Type type) {
            return type != typeof (string) && (type == typeof (IEnumerable) || type.GetInterfaces().Any(t => t == typeof (IEnumerable)));
        }
        public static Type GetEnumerableType(Type type) {
            return type.IsGenericType ? type.GetGenericArguments().First() : typeof(object);
        }
        private static string GetNameFromSimpleType(Type type)
        {
            if (SYSTEM_TYPE_NAMES.ContainsKey(type))
                return SYSTEM_TYPE_NAMES[type];
            return type.Name;
        }
    }
}