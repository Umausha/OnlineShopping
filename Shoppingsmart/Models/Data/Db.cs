using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shoppingsmart.Models.Data
{
    public class Db: DbContext
    {
        public DbSet<PageDto> pages { get; set; }
        public DbSet<SideDto> Sidebar { get; set; }
    }
}