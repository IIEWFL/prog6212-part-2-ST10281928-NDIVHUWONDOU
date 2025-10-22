
using Microsoft.AspNetCore.Mvc;
using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ContractMonthlyClaimSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LecturerController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Lecturer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim, IFormFile file)
        {
          
            // Upload file first
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                claim.DocumentPath = "/uploads/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // At this point ModelState is invalid. File is already saved, so optionally display it in the view
            return View(claim);
        }


        // GET: Lecturer/Index
        public IActionResult Index()
        {
            var claims = _context.Claims.ToList();
            return View(claims);
        }
        // GET: Lecturer/Details/5
        public IActionResult Details(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }
        // GET: Lecturer/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Lecturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Claim updatedClaim, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Content("ModelState errors: " + string.Join(", ", errors));
            }

            var claim = await _context.Claims.FindAsync(updatedClaim.Id);
            if (claim == null)
            {
                return NotFound();
            }

            // ✅ Update fields
            claim.LecturerName = updatedClaim.LecturerName;
            claim.Module = updatedClaim.Module;
            claim.HoursWorked = updatedClaim.HoursWorked;
            claim.HourlyRate = updatedClaim.HourlyRate;
            claim.Status = updatedClaim.Status;
            claim.statusCoord = updatedClaim.statusCoord;
            claim.statusManager = updatedClaim.statusManager;

            // ✅ Handle file upload (if new one is uploaded)
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                claim.DocumentPath = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Lecturer/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Lecturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public IActionResult CheckStatus(int id, string status)
        //{
        //    var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
        //    if (claim == null) return NotFound();

        //    if(claim.statusCoord == "Approved" && claim.statusManager == "Approved")
        //    {
        //        claim.Status = "Approved";
        //    }
        //    else if(claim.statusCoord.IsNullOrEmpty() || claim.statusManager.IsNullOrEmpty() || claim.statusManager == "Pending")
        //    {
        //        claim.Status = "Pending";
        //    }
        //    else
        //    {
        //        claim.Status = "Rejected";
        //    }

        //        _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}
    }
}

