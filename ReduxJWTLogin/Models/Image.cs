using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReduxJWTLogin.Models
{
	public class Image
	{
		public string Id { get; set; }
		
		public IFormFile File { get; set; }
		
	}
}

