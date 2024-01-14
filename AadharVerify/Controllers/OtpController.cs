using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AadharVerify.Models;
using AadharVerify.Services;

namespace AadharVerify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly UserDataDbContext _context;
        private readonly OtpService _otpService;

        public OtpController(UserDataDbContext context, OtpService otpService)
        {
            _context = context;
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
        }

        // GET: api/Otp
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phone>>> GetPhonenumber()
        {
            if (_context.Phonenumber == null)
            {
                return NotFound();
            }
            return await _context.Phonenumber.ToListAsync();
        }

        // GET: api/Otp/5
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

        [HttpPost]
        [Route("api/otp/generate")]
        public IActionResult GenerateOtp([FromBody] Phone phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                // Call a service or logic to generate OTP
                string generatedOtp = _otpService.GenerateOtp(phone.PhoneNumber);

                // Save the generated OTP in the Phone model
                phone.OTP = generatedOtp;

                // Save the Phone model to a database
                _otpService.SaveOtpRecord(phone);

                return Ok(new { Message = "OTP generated successfully", Otp = generatedOtp });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/otp/verify")]
        public IActionResult VerifyOtp([FromBody] OtpVerificationRequest otpVerificationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool isOtpValid = _otpService.VerifyOtp(otpVerificationRequest.PhoneNumber, otpVerificationRequest.Otp);

                if (isOtpValid)
                {
                    // OTP is valid, you can perform further actions here
                    return Ok(new { Message = "OTP verified successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Invalid OTP" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
    }
}
