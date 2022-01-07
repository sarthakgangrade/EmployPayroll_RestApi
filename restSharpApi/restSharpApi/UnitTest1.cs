using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace restSharpApi
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:7000");
        }

        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);

            //act

            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<employee> dataResponse = JsonConvert.DeserializeObject<List<employee>>(response.Content);
            Assert.AreEqual(15, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }


        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Muskan");
            jObjectbody.Add("Salary", "150000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            employee dataResponse = JsonConvert.DeserializeObject<employee>(response.Content);
            Assert.AreEqual("Muskan", dataResponse.name);
            Assert.AreEqual(150000, dataResponse.Salary);

        }

        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            //making a request for a particular employee to be updated
            RestRequest request = new RestRequest("employees/13", Method.PUT);
            JsonObject jobject = new JsonObject();
            jobject.Add("name", "Clark");
            jobject.Add("salary", 120000);
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserializing content added in json file
            employee dataResponse = JsonConvert.DeserializeObject<employee>(response.Content);
            //asserting for salary
            Assert.AreEqual(dataResponse.Salary, 120000);
            //writing content without deserializing from resopnse. 
            Console.WriteLine(response.Content);
        }
        
        [TestMethod]
        public void GivenEmployee_OnDelete_ShouldReturnSuccessStatus()
        {
            //request for deleting elements from json 
            RestRequest request = new RestRequest("employees/11", Method.DELETE);
            //executing request using rest client
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            //checking status codes.
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

    }
}

