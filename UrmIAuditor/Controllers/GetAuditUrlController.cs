using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UrmIAuditor.Models;




namespace UrmIAuditor.Controllers
{
    public class GetauditurlController : ApiController
    {
		[Route("api/auditlink")]
		[HttpGet]
		

		public async Task<HttpResponseMessage> Get(string audit_id = "", string template="", string order="", string modified_after="", string modified_before = "")
		{
			if(String.IsNullOrEmpty(modified_after))
            {
                DateTime thisDay = DateTime.Today;
                modified_after = thisDay.ToString("yyyy-MM-dd");
            }
			
			var result = await GetTemplateListAsync(audit_id, template, order, modified_after, modified_before);
			if (result != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, result);
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Not Found");
			}
			
		} 
	
		public async Task<JObject> GetTemplateListAsync(string audit_id, string template, string order, string modified_after, string modified_before)
		{
			string uri = "https://api.safetyculture.io/";

			string resource = "audits/search";
			
			string parameters = null;

			if (!String.IsNullOrEmpty(audit_id) || !String.IsNullOrEmpty(template)  || !String.IsNullOrEmpty(order)  || !String.IsNullOrEmpty(modified_after) || !String.IsNullOrEmpty(modified_before))
			{
				if (!String.IsNullOrEmpty(audit_id))
					parameters += "audit_id=" + audit_id;

				if (!String.IsNullOrEmpty(template) && !String.IsNullOrEmpty(parameters))
					parameters += "&template=" + template;
				else if (!String.IsNullOrEmpty(template) && String.IsNullOrEmpty(parameters))
					parameters += "template=" + template;

				if (!String.IsNullOrEmpty(order)  && !String.IsNullOrEmpty(parameters))
					parameters += "&order=" + order;
				else if (!String.IsNullOrEmpty(order) && String.IsNullOrEmpty(parameters))
					parameters += "order=" + order;

				if (!String.IsNullOrEmpty(modified_after) && !String.IsNullOrEmpty(parameters))
                    //parameters += "&modified_after=" + modified_after;
                    
                    parameters += "&modified_after=" + modified_after;
                else if (!String.IsNullOrEmpty(modified_after) && String.IsNullOrEmpty(parameters))
					parameters += "modified_after=" + modified_after;

				if (!String.IsNullOrEmpty(modified_before) && !String.IsNullOrEmpty(parameters))
					parameters += "&modified_before=" + modified_before;
				else if (!String.IsNullOrEmpty(modified_after) && String.IsNullOrEmpty(parameters))
					parameters += "modified_before=" + modified_before;

				string urlEncodedParameter = System.Web.HttpUtility.UrlPathEncode(parameters);
				//Adding parameters to resource
				resource += "?" + urlEncodedParameter;
			}


			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(uri);
			client.DefaultRequestHeaders.Add("User-Agent", "UrmIAuditorClient/1.0");
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer 49b6d1283cb4741460956c091e04368b72e2f6932d6834ec2c2e8f6f9e6215e4");

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resource);


			
		     HttpResponseMessage response = await client.SendAsync(request);
			

			

			string jsonResponse = null;
            string jsonResponse1 = null;
            JObject json = null;
            JObject json1 = null;
            

            if (response.IsSuccessStatusCode)
			{
               
				jsonResponse = await response.Content.ReadAsStringAsync();
                AuditSearch auditSearch = new AuditSearch();
                JsonConvert.PopulateObject(jsonResponse, auditSearch);

               
                Audit[] auditList =auditSearch.audits;
                AuditLinkList auditLinkListObj = new AuditLinkList();
                List<AuditLink> auditLinkList = new List<AuditLink>();

                List<Task<HttpResponseMessage>> requests = new List<Task<HttpResponseMessage>>();
                foreach (Audit audit in auditList)
                {
                    
                    string resource1 = "/audits/" + audit.audit_id + "/deep_link";
                    
                    HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Post, resource1);


                    //HttpResponseMessage response1 = await client.SendAsync(request1);
                    requests.Add(client.SendAsync(request1));



                    //if (response1.IsSuccessStatusCode)
                    //{
                    //    jsonResponse1 = await response1.Content.ReadAsStringAsync();
                    //    AuditDetails auditDetails = new AuditDetails();
                    //    JsonConvert.PopulateObject(jsonResponse1, auditDetails);
                    //    auditDetailsList.Add(auditDetails);
                    //}
                    

                 
                }

                await Task.WhenAll(requests);

                foreach (var req in requests)
                {
                    HttpResponseMessage response1 = await req;
                   
                    if (response1.IsSuccessStatusCode)
                    {
                        jsonResponse1 = await response1.Content.ReadAsStringAsync();
                        AuditLink auditLink = new AuditLink();
                        JsonConvert.PopulateObject(jsonResponse1, auditLink);
                        auditLinkList.Add(auditLink);
                    }
                }



                auditLinkListObj.auditLink= auditLinkList;
                string auditDetailsListJsonString = JsonConvert.SerializeObject(auditLinkListObj);
                json1 = JObject.Parse(auditDetailsListJsonString);
                json = JObject.Parse(jsonResponse);

			}


			return json1;
		} 

	


	}
}
