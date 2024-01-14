using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AadharVerify.Models;
using AadharVerify.Dto;
using AadharVerify.Helper;
using AadharVerify.Services;

namespace AadharVerify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalUsersController : ControllerBase
    {
        private readonly UserDataDbContext _context;
        private readonly IEmailService emailService;


        public FinalUsersController(UserDataDbContext context, IEmailService emailService)
        {
            _context = context;
            this.emailService = emailService;
        }

        // GET: api/FinalUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsersList()
        {
     
          if (_context.UsersList == null)
          {
              return NotFound("Users are Not Found");
          }
            return await _context.UsersList.ToListAsync();
        }

        // GET: api/FinalUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            try
            {
                if (_context.UsersList == null)
                {
                    return NotFound("User list not found.");
                }

                var users = await _context.UsersList.FindAsync(id);

                if (users == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                return users;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // PUT: api/FinalUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, [FromBody] Users users)
        {
            try
            {
                if (id != users.Id)
                {
                    return BadRequest("Invalid ID in the request.");
                }

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return BadRequest(errors);
                }

                _context.Entry(users).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound($"User with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        // POST: api/FinalUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            try
            {
                if (_context.UsersList == null)
                {
                    return Problem("Entity set 'UserDataDbContext.UsersList' is null.");
                }

                users.UserType = "user";

                _context.UsersList.Add(users);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUsers", new { id = users.Id }, users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("email-exists/{email}")]
        public IActionResult CheckEmailExists(string email)
        {
            try
            {
                if (_context.UsersList == null)
                {
                    return Problem("Entity set 'UserDataDbContext.UsersList' is null.");
                }

                var exists = _context.UsersList.Any(u => u.Email == email);

                if (exists)
                {
                    return Ok(new { exists = true, message = "Email already registered." });
                }
                else
                {
                    return Ok(new { exists = false, message = "Email is unique." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        // DELETE: api/FinalUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            try
            {
                if (_context.UsersList == null)
                {
                    return Problem("Entity set 'UserDataDbContext.UsersList' is null.");
                }

                var users = await _context.UsersList.FindAsync(id);

                if (users == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                _context.UsersList.Remove(users);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] EmailVerifyDto emailVerifyDto)
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = "amruthasai567@gmail.com";
                mailrequest.Subject = "Welcome to AadharVerify";
                mailrequest.Body = GetHtmlcontent(emailVerifyDto);
                await emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while sending the email. Error Details: {ex.Message}");
            }
        }


        private string GetHtmlcontent(EmailVerifyDto emailVerifyDto)
        {
            string response = "<div style=\"width:100%;background-color:lightgreen;text-align:center;margin:10px\">";
            response += "<h1 style=\"color:navy;\">Welcome to Aadhar Verify</h1>";

            response += "<img src=\"https://example.com/your-custom-image.jpg\" alt=\"Custom Image\" />";
            response += "<h2 style=\"color:darkgreen;\">Thanks for subscribing!</h2>";
            response += "<a href=\"https://www.youtube.com/channel/UCsbmVmB_or8sVLLEq4XhE_A/join\" style=\"color:blue;\">Please join membership by clicking the link</a>";
            response += "<div><h1 style=\"color:maroon;\">Contact us: nihiratechiees@gmail.com</h1></div>";
            response += $"<div><h1> 6 digit Otp  : {emailVerifyDto.Otp}</h1></div>"; // Use $ for string interpolation
            response += "</div>";
            return response;
        }

        private bool UsersExists(int id)
        {
            return (_context.UsersList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
