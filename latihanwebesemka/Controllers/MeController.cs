using latihanwebesemka.Data;
using latihanwebesemka.Models;
using latihanwebesemka.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace latihanwebesemka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly EsemkaLaundryContext _context;
        private readonly IUserService _userService;
        public static IWebHostEnvironment _webHostEnvironment;

        public MeController(EsemkaLaundryContext context, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Me
        [HttpGet, Authorize]
        public async Task<ActionResult<User>> GetUser()
        {
            //var name = User?.Identity?.Name;
            var userEmail = _userService.GetByemail();

            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Me/5
        [HttpGet("Photo"), Authorize]
        public async Task<ActionResult> GetUserPhoto()
        {
            var userEmail = _userService.GetByemail();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            var filePath = user.PhotoPath;
            if(filePath == null)
            {
                return NotFound("Tidak ada photo");
            }
            if (System.IO.File.Exists(filePath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
                return File(b, "image/png");
            }
            return null;
        }

        // PUT: api/Me/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut, Authorize]
        public async Task<IActionResult> PutUser(User user)
        {
            //var name = User?.Identity?.Name;
            //var email = ClaimTypes.Email;
            var userEmail = _userService.GetByemail();

            if (userEmail == null)
            {
                return NotFound();
            }

            if (user.Email != userEmail)
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
                if (!UserExists(user.Email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        // POST: api/Me
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Photo"), Authorize]
        public async Task<ActionResult<User>> PostUser([FromForm] FileUpload fileUpload)
        {

            try
            {
                if (fileUpload.file.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.file.FileName))
                    {
                        fileUpload.file.CopyTo(fileStream);
                        fileStream.Flush();

                        var userEmail = _userService.GetByemail();
                        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                        user.PhotoPath = path + fileUpload.file.FileName;
                        _context.Entry(user).State = EntityState.Modified;

                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!UserExists(userEmail))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }

                        return Ok(user);
                    }
                }
                else
                {
                    return BadRequest("Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool UserExists(string email)
        {
            return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
        }
    }
}
