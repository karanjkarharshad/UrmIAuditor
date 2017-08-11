using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UrmIAuditor.Models;
using System.Linq;




namespace UrmIAuditor.Controllers
{
    public class AuditsearchController : ApiController
    {
		[Route("api/auditsearch")]
		[HttpGet]
		

		public async Task<HttpResponseMessage> Get(string template="", string order="", string modified_after="", string modified_before = "",string completed="")
		{

            DateTime thisDay = DateTime.Today;
            //If modified_after="", by default, it has been assumed to get the data for Today
            if (String.IsNullOrEmpty(modified_after))
            {

                modified_after = thisDay.ToString("yyyy-MM-dd");
            }
            //If modified_after="Week", get data for Last 7 Days
            else if (modified_after == "Week")
            {

                DateTime sevenDaysEarlier = thisDay.AddDays(-7);
                modified_after = sevenDaysEarlier.ToString("yyyy-MM-dd");

            }
            
			var result = await GetTemplateListAsync(template, order, modified_after, modified_before,completed);
			if (result != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, result);
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Not Found");
			}
			
		} 
	
		public async Task<JObject> GetTemplateListAsync(string template, string order, string modified_after, string modified_before,string completed)
		{
			string uri = "https://api.safetyculture.io/";

			string resource = "audits/search";
			
			string parameters = null;
            //Create the Query String for Searching Audits
			if (!String.IsNullOrEmpty(template)  || !String.IsNullOrEmpty(order)  || !String.IsNullOrEmpty(modified_after) || !String.IsNullOrEmpty(modified_before) || !String.IsNullOrEmpty(completed))
			{
			

				if (!String.IsNullOrEmpty(template) && !String.IsNullOrEmpty(parameters))
					parameters += "&template=" + template;
				else if (!String.IsNullOrEmpty(template) && String.IsNullOrEmpty(parameters))
					parameters += "template=" + template;

				if (!String.IsNullOrEmpty(order)  && !String.IsNullOrEmpty(parameters))
					parameters += "&order=" + order;
				else if (!String.IsNullOrEmpty(order) && String.IsNullOrEmpty(parameters))
					parameters += "order=" + order;

				if (!String.IsNullOrEmpty(modified_after) && !String.IsNullOrEmpty(parameters))
                    parameters += "&modified_after=" + modified_after;
                else if (!String.IsNullOrEmpty(modified_after) && String.IsNullOrEmpty(parameters))
					parameters += "modified_after=" + modified_after;

				if (!String.IsNullOrEmpty(modified_before) && !String.IsNullOrEmpty(parameters))
					parameters += "&modified_before=" + modified_before;
				else if (!String.IsNullOrEmpty(modified_before) && String.IsNullOrEmpty(parameters))
					parameters += "modified_before=" + modified_before;

                if (!String.IsNullOrEmpty(completed) && !String.IsNullOrEmpty(parameters))
                    parameters += "&completed=" + completed;
                else if (!String.IsNullOrEmpty(completed) && String.IsNullOrEmpty(parameters))
                    parameters += "completed=" + completed;

                string urlEncodedParameter = System.Web.HttpUtility.UrlPathEncode(parameters);
				//Adding parameters to resource
				resource += "?" + urlEncodedParameter;
			}

            //Preparing the Http Client. This is client Object has been reused for all the Safety Culture API calls
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(uri);
			client.DefaultRequestHeaders.Add("User-Agent", "UrmIAuditorClient/1.0");
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer 49b6d1283cb4741460956c091e04368b72e2f6932d6834ec2c2e8f6f9e6215e4");

            // Request for calling "https://api.safetyculture.io/audits/search"
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resource);
            //Get Reponse from "https://api.safetyculture.io/audits/search"
            HttpResponseMessage response = await client.SendAsync(request);
			
            //Initialise Variables
			string jsonResponse = null;
            string jsonResponseString1 = null;
            //JObject json = null;
            JObject json1 = null;
            

            if (response.IsSuccessStatusCode)
			{
                //Read Reponse from "https://api.safetyculture.io/audits/search"              
                jsonResponse = await response.Content.ReadAsStringAsync();
                AuditSearch auditSearch = new AuditSearch();
                JsonConvert.PopulateObject(jsonResponse, auditSearch);

               
                Audit[] auditList =auditSearch.audits;
                AuditDetailsList auditDetailsListObj = new AuditDetailsList();

                Dictionary<string, AuditDetails> AuditDetailsDic = new Dictionary<string, AuditDetails>();


                List<Task<HttpResponseMessage>> auditDetailsRequests = new List<Task<HttpResponseMessage>>();
                List<Task<HttpResponseMessage>> auditLinkRequests = new List<Task<HttpResponseMessage>>();
                foreach (Audit audit in auditList)
                {
                    // Request for calling "https://api.safetyculture.io/audits/{audit_id}"
                    string resource1 = "/audits/" + audit.audit_id;
                    
                    HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Get, resource1);
                    auditDetailsRequests.Add(client.SendAsync(request1));

                    // Request for calling "https://api.safetyculture.io/audits/{audit_id}/deep_link"
                    string resource2 = "/audits/" + audit.audit_id + "/deep_link";
                    HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Post, resource2);
                    auditLinkRequests.Add(client.SendAsync(request2));

                }
                //Wait for all the above Tasks to be completed
                await Task.WhenAll(auditDetailsRequests);
                await Task.WhenAll(auditLinkRequests);

                //Work on the response from "https://api.safetyculture.io/audits/{audit_id}"
                foreach (var req in auditDetailsRequests)
                {
                    HttpResponseMessage response1 = await req;
                    if (response1.IsSuccessStatusCode)
                    {
                        jsonResponseString1 = await response1.Content.ReadAsStringAsync();

                        //Get Audit_Id from the Response Message
                        string reqAbsoluteUri = response1.RequestMessage.RequestUri.AbsolutePath;
                        string[] reqtokenizeAbsoluteUri = reqAbsoluteUri.Split('/');
                        string reqAuditId = reqtokenizeAbsoluteUri[2];

                        //Create AuditDetails Json Object
                        AuditDetails auditDetails = new AuditDetails();
                        JsonConvert.PopulateObject(jsonResponseString1, auditDetails);
                        
                        //Add AuditDetails Json Object to Dictionary(HashMap)
                        AuditDetailsDic.Add(reqAuditId, auditDetails);
                    }
                }

                //Work on the response from "https://api.safetyculture.io/audits/{audit_id}/deep_link"
                foreach (var req in auditLinkRequests)
                {
                    HttpResponseMessage response1 = await req;
                    if (response1.IsSuccessStatusCode)
                    {
                        jsonResponseString1 = await response1.Content.ReadAsStringAsync();

                        //Get Audit_Id from the Response Message
                        string reqAbsoluteUri = response1.RequestMessage.RequestUri.AbsolutePath;
                        string[] reqtokenizeAbsoluteUri= reqAbsoluteUri.Split('/');
                        string reqAuditId = reqtokenizeAbsoluteUri[2];

                        //Create AuditLink Json Object
                        AuditLink auditLink = new AuditLink();
                        JsonConvert.PopulateObject(jsonResponseString1, auditLink);

                        //Add AuditLink Json Object to Dictionary(HashMap)
                        AuditDetails obj = AuditDetailsDic[reqAuditId];
                        obj.audit_url = auditLink.url;
                        
                    }
                }

                //Map the Dictionary to AuditDetailsList
                auditDetailsListObj.auditDetails = AuditDetailsDic.Values.ToList();

                //SerializeObject to string
                string auditDetailsListJsonString = JsonConvert.SerializeObject(auditDetailsListObj);
                json1 = JObject.Parse(auditDetailsListJsonString);
                

			}


			return json1;
		} 

	


	}
}
