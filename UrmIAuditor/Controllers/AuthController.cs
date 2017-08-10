using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json.Linq;


namespace UrmIAuditor.Controllers
{
    public class AuthController : ApiController
    {
		/*public async Task<JObject> Get()
		{
			string username = "iaudit@urmgroup.com.au";
			string password = "urm123";
			string grantType = "password";
			var result = await GetTokenAsync(username,password,grantType);
			return result;
		}*/

		public async Task<HttpResponseMessage> Get()
		{
			string username = "iaudit@urmgroup.com.au";
			string password = "urm123";
			string grantType = "password";
			var result = await GetTokenAsync(username, password, grantType);
			if (result != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, result);
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
			}
		}

		public async Task<JObject> GetTokenAsync(string username, string password, string grantType)
		{
			string uri = "https://api.safetyculture.io/";
			string parameter = "username=" + username + "&password=" + password + "&grant_type=" + grantType;


			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(uri);
			client.DefaultRequestHeaders.Add("User-Agent", "UrmIAuditorClient/1.0");
			client.DefaultRequestHeaders.Add("Accept", "application/json");

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "auth");
			request.Content = new StringContent(parameter,Encoding.UTF8 , "application/x-www-form-urlencoded");

			HttpResponseMessage response = await client.SendAsync(request);

			string jsonResponse= null;

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
