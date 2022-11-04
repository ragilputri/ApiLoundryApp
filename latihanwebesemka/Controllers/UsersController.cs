using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using latihanwebesemka.Data;
using latihanwebesemka.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace latihanwebesemka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EsemkaLaundryContext _context;

        public UsersController(EsemkaLaundryContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize(Roles = "0")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(int page, string? keyword, [FromQuery] UserQueryModel queryModel)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.ToListAsync();

            if (page == 0 && keyword == null)
            {
                users = await _context.Users.ToListAsync();
            }
            else if (page == 0 && keyword == null && queryModel.OptionGender == OptionGenderEnum.Male)
            {
                users = await _context.Users.Where(u => u.Gender == 1).ToListAsync();
            }
            else if (page == 0 && keyword == null && queryModel.OptionGender == OptionGenderEnum.Female)
            {
                users = await _context.Users.Where(u => u.Gender == 2).ToListAsync();
            }
            //
            else if (page == 0 && keyword != null)
            {
                users = await _context.Users.Where(u => u.Name.Contains(keyword)).ToListAsync();
            }
            else if (page == 0 && keyword != null && queryModel.OptionGender == OptionGenderEnum.Male)
            {
                users = await _context.Users.Where(u => u.Name.Contains(keyword) && u.Gender == 1).ToListAsync();
            }
            else if (page == 0 && keyword != null && queryModel.OptionGender == OptionGenderEnum.Female)
            {
                users = await _context.Users.Where(u => u.Name.Contains(keyword) && u.Gender == 2).ToListAsync();
            }
            //
            else if (page != 0 && keyword == null)
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_context.Users.Count() / pageResults);
                users = await _context.Users
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else if (page != 0 && keyword == null && queryModel.OptionGender == OptionGenderEnum.Male)
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_context.Users.Count() / pageResults);
                users = await _context.Users
                .Where(u => u.Gender == 1)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else if (page != 0 && keyword == null && queryModel.OptionGender == OptionGenderEnum.Female)
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_context.Users.Count() / pageResults);
                users = await _context.Users
                .Where(u => u.Gender == 2)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            //

            else if (page != 0 && keyword != null)
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_context.Users.Count() / pageResults);
                users = await _context.Users
                .Where(u => u.Name.Contains(keyword))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else if (page != 0 && keyword != null && queryModel.OptionGender == OptionGenderEnum.Male)
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_context.Users.Count() / pageResults);
                users = await _context.Users
                .Where(u => u.Name.Contains(keyword) && u.Gender == 1)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else if (page != 0 && keyword != null && queryModel.OptionGender == OptionGenderEnum.Female)
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_context.Users.Count() / pageResults);
                users = await _context.Users
                .Where(u => u.Name.Contains(keyword) && u.Gender == 2)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }

            return Ok(users);
        }

        [HttpGet("csv")]
        public async Task<FileResult> GetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("email,name");
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                builder.AppendLine($"{user.Email},{user.Name}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
        }

        [HttpGet("excel")]
        public async Task<FileResult> GetExcel()
        {
            var builder = new StringBuilder();
            builder.Append("<table border = `"+"1px"+"`b>");
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                builder.Append($"<tr><td>{user.Email}</td><td>{user.Name}</td></tr>");
            }
            builder.Append("</table>");
            byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(new MemoryStream(bytes, 0, bytes.Length), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
        }

        [HttpPost, Authorize(Roles = "0")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'LibraryContext.Users'  is null.");
            }
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { email = user.Email }, user);
        }

        [HttpGet("{email}"), Authorize(Roles = "0")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{email}"), Authorize(Roles = "0")]
        public async Task<IActionResult> PutUser(string email, User user)
        {
            if (email != user.Email)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { email = user.Email }, user);
        }

        private bool UserExists(string email)
        {
            return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
        }
    }
}
