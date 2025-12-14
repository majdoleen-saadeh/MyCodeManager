using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCodeManager.Models;

namespace MyCodeManager.Data
{
    public class MyCodeManagerContext : DbContext
    {
        public MyCodeManagerContext (DbContextOptions<MyCodeManagerContext> options)
            : base(options)
        {
        }

        public DbSet<MyCodeManager.Models.Snippet> Snippet { get; set; } = default!;
    }
}
