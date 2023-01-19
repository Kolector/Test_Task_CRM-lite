using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Task_CRM_lite.Models;

namespace Test_Task_CRM_lite.Database
{
    public class SQliteContext : DbContext
    {
        SQliteContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Role> Role { get; set; }
    }
}
