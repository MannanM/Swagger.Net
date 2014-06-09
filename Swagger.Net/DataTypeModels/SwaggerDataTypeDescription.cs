using System.Collections.Generic;

namespace Swagger.Net.DataTypeModels {
    /// <summary>
    /// The required fields for a Swagger model
    /// </summary>
    public class SwaggerDataTypeDescription {
        public string id;
        public Dictionary<string, object> properties;
        //Not yet implemented
        //public string[] required;
    }
}
