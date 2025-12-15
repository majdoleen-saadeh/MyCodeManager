using Microsoft.AspNetCore.Identity; // 1. ضروري عشان يفهم نوع المستخدم
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCodeManager.Models;

namespace MyCodeManager.Data
{
    // 2. حددنا أن هذا السياق يستخدم IdentityUser الافتراضي
    public class MyCodeManagerContext : IdentityDbContext<IdentityUser>
    {
        public MyCodeManagerContext(DbContextOptions<MyCodeManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Snippet> Snippet { get; set; } = default!;
    }
}