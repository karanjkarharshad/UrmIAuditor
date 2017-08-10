using System;
using System.Net;
using System.Net.Http;

using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;



namespace UrmIAuditor.Controllers
{
    public class TemplatesearchController : ApiController
    {
		[Route("api/templatesearch")]
		[HttpGet]
		/*public async Task<JObject> GetTemp()
		{

			var result = await GetTemplateListAsync();
			return result;
		}*/

		public async Task<HttpResponseMessage> GetTemp()
		{

			var result = await GetTemplateListAsync();
			if (result != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, result);
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
			}
		}

		public async Task<JObject> GetTemplateListAsync()
		{
			string uri = "https://api.safetyculture.io/";

            //var tokenJson = new AuthController().Get();

			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(uri);
			client.DefaultRequestHeaders.Add("User-Agent", "UrmIAuditorClient/1.0");
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer 49b6d1283cb4741460956c091e04368b72e2f6932d6834ec2c2e8f6f9e6215e4");

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "templates/search");
			

			HttpResponseMessage response = await client.SendAsync(request);

			string jsonResponse = null;
			JObject json = null;

			if (response.IsSuccessStatusCode)
			{

				jsonResponse = await response.Content.ReadAsStringAsync();
				json = JObject.Parse(jsonResponse);

			}


			return json;
		}

		
	}
}
