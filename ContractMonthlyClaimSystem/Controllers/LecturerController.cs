////using ContractMonthlyClaimSystem.Models;
////using Microsoft.AspNetCore.Mvc;

////namespace ContractMonthlyClaimSystem.Controllers
////{
////    public class LecturerController : Controller
////    {
////        private readonly ApplicationDbContext _context;
////        public LecturerController(ApplicationDbContext context)
////        {
////            _context = context;
////        }
////        private static List<Claim> claims = new()
////        {
////            new Claim{Id=1, LecturerName="Dr. Smith", Module="Prog6212", HourlyRate=300, HoursWorked=10, Status="Pending", statusCoord="Approved", statusManager="Approved"},
////            new Claim{Id=2,  LecturerName="Prof. Johnson", Module="Database", HourlyRate=150, HoursWorked=8, Status="Accessed", statusCoord="Approved", statusManager="Rejected" }
////        };
////        public IActionResult Index()
////        {
////            var claims = _context.Claims.ToList();
////            return View(claims);
////        }

////        public IActionResult Details(int id)
////        {
////            var claim = claims.FirstOrDefault(c => c.Id == id);
////            if (claim == null) return NotFound();
////            return View(claim);
////        }

////        [HttpGet]
////        public IActionResult Create() => View();
////        [HttpPost]
////        public IActionResult Create(Claim claim, IFormFile? document)
////        {
////            if (ModelState.IsValid)
////            {
////                if (document != null && document.Length > 0)
////                {
////                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
////                    if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

////                    var filepath = Path.Combine(uploadFolder, document.FileName);
////                    using (var stream = new FileStream(filepath, FileMode.Create))
////                    {
////                        document.CopyTo(stream);
////                    }

////                    claim.DocumentPath = "/uploads/" + document.FileName;

////                }

////                claim.Id = claims.Count + 1;
////                claims.Add(claim);
////                return RedirectToAction("Index");
////            }
////            return View(claim);
////        }

////        //Edit
////        [HttpGet]
////        public IActionResult Edit(int id)
////        {
////            var claim = claims.FirstOrDefault(c => c.Id == id);
////            if (claim == null) return NotFound();
////            return View(claim);
////        }
////        [HttpPost]
////        public IActionResult Edit(Claim updatedclaim)
////        {
////            var claim = claims.FirstOrDefault(c => c.Id == updatedclaim.Id);
////            if (claim == null) return NotFound();

////            claim.LecturerName = updatedclaim.LecturerName;
////            claim.Module = updatedclaim.Module;
////            claim.HoursWorked = updatedclaim.HoursWorked;
////            claim.HourlyRate = updatedclaim.HourlyRate;
////            claim.DocumentPath = updatedclaim.DocumentPath;
////            claim.Status = updatedclaim.Status;

////            return RedirectToAction("Index");
////        }

////        //Delete
////        [HttpGet]
////        public IActionResult Delete(int id)
////        {
////            var claim = claims.FirstOrDefault(c => c.Id == id);
////            if (claim == null) return NotFound();
////            return View(claim);
////        }

////        [HttpPost, ActionName("Delete")]
////        public IActionResult DeleteConfirmed(int id)
////        {
////            var claim = claims.FirstOrDefault(c => c.Id == id);
////            if (claim != null)claims.Remove(claim);
////            return RedirectToAction("Index");
////        }
////    }
////}
//using ContractMonthlyClaimSystem.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ContractMonthlyClaimSystem.Controllers
//{
//    public class LecturerController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public LecturerController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        //Display all claims
//        public IActionResult Index()
//        {
//            var claims = _context.Claims.ToList();
//            return View(claims);
//        }

//        //View details of a specific claim
//        public IActionResult Details(int id)
//        {
//            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
//            if (claim == null) return NotFound();
//            return View(claim);
//        }

//        //Create new claim (GET)
//        [HttpGet]
//        public IActionResult Create() => View();

//        //Create new claim (POST)
//        [HttpPost]
//        public IActionResult Create(Claim claim, IFormFile? document)
//        {
//            if (ModelState.IsValid)
//            {
//                // Handle file upload if present
//                if (document != null && document.Length > 0)
//                {
//                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
//                    if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

//                    var filePath = Path.Combine(uploadFolder, document.FileName);
//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        document.CopyTo(stream);
//                    }

//                    claim.DocumentPath = "/uploads/" + document.FileName;
//                }

//                // Save claim to database
//                _context.Claims.Add(claim);
//                _context.SaveChanges();

//                return RedirectToAction("Index");
//            }
//            return View(claim);
//        }

//        // Edit claim (GET)
//        [HttpGet]
//        public IActionResult Edit(int id)
//        {
//            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
//            if (claim == null) return NotFound();
//            return View(claim);
//        }

//        // Edit claim (POST)
//        [HttpPost]
//        public IActionResult Edit(Claim updatedClaim)
//        {
//            if (ModelState.IsValid)
//            {
//                var claim = _context.Claims.FirstOrDefault(c => c.Id == updatedClaim.Id);
//                if (claim == null) return NotFound();

//                claim.LecturerName = updatedClaim.LecturerName;
//                claim.Module = updatedClaim.Module;
//                claim.HoursWorked = updatedClaim.HoursWorked;
//                claim.HourlyRate = updatedClaim.HourlyRate;
//                claim.DocumentPath = updatedClaim.DocumentPath;
//                claim.Status = updatedClaim.Status;

//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(updatedClaim);
//        }

//        // Delete claim (GET)
//        [HttpGet]
//        public IActionResult Delete(int id)
//        {
//            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
//            if (claim == null) return NotFound();
//            return View(claim);
//        }

//        // Delete claim (POST)
//        [HttpPost, ActionName("Delete")]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
//            if (claim != null)
//            {
//                _context.Claims.Remove(claim);
//                _context.SaveChanges();
//            }

//            return RedirectToAction("Index");
//        }
//    }
//}
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Content("ModelState errors: " + string.Join(", ", errors));
            }

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

        [HttpPost]
        public IActionResult CheckStatus(int id, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            if(claim.statusCoord == "Approved" && claim.statusManager == "Approved")
            {
                claim.Status = "Approved";
            }
            else if(claim.statusCoord.IsNullOrEmpty() || claim.statusManager.IsNullOrEmpty() || claim.statusManager == "Pending")
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

