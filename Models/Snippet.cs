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

        // --- الإضافات الجديدة ---
        public string? Description { get; set; } // الوصف (اختياري)
        public DateTime CreatedAt { get; set; } = DateTime.Now; // تاريخ الإضافة
    }
}