using System;
using System.Collections.Generic;
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
            if (SwaggerSpec.IsEnumerable(type)) {
                //Don't bother adding an Enumerable type, just add what it is of
                InnerAdd(SwaggerSpec.GetEnumerableType(type));
            } else if (!SwaggerSpec.IsBasicObject(type)) {
                InnerAdd(type);
            }
        }

        private void InnerAdd(Type type) {
            if (!ContainsKey(type.Name)) {
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

            var fields = inputType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var info in fields) {
                result.properties.Add(info.Name, ConvertProperty(info.FieldType));
            }

            var properties = inputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var info in properties) {
                result.properties.Add(info.Name, ConvertProperty(info.PropertyType));
            }

            return result;
        }

        private object ConvertProperty(Type type) {
            if (SwaggerSpec.IsEnumerable(type)) {
                var typeArg = SwaggerSpec.GetEnumerableType(type);
                if (SwaggerSpec.IsBasicObject(typeArg)) {
                    return new ArrayProperty("type", SwaggerSpec.GetDataTypeName(typeArg));
                }
                Add(typeArg);
                return new ArrayProperty("$ref", typeArg.Name);
            }
            if (type.IsEnum) {
                return new Dictionary<string, object> {
                    {"type", "string"},
                    {"enum", Enum.GetNames(type)}
                };
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