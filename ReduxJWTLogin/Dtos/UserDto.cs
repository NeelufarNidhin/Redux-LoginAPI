using System;
namespace ReduxJWTLogin.Dtos
{
	public class UserDto
	{
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
        public string Email { get; set; }
		public string  token { get; set; }
	}
}

