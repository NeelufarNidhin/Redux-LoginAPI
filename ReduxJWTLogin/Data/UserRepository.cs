using System;
using ReduxJWTLogin.Models;

namespace ReduxJWTLogin.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ApplicationUser Create(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetUsers()
        {
            var users = _db.Users.ToList();
            return users.ToList();
        }

        public ApplicationUser GetUserById(string id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
          
                return user;
            
           
        }

        public List<ApplicationUser>GetSearchQuery(string searchString)
        {
            var users = _db.ApplicationUsers.Where(x => x.Email.Contains(searchString)
            || x.FirstName.Contains(searchString) || x.LastName.Contains(searchString)).ToList();
               
            return users.ToList();
        }



        public ApplicationUser GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser Update(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);

            return user;
        }
    }
}

