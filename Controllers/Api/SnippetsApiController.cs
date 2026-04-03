using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCodeManager.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCodeManager.Controllers.Api
{
    [Route("api/snippets")]
    [ApiController]
    public class SnippetsApiController : ControllerBase
    {
        private readonly MyCodeManagerContext _context;

        public SnippetsApiController(MyCodeManagerContext context)
        {
            _context = context;
        }

        // 1. نقطة اتصال لجلب الأكواد العامة (متاحة للجميع)
        [HttpGet("public")]
        public async Task<IActionResult> GetPublicSnippets()
        {
            var snippets = await _context.Snippet
                .Where(s => s.IsPublic)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Language,
                    s.Description,
                    s.ShareToken,
                    // لم نقم بجلب الـ UserId لأسباب أمنية
                })
                .ToListAsync();

            return Ok(snippets);
        }

        // 2. نقطة اتصال البحث الذكي (متاحة فقط للمستخدم المسجل)
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchMySnippets(string term)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // إذا كان مربع البحث فارغاً، نرجع مصفوفة فارغة
            if (string.IsNullOrWhiteSpace(term))
                return Ok(new object[] { });

            // نبحث عن الأكواد التي يحتوي عنوانها أو لغتها على الكلمة المكتوبة
            var results = await _context.Snippet
                .Where(s => s.UserId == userId && (s.Title.Contains(term) || s.Language.Contains(term)))
                .Take(5) // نكتفي بأول 5 نتائج لتكون القائمة سريعة
                .Select(s => new
                {
                    id = s.Id,
                    title = s.Title,
                    language = s.Language
                })
                .ToListAsync();

            return Ok(results);
        }
    }
}