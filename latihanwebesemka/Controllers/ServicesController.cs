using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using latihanwebesemka.Data;
using latihanwebesemka.Models;

namespace latihanwebesemka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly EsemkaLaundryContext _context;

        public ServicesController(EsemkaLaundryContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceUpload>>> GetServices(int page, string? name, [FromQuery] ServiceQueryModel serviceQueryModel, int? minPrice, int? maxPrice)
        { 
            var services = await _context.Services.ToListAsync();
            List<ServiceUpload> results = new List<ServiceUpload>();
            for(int i = 0; i < services.Count; i++)
            {
                var service = await _context.Services.FindAsync(services[i].Id);
                string caregoryService;
                string unitService;
                if (service == null)
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

                results.Add(responseService);

            }

            return results.ToArray();
            
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceUpload>> GetService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            string caregoryService;
            string unitService;
            if (service == null)
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

            return responseService;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(Guid id, ServiceUpload serviceUpdated)
        {
            int categoryId;
            int unitId;
            if (id != serviceUpdated.Id)
            {
                return BadRequest();
            }
            if (serviceUpdated.Category == "Kiloan")
            {
                categoryId = 0;
            }
            else if (serviceUpdated.Category == "Satuan")
            {
                categoryId = 1;
            }
            else if (serviceUpdated.Category == "PerlengkapanBayi")
            {
                categoryId = 2;
            }
            else if (serviceUpdated.Category == "Helm")
            {
                categoryId = 3;
            }
            else if (serviceUpdated.Category == "Sepatu")
            {
                categoryId = 4;
            }
            else
            {
                return NotFound("value of category is (Kiloan, Satuan, PerlengkapanBayi, Helm, Sepatu)");
            }
            if (serviceUpdated.Unit == "KG")
            {
                unitId = 0;
            }
            else if (serviceUpdated.Unit == "Piece")
            {
                unitId = 1;
            }
            else
            {
                return NotFound("value of unit is (KG, Piece))");
            }

            var service = await _context.Services.FindAsync(id);
            service.Name = serviceUpdated.Name;
            service.Category = categoryId;
            service.Unit = unitId;
            service.Price = serviceUpdated.Price;
            service.EstimationDuration = serviceUpdated.EstimationDuration;
            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(ServiceUpload service)
        {
            int categoryId;
            int unitId;
            if(service.Category == "Kiloan")
            {
                categoryId = 0;
            }else if (service.Category == "Satuan")
            {
                categoryId = 1;
            }
            else if (service.Category == "PerlengkapanBayi")
            {
                categoryId = 2;
            }
            else if (service.Category == "Helm")
            {
                categoryId = 3;
            }
            else if (service.Category == "Sepatu")
            {
                categoryId = 4;
            }
            else
            {
                return NotFound("value of category is (Kiloan, Satuan, PerlengkapanBayi, Helm, Sepatu)");
            }

            if (service.Unit == "KG")
            {
                unitId = 0;
            }else if(service.Unit == "Piece")
            {
                unitId = 1;
            }
            else
            {
                return NotFound("value of unit is (KG, Piece))");
            }

            var newService = new Service
            {
                Name = service.Name,
                Category = categoryId,
                Unit = unitId,
                EstimationDuration = service.EstimationDuration,
            };
            _context.Services.Add(newService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = newService.Id }, newService);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(Guid id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
