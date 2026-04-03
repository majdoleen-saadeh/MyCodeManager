using System;

namespace MyCodeManager.Models
{
    public class Snippet
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
        public bool IsFavorite { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsPublic { get; set; } = false; // هل الكود متاح للجميع؟
        public string? ShareToken { get; set; }    // رمز فريد للمشاركة (مثل Guid)

        // === الإضافة الجديدة ===
        // هذه الخانة ستخزن رقم هوية المستخدم الذي أنشأ الكود
        public string? UserId { get; set; }
    }
}