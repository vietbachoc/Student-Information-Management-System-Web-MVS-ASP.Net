using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using StudentInformationManagementSystem.Data;
using StudentInformationManagementSystem.Models;

namespace StudentInformationManagementSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly SchoolManagementDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StudentsController(SchoolManagementDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return _context.Students != null ?
                          View(await _context.Students.ToListAsync()) :
                          Problem("Entity set 'SchoolManagementDbContext.Students'  is null.");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentsViewsModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (studentViewModel.photo != null)
                {
                    try
                    {
                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }
                        filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(studentViewModel.photo.FileName);
                        string filePath = Path.Combine(uploadFolder, filename);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await studentViewModel.photo.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = $"An error occurred while uploading the photo: {ex.Message}";
                        return View(studentViewModel);
                    }
                }
                var student = new Student
                {
                    FirstName = studentViewModel.firstName,
                    LastName = studentViewModel.lastName,
                    Gender = studentViewModel.gender,
                    Images = filename,
                    DateOfBirth = studentViewModel.DoB,
                    Email = studentViewModel.email,
                    Address = studentViewModel.address,
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                ViewBag.success = "Record added";
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel); // Redirect to index action
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'SchoolManagementDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
