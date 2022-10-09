using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreProject.Models;
using PreProject.Models.DTOs;
using PreProject.Repository.IRepository;

namespace PreProject.Controllers
{
    [Authorize] //To be applicable to all the methods inside this particular controller
    [Route("api/v{version:apiVersion}/Users")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Dependency Injection to access User Repository
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        //Authenticat action, if any user calls the authenticate action
        [AllowAnonymous] //This will override the existing 'authorize' above on the controller
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }    
            return Ok(user); //returning a user object if the above is not true
        }

        [AllowAnonymous]
        [HttpPost("register")]

        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = _userRepo.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }
    }
}
