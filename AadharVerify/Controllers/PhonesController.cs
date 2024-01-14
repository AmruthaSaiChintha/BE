using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AadharVerify.Models;
using static System.Net.WebRequestMethods;

namespace AadharVerify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly UserDataDbContext _context;

        public PhonesController(UserDataDbContext context)
        {
            _context = context;
        }

        // GET: api/Phones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phone>>> GetPhonenumber()
        {
          if (_context.Phonenumber == null)
          {
              return NotFound();
          }
            return await _context.Phonenumber.ToListAsync();
        }

        // GET: api/Phones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Phone>> GetPhone(int id)
        {
          if (_context.Phonenumber == null)
          {
              return NotFound();
          }
            var phone = await _context.Phonenumber.FindAsync(id);

            if (phone == null)
            {
                return NotFound();
            }

            return phone;
        }

        // PUT: api/Phones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhone(int id, Phone phone)
        {
            if (id != phone.Id)
            {
                return BadRequest();
            }

            _context.Entry(phone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Phones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Phone>> PostPhone(Phone phone)
        {
          if (_context.Phonenumber == null)
          {
              return Problem("Entity set 'UserDataDbContext.Phonenumber'  is null.");
          }
            _context.Phonenumber.Add(phone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhone", new { id = phone.Id }, phone);
        }

        // DELETE: api/Phones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            if (_context.Phonenumber == null)
            {
                return NotFound();
            }
            var phone = await _context.Phonenumber.FindAsync(id);
            if (phone == null)
            {
                return NotFound();
            }

            _context.Phonenumber.Remove(phone);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhoneExists(int id)
        {
            return (_context.Phonenumber?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost("verifyotp")]
        public async Task<IActionResult> VerifyOTP([FromBody] Phone request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.OTP))
            {
                return BadRequest("Phone number and OTP are required.");
            }

            // Retrieve the Phone record from the database
            var phone = await _context.Phonenumber.FirstOrDefaultAsync(p => p.PhoneNumber == request.PhoneNumber);

            if (phone == null || phone.OTP != request.OTP)
            {
                return BadRequest("Invalid OTP.");

            }
            // Example logging
           


            // Clear the OTP after successful verification
            phone.OTP = null;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "OTP verified successfully." });
        }

    [HttpPost("sendotp")]
        public async Task<IActionResult> SendOTP([FromBody] Phone request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            // Generate and send OTP (you need to implement your OTP generation and sending logic here)
            // For now, let's assume OTP is 123456
            string otp = "123456";

            // Save the OTP in the Phone model
            var phone = await _context.Phonenumber.FirstOrDefaultAsync(p => p.PhoneNumber == request.PhoneNumber);
            if (phone != null)
            {
                phone.OTP = otp;
                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = "OTP sent successfully." });
        }

    }


}
