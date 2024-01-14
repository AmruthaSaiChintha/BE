using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AadharVerify.Models;
using AadharVerify.Dto;



namespace AadharVerify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserDataDbContext _context;
       


        public UserDataController(UserDataDbContext context)
        {
            _context = context;
           

        }

        // GET: api/UserData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Data>>> GetDatas()
        {
            if (_context.Datas == null)
            {
                return NotFound();
            }
            return await _context.Datas.ToListAsync();
        }

        // GET: api/UserData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Data>> GetData(int id)
        {
            if (_context.Datas == null)
            {
                return NotFound();
            }
            var data = await _context.Datas.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        // PUT: api/UserData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutData(int id, Data data)
        {
            if (id != data.Id)
            {
                return BadRequest();
            }

            _context.Entry(data).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataExists(id))
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

        // POST: api/UserData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Data>> PostData(Data data)
        {
            if (_context.Datas == null)
            {
                return Problem("Entity set 'UserDataDbContext.Datas'  is null.");
            }
            _context.Datas.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetData", new { id = data.Id }, data);
        }

        // DELETE: api/UserData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            if (_context.Datas == null)
            {
                return NotFound();
            }
            var data = await _context.Datas.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            _context.Datas.Remove(data);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DataExists(int id)
        {
            return (_context.Datas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] EmailVerifyDto userLogin)
        {
            var user = _context.Datas.FirstOrDefault(u => u.Email == userLogin.email);
            if (user == null || user.Email != userLogin.email)
            {
                return BadRequest(new { success = false, message = "invalid email" });
            }
            return Ok(new { success = true, message = "Email already registered" });

        }

        

        // GET: api/Users/5
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<Data>> GetVerifiedUserByEmail([FromRoute] string email)
        {
            var verifiedUser = await _context.Datas
                .FirstOrDefaultAsync(u => u.Email == email);

            if (verifiedUser == null)
            {
                return NotFound();
            }

            return verifiedUser;
        }

        [HttpGet("check-aadhar/{aadharNumber}")]
        public IActionResult CheckPassportNumberExists([FromRoute] string aadharNumber)
        {
            try
            {
                var exists = _context.Datas.Any(u => u.AadharNumber == aadharNumber);
                if(exists)
                return Ok(exists);
                else return BadRequest(new { success = false });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error checking passport number: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("by-aadhar/{aadharNumber}")]
        public async Task<ActionResult<Data>> GetVerifiedUserByAadhar([FromRoute] string aadharNumber)
        {
            var verifiedUser = await _context.Datas
                .FirstOrDefaultAsync(u => u.AadharNumber == aadharNumber);

            if (verifiedUser == null)
            {
                return NotFound();
            }

            return verifiedUser;
        }
        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<Data>> Getid([FromRoute]int id)
        {
            var verifiedUser = await _context.Datas
                .FirstOrDefaultAsync(u => u.Id == id);

            if (verifiedUser == null)
            {
                return NotFound();
            }

            return verifiedUser;
        }

    }
}