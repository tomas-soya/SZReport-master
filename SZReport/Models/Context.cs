using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZReport.Models
{
    public class Context : DbContext
    {
        //更新数据库（程序包管理器控制台）：
        //Add-Migration InitialCreate
        //Update-Database
        public DbSet<SHA> SHAs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=SZ.db");
    }


}
