using AadharVerify.Models;
using AadharVerify.Models.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace AadharVerify.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class UserListController : ControllerBase
        {
            private readonly IConfiguration _configuration;
            private readonly UserDataDbContext _dbContext;

            public UserListController(IConfiguration configuration, UserDataDbContext dbContext)
            {
                _configuration = configuration;
                _dbContext = dbContext;
            }




        [HttpGet("Login")]
            public IActionResult Login(string email, string password)
            {
                var user = _dbContext.UsersList.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    var jwt = new JWT(_configuration["Jwt:Key"], _configuration["Jwt:Duration"]);
                    var token = jwt.GenerateToken(user);

                var response = new ResponseDTO
                {
                    token = token,
                    role = user.UserType
                };
                    return Ok(response);
                }

                return Unauthorized("Invalid credentials");
            }

        }
    }



