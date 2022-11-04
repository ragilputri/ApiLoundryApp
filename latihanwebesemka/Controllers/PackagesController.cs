using latihanwebesemka.Data;
using latihanwebesemka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace latihanwebesemka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly EsemkaLaundryContext _context;

        public PackagesController(EsemkaLaundryContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Package>>> GetPackages(int page, int? total, int? minPrice, int? maxPrice)
        {
            var packages = await _context.Packages.ToListAsync();
            var result = new List<Package>();
            for (int i = 0; i < packages.Count; i++)
            {
                var serviceDetail = await _context.Services.FindAsync(packages[i].ServiceId);
                string caregoryService;
                string unitService;
                if (serviceDetail.Category == 0)
                {
                    caregoryService = "Kiloan";
                }
                else if (serviceDetail.Category == 1)
                {
                    caregoryService = "Satuan";
                }
                else if (serviceDetail.Category == 2)
                {
                    caregoryService = "PerlengkapanBayi";
                }
                else if (serviceDetail.Category == 3)
                {
                    caregoryService = "Helm";
                }
                else if (serviceDetail.Category == 4)
                {
                    caregoryService = "Sepatu";
                }
                else
                {
                    return NotFound("not found value from category");
                }
                if (serviceDetail.Unit == 0)
                {
                    unitService = "KG";
                }
                else if (serviceDetail.Unit == 1)
                {
                    unitService = "Piece";
                }
                else
                {
                    return NotFound("not found value from unit");
                }
                var serviceNew = new ServiceUpload
                {
                    Id = serviceDetail.Id,
                    Category = caregoryService,
                    Unit = unitService,
                    Name = serviceDetail.Name,
                    Price = serviceDetail.Price,
                    EstimationDuration = serviceDetail.EstimationDuration
                };
                packages[i].Service = serviceNew;
                result.Add(packages[i]);
            }

            return result;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetPackage(Guid id)
        {
            var package = await _context.Packages.FindAsync(id);
            var service = await _context.Services.FindAsync(package.ServiceId);
            string caregoryService;
            string unitService;
            if (package == null)
            {
                return NotFound();
            }

            if (service.Category == 0)
            {
                caregoryService = "Kiloan";
            }
            else if (service.Category == 1)
            {
                caregoryService = "Satuan";
            }
            else if (service.Category == 2)
            {
                caregoryService = "PerlengkapanBayi";
            }
            else if (service.Category == 3)
            {
                caregoryService = "Helm";
            }
            else if (service.Category == 4)
            {
                caregoryService = "Sepatu";
            }
            else
            {
                return NotFound("not found value from category");
            }
            if (service.Unit == 0)
            {
                unitService = "KG";
            }
            else if (service.Unit == 1)
            {
                unitService = "Piece";
            }
            else
            {
                return NotFound("not found value from unit");
            }

            var responseService = new ServiceUpload
            {
                Id = service.Id,
                Category = caregoryService,
                Unit = unitService,
                Name = service.Name,
                Price = service.Price,
                EstimationDuration = service.EstimationDuration
            };
            package.Service = responseService;

            return package;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackage(Guid id, PackageUpload packageUpdated)
        {
            if (id != packageUpdated.Id)
            {
                return BadRequest();
            }
            _context.Entry(packageUpdated).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Package>> PostPackage(PackageUpload packageUpload)
        {

            var newPackage = new Package
            {
                ServiceId = packageUpload.ServiceId,
                Total = packageUpload.Total,
                Price = packageUpload.Price,
            };
            _context.Packages.Add(newPackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPackage", new { id = newPackage.Id }, newPackage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PackageExists(Guid id)
        {
            return _context.Packages.Any(e => e.Id == id);
        }

    }

}
