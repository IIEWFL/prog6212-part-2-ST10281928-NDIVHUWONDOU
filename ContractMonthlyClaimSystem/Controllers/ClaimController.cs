using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContractMonthlyClaimSystem.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClaimController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claims = _context.Claims.ToList();
            return View(claims);
        }

        public IActionResult Details(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();
            return View(claim);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Claim claim)
        {
            _context.Claims.Add(claim);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            claim.statusCoord = status;
            if (claim.statusCoord == "Approved" && claim.statusManager == "Approved")
            {
                claim.Status = "Approved";
            }
            else if (claim.statusCoord.IsNullOrEmpty() || claim.statusManager.IsNullOrEmpty() || claim.statusManager == "Pending")
            {
                claim.Status = "Pending";
            }
            else
            {
                claim.Status = "Rejected";
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

