using Microsoft.AspNetCore.Identity; // إضافة مكتبة الهوية
using Microsoft.EntityFrameworkCore;
using MyCodeManager.Data;

namespace MyCodeManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. إعداد الاتصال بقاعدة البيانات
            var connectionString = builder.Configuration.GetConnectionString("MyCodeManagerContext")
                ?? throw new InvalidOperationException("Connection string 'MyCodeManagerContext' not found.");

            builder.Services.AddDbContext<MyCodeManagerContext>(options =>
                options.UseSqlServer(connectionString));

            // 2. === تفعيل خدمة Identity (جديد) ===
            // هذا السطر يربط نظام المستخدمين بقاعدة بياناتك
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<MyCodeManagerContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // 3. === تفعيل التوثيق (جديد) ===
            // مهم جداً: يجب أن يكون Authentication قبل Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Snippets}/{action=Index}/{id?}")
                .WithStaticAssets();

            // 4. === تشغيل صفحات الدخول (جديد) ===
            app.MapRazorPages();

            app.Run();
        }
    }
}