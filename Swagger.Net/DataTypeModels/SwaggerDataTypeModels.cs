using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Swagger.Net.DataTypeModels {
    /// <summary>
    /// Represents the data structure of the objects used.
    /// </summary>
    public class SwaggerDataTypeModels : Dictionary<string, SwaggerDataTypeDescription> {
        /// <summary>
        /// Add a type to be displayed
        /// </summary>
        /// <param name="type">Type to add</param>
        public void Add(Type type) {
            if (!ContainsKey(type.Name) && !SwaggerSpec.IsBasicObject(type)) {
                //This looks stupid but you need to reserve the name before converting to prevent stack overflow
                Add(type.Name, new SwaggerDataTypeDescription());
                this[type.Name] = Convert(type);
            }
        }

        private SwaggerDataTypeDescription Convert(Type inputType) {
            var result = new SwaggerDataTypeDescription {
                id = inputType.Name,
                properties = new Dictionary<string, object>()
            };

            var properties = inputType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var info in properties) {
                result.properties.Add(info.Name, ConvertProperty(info.FieldType));
            }

            return result;
        }

        private object ConvertProperty(Type type) {
            if (SwaggerSpec.IsEnumerable(type)) {
                var typeArg = type.IsGenericType ? type.GetGenericArguments().First() : typeof (object);
                if (SwaggerSpec.IsBasicObject(typeArg)) {
                    return new ArrayProperty("type", SwaggerSpec.GetDataTypeName(typeArg));
                }
                Add(typeArg);
                return new ArrayProperty("$ref", typeArg.Name);
            }
            if (!SwaggerSpec.IsBasicObject(type)) {
                Add(type);
                return new Dictionary<string, object> {{"$ref", type.Name}};
            }
            return new Dictionary<string, object> {
                {"type", SwaggerSpec.GetDataTypeName(type)}
            };
        }

        private class ArrayProperty {
            public string type = "array";
            public readonly Dictionary<string, string> items = new Dictionary<string, string>();

            public ArrayProperty(string key, string value) {
                items.Add(key, value);
            }
        }
    }
}