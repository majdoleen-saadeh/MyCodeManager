using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCodeManager.Services;
using MyCodeManager.Data;      // للاتصال بقاعدة البيانات
using MyCodeManager.Models;    // للوصول إلى جدول Snippets
using System.Security.Claims;  // لمعرفة هوية المستخدم
using System.Threading.Tasks;
using System;

namespace MyCodeManager.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly GeminiAIService _aiService;
        private readonly MyCodeManagerContext _context; // إضافة قاعدة البيانات

        // حقن الخدمتين (الذكاء الاصطناعي + قاعدة البيانات)
        public AIController(GeminiAIService aiService, MyCodeManagerContext context)
        {
            _aiService = aiService;
            _context = context;
        }

        public IActionResult Chat()
        {
            return View();
        }

        public class PromptModel
        {
            public string Text { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] PromptModel prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt.Text))
                return BadRequest("لا يوجد سؤال.");

            var answer = await _aiService.AskAIAsync(prompt.Text);
            return Json(new { response = answer });
        }

        // === الإضافة الجديدة: دالة حفظ الكود السريعة ===
        public class SaveSnippetModel
        {
            public string Code { get; set; }
            public string Language { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SaveSnippet([FromBody] SaveSnippetModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Code))
                return BadRequest("لا يوجد كود للحفظ.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // إنشاء كود جديد وربطه بالمستخدم الحالي
            var snippet = new Snippet
            {
                Title = "AI Generated Code", // عنوان افتراضي
                Language = string.IsNullOrEmpty(model.Language) ? "C#" : model.Language,
                Code = model.Code,
                Description = "تم توليد هذا الكود وحفظه بواسطة المساعد الذكي.",
                UserId = userId,
                CreatedAt = DateTime.Now,
                IsFavorite = false
            };

            _context.Snippet.Add(snippet);
            await _context.SaveChangesAsync();

            // نرجع رسالة نجاح مع رقم الكود
            return Json(new { success = true, id = snippet.Id });
        }
    }
}