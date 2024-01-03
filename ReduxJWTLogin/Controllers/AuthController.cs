using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ReduxJWTLogin.Data;
using ReduxJWTLogin.Dtos;
using ReduxJWTLogin.Models;
using ReduxJWTLogin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReduxJWTLogin.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public readonly IUserRepository _userRepository;


        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
             TokenService tokenService, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _userRepository = userRepository;


        }

        [AllowAnonymous]
        // POST api/values
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email!);

            if (user != null) {
                var chkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password!);

                if (chkPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //obtaining the token created in toknservice using generate token
                        var JWTtoken = await _tokenService.GenerateToken(user);

                      

                        var response = new UserDto
                        {
                            UserName = user.UserName,
                            Roles = roles,

                            Email = user.Email,
                            token = JWTtoken

                        };
                        return Ok(response);

                    }

                }
            }


            return BadRequest("Username or Password Invalid");




        }

        [HttpPost("register")]
        [AllowAnonymous]
        //[Authorize(Roles ="User , Admin")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new ApplicationUser { FirstName = registerDto.FirstName, LastName = registerDto.LastName, UserName = registerDto.FirstName + registerDto.LastName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(user, "User");

            return StatusCode(201);
        }


        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);



            return new UserDto
            {
                Email = user.Email,
                token = await _tokenService.GenerateToken(user),

            };
        }


      //  [Authorize(Roles = " Admin")]
        [AllowAnonymous]
        [HttpGet("getall")]
        public Task<IActionResult> GetAll()
        {
            var users = _userRepository.GetUsers();

            return Task.FromResult<IActionResult>(Ok(users));
        }

        //[Authorize(Roles = "User")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public Task<IActionResult> GetUser(string id)

        {
            var user =   _userRepository.GetUserById(id);
            if (user is null)
                return Task.FromResult<IActionResult>(NotFound());

            return Task.FromResult<IActionResult>(Ok(user));
        }

       [AllowAnonymous]
        [HttpPost("img")]
       // [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UploadImage([FromBody] ImageDto imageDto)
        {
            var user =  _userRepository.GetUserById(imageDto.Id);

            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageDto.Img.FileName);

                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    imageDto.Img.CopyTo(stream);
                }

                return Ok(path);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

       // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateDto updatedUser)
        {
            var user =  _userRepository.GetUserById(id);

            if (user is null)
                return NotFound();


            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;



            var result = await _userManager.UpdateAsync(user);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin")]
         [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
           // var logins = user.Logins;
           // var rolesForUser = await _userManager.GetRolesAsync(id);

            if (user is null)
                return NotFound();

           



            var result = await _userManager.DeleteAsync(user);

            return Ok(result);
        }



        [AllowAnonymous]
        [HttpGet("search")]
        public  ActionResult Search([FromQuery] string query)
        {
            var userList =  _userRepository.GetSearchQuery(query);

            return Ok(userList);
        }


        [AllowAnonymous]
        [HttpGet("userId{email}")]
        public async Task<IActionResult> UserId( string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return BadRequest("Please check username or password");
            }

            return Ok(user.Id);
        }


    }
}

