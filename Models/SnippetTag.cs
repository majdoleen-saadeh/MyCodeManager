using MyCodeManager.Models;

public class SnippetTag
{
    // هذا الجدول يمثل علاقة Many-to-Many

    // المفاتيح الأجنبية المركبة (Composite Foreign Key)
    public int SnippetId { get; set; }
    public int TagId { get; set; }

    // خاصيات Navigation
    public Snippet Snippet { get; set; }
    public Tag Tag { get; set; }
}