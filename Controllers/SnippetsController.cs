using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCodeManager.Data;
using MyCodeManager.Models;

namespace MyCodeManager.Controllers
{
    public class SnippetsController : Controller
    {
        private readonly MyCodeManagerContext _context;

        public SnippetsController(MyCodeManagerContext context)
        {
            _context = context;
        }

        // هذه الدالة الجديدة تقبل كلمة بحث
        public async Task<IActionResult> Index(string searchString)
        {
            // 1. نجلب كل الأكواد مبدئياً
            var snippets = from s in _context.Snippet
                           select s;

            // 2. إذا كتب المستخدم شيراً في البحث
            if (!String.IsNullOrEmpty(searchString))
            {
                // نبحث في العنوان (Title) أو اللغة (Language)
                snippets = snippets.Where(s => s.Title.Contains(searchString)
                                            || s.Language.Contains(searchString));
            }

            // 3. نرجع النتائج النهائية للصفحة
            return View(await snippets.ToListAsync());
        }

        // GET: Snippets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snippet = await _context.Snippet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (snippet == null)
            {
                return NotFound();
            }

            return View(snippet);
        }

        // GET: Snippets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Snippets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Language,Code")] Snippet snippet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(snippet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(snippet);
        }

        // GET: Snippets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snippet = await _context.Snippet.FindAsync(id);
            if (snippet == null)
            {
                return NotFound();
            }
            return View(snippet);
        }

        // POST: Snippets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Language,Code")] Snippet snippet)
        {
            if (id != snippet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(snippet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SnippetExists(snippet.Id))
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
            return View(snippet);
        }

        // GET: Snippets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snippet = await _context.Snippet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (snippet == null)
            {
                return NotFound();
            }

            return View(snippet);
        }

        // POST: Snippets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var snippet = await _context.Snippet.FindAsync(id);
            if (snippet != null)
            {
                _context.Snippet.Remove(snippet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SnippetExists(int id)
        {
            return _context.Snippet.Any(e => e.Id == id);
        }
    }
}
