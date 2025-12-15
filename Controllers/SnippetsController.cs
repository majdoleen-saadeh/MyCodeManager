using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // ضرورية لجلب هوية المستخدم
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // ضرورية للقفل
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCodeManager.Data;
using MyCodeManager.Models;

namespace MyCodeManager.Controllers
{
    // 1. [Authorize]: تمنع أي شخص غير مسجل من فتح أي صفحة هنا
    [Authorize]
    public class SnippetsController : Controller
    {
        private readonly MyCodeManagerContext _context;

        public SnippetsController(MyCodeManagerContext context)
        {
            _context = context;
        }

        // GET: Snippets (الصفحة الرئيسية)
        public async Task<IActionResult> Index(string searchString, string lang, bool showFavorites)
        {
            // 2. جلب هوية المستخدم الحالي
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 3. فلترة الأكواد: هات الأكواد الخاصة بهذا المستخدم فقط
            var snippets = from s in _context.Snippet
                           where s.UserId == userId
                           select s;

            // --- باقي كود الفلترة والبحث كما هو ---
            if (!String.IsNullOrEmpty(searchString))
            {
                snippets = snippets.Where(s => s.Title.Contains(searchString) || s.Code.Contains(searchString) || s.Description.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(lang))
            {
                snippets = snippets.Where(s => s.Language == lang);
            }

            if (showFavorites)
            {
                snippets = snippets.Where(s => s.IsFavorite);
            }

            // تخزين الفلاتر الحالية لتبقى في الصفحة
            ViewData["CurrentFilter"] = searchString;
            return View(await snippets.ToListAsync());
        }

        // GET: Snippets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // التأكد أن الكود يخص المستخدم الحالي
            var snippet = await _context.Snippet
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (snippet == null) return NotFound();

            return View(snippet);
        }

        // GET: Snippets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Snippets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Language,Code,IsFavorite,Description")] Snippet snippet)
        {
            if (ModelState.IsValid)
            {
                // 4. عند الحفظ: نختم الكود برقم هوية المستخدم الحالي
                snippet.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                snippet.CreatedAt = DateTime.Now;

                _context.Add(snippet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(snippet);
        }

        // GET: Snippets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // التأكد أن المستخدم يملك هذا الكود قبل التعديل
            var snippet = await _context.Snippet.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (snippet == null) return NotFound();

            return View(snippet);
        }

        // POST: Snippets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Language,Code,IsFavorite,Description,CreatedAt")] Snippet snippet)
        {
            if (id != snippet.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // الحفاظ على UserId القديم حتى لا يضيع
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    snippet.UserId = userId;

                    _context.Update(snippet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SnippetExists(snippet.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(snippet);
        }

        // POST: Snippets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // حذف الكود الخاص بالمستخدم فقط
            var snippet = await _context.Snippet.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (snippet != null)
            {
                _context.Snippet.Remove(snippet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Action للمفضلة (إضافة وتعديل)
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var snippet = await _context.Snippet.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (snippet != null)
            {
                snippet.IsFavorite = !snippet.IsFavorite;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SnippetExists(int id)
        {
            return _context.Snippet.Any(e => e.Id == id);
        }
    }
}