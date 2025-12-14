using System.Collections.Generic;

public class Tag
{
    // 1. Id (المفتاح الأساسي)
    // Entity Framework Core يتعرف تلقائياً على خاصية اسمها 'Id' أو 'TagId' كمفتاح أساسي (Primary Key).
    public int Id { get; set; }

    // 2. Name (اسم الوسم)
    // هذا هو اسم الوسم الفعلي الذي يظهر للمستخدم (مثل: "LINQ", "Security", "Frontend").
    public string Name { get; set; }

    // 3. خاصية Navigation (للعلاقات)
    // هذه الخاصية ضرورية لتمثيل علاقة Many-to-Many بين الوسم والمقتطفات.
    // تشير إلى مجموعة من كائنات SnippetTag المرتبطة بهذا الوسم.
    public ICollection<SnippetTag> SnippetTags { get; set; }
}