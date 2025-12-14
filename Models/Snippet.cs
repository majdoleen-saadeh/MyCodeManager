namespace MyCodeManager.Models
{
    public class Snippet
    {
        public int Id { get; set; }
        public string Title { get; set; } // عنوان الكود
        public string Language { get; set; } // لغة البرمجة
        public string Code { get; set; } // الكود نفسه
                                         // الإضافة الجديدة
        public bool IsFavorite { get; set; }
    }
}