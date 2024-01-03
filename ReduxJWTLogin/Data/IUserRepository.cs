using System;
using ReduxJWTLogin.Models;

namespace ReduxJWTLogin.Data
{
	public interface IUserRepository
	{
        ApplicationUser Create(ApplicationUser user);
        ApplicationUser GetByUserName(string userName);
        ApplicationUser GetUserById(string id);
        ApplicationUser Update(string id);
        List<ApplicationUser> GetUsers();
        List<ApplicationUser> GetSearchQuery(string searchString);
    }
}

