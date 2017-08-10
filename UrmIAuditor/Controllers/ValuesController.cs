using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UrmIAuditor.Controllers
{
	public class ValuesController : ApiController
	{
		static List<string> parameters = new List<string>();

		// GET api/values
		public IEnumerable<string> Get()
		{
			//return new string[] { "value1", "value2" };
			return parameters;
		}

		// GET api/values/5
		public string Get(int id)
		{
			//return "value";
			return parameters[id];
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
			parameters.Add(value);
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}
