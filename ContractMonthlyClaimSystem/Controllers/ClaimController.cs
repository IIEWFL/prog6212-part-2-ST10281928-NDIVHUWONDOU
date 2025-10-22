
using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ContractMonthlyClaimSystem.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: List all claims
        public IActionResult Index()
        {
            var claims = _context.Claims.ToList();
            return View(claims);
        }

        // GET: Claim details
        public IActionResult Details(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();
            return View(claim);
        }

        // GET: Create form
        [HttpGet]
        public IActionResult Create() => View();

        // POST: Create claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Claim claim)
        {
            if (!ModelState.IsValid)
                return View(claim);

            _context.Claims.Add(claim);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Update status and reason (Coordinator)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status, string? reason)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            claim.statusCoord = status;

            // Save rejection reason (only if rejected)
            claim.reasonCoord = status == "Rejected" ? reason : null;

            // Determine overall claim status
            if (claim.statusCoord == "Approved" && claim.statusManager == "Approved")
                claim.Status = "Approved";
            else if (claim.statusCoord == "Rejected" || claim.statusManager == "Rejected")
                claim.Status = "Rejected";
            else
                claim.Status = "Pending";

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

