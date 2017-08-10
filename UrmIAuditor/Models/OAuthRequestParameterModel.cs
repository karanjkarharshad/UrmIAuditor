using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UrmIAuditor.Models
{
	public class OAuthRequestParameterModel
	{
		public string username { get; set; }
		[Required]
		public string password { get; set; }
		[Required]
		public string grant_type { get; set; }
		
	}

}