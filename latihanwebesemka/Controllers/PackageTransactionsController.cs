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
    public class PackageTransactionsController : ControllerBase
    {
        private readonly EsemkaLaundryContext _context;

        public PackageTransactionsController(EsemkaLaundryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageTransaction>>> GetPackageTransactions(int page, string? keyword)
        {
            return await _context.PackageTransactions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageTransactionsShow>> GetPackageTransaction(Guid id)
        {
            var packageTransaction = await _context.PackageTransactions.FindAsync(id);
            var package = await _context.Packages.FindAsync(packageTransaction.PackageId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == packageTransaction.UserEmail);
            if (packageTransaction == null)
            {
                return NotFound();
            };
            var result = new PackageTransactionsShow
            {
                Id = packageTransaction.Id,
                PackageId = packageTransaction.PackageId,
                UserEmail = packageTransaction.UserEmail,
                User = user,
                Package = package,
                Price = packageTransaction.Price,
                AvaibleUnit = packageTransaction.AvaibleUnit,

            };

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<PackageTransactionsShow>> PostPackageTransaction(PackageTransactionCreate packageTransactionCreate)
        {
            if (packageTransactionCreate.UserEmail == null) 
            { 
                return BadRequest(); 
            }else if (packageTransactionCreate.PackageId == null) 
            { 
                return BadRequest(); 
            }
            var package = await _context.Packages.FindAsync(packageTransactionCreate.PackageId);
            if (package == null) return NotFound();

            var newPackageTransaction = new PackageTransaction
            {
                PackageId = packageTransactionCreate.PackageId,
                UserEmail = packageTransactionCreate.UserEmail,
                Price = package.Price,
                AvaibleUnit = 0,
            };

            _context.PackageTransactions.Add(newPackageTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPackageTransaction", new { id = newPackageTransaction.Id }, newPackageTransaction);
        }

        private bool PackageTransactionExists(Guid id)
        {
            return _context.PackageTransactions.Any(e => e.Id == id);
        }
    }
}
