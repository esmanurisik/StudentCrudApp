using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCrudApp.Models;
using StudentCrudApp.Data;
using StudentCrudApp.Helpers;

namespace StudentCrudApp.Controllers;

public class StudentsController : Controller
{
    private readonly SchoolContext _context;

    public StudentsController(SchoolContext context)
    {
        _context = context;
    }

    // GET: Students (Listeleme, Arama, Sıralama, Sayfalama)
    public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        if (searchString != null) { pageNumber = 1; }
        else { searchString = currentFilter; }

        ViewData["CurrentFilter"] = searchString;

        var students = _context.Students.AsQueryable();

        // Arama kısmı
        if (!String.IsNullOrEmpty(searchString))
        {
            students = students.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString) || s.Class.Contains(searchString) || s.Number.Contains(searchString));
        }

        // Sıralama kısmı
        switch (sortOrder)
        {
            case "name_desc": students = students.OrderByDescending(s => s.FirstName); break;
            case "name_asc": students = students.OrderBy(s => s.FirstName); break;
            case "surname_desc": students = students.OrderByDescending(s => s.LastName); break;
            case "surname_asc": students = students.OrderBy(s => s.LastName); break;
            case "class_desc": students = students.OrderByDescending(s => s.Class); break;
            case "class_asc": students = students.OrderBy(s => s.Class); break;
            case "number_desc": students = students.OrderByDescending(s => s.Number); break;
            case "number_asc": students = students.OrderBy(s => s.Number); break;
            default: students = students.OrderBy(s => s.LastName); break;
        }

        int pageSize = 5; // Sayfada gösterilecek öğrenci sayısı
        return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    // GET: Students/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
        if (student == null) return NotFound();
        return View(student);
    }

    // GET: Students/Create
    public IActionResult Create() => View();

    // POST: Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Class,Number")] Student student)
    {
        if (_context.Students.Any(s => s.Number == student.Number))
        {
            ModelState.AddModelError("Number", "Bu numaraya sahip bir öğrenci zaten kayıtlı.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: Students/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }

    // POST: Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Class,Number")] Student student)
    {
        if (id != student.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: Students/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
        if (student == null) return NotFound();
        return View(student);
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int id) => _context.Students.Any(e => e.Id == id);
}