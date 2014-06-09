using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swagger.Net.DataTypeModels;

namespace Swagger.Net.Tests {
    [TestClass]
    public class SwaggerDataTypeModelTest {

        [TestMethod]
        public void TestSwaggerModelConverter() {
            var result = new SwaggerDataTypeModels {typeof (ExampleModel)};

            Assert.AreEqual("{'ExampleModel':{" +
                            "'id':'ExampleModel'," +
                            //"'required':null," +
                            "'properties':{" +
                                "'Field0':{'type':'integer'}," +
                                "'Field1':{'type':'string'}," +
                                "'Field2':{'type':'array','items':{'type':'string'}}," +
                                "'Field3':{'$ref':'InnerModel'}," +
                                "'Field4':{'type':'array','items':{'$ref':'InnerModel'}}"+
                            "}},'InnerModel':{" +
                            "'id':'InnerModel'," +
                            //"'required':null," +
                            "'properties':{" +
                                "'InnerAttribute':{'type':'integer'}" +
                            "}}}",
                JsonConvert.SerializeObject(result).Replace("\"", "'"));
        }

        private class ExampleModel {
            public int Field0;
            public string Field1;
            public List<string> Field2;
            public InnerModel Field3;
            public List<InnerModel> Field4;
        }

        private class InnerModel {
            public int InnerAttribute;
        }
    }
}