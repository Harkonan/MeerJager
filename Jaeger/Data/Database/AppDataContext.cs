using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Data.Database
{
    public class AppDataContext : DbContext
    {
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<Range> ProfileRange { get; set; }
        public DbSet<Range> HealthRange { get; set; }
        


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=.\\data\\Database\\AppData.db");

    }

    public class Ship
    {
        public int ShipID { get; set; }
        public int? ClassId { get; set; }
        public Class Class { get; set; }
        public int? HealthId { get; set; }
        public virtual Range Health { get; set; }
        public int? ProfileId { get; set; }
        public virtual Range Profile { get; set; }
        public string Name { get; set; }


    }

    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public List<Ship> Ships { get; } = new List<Ship>();
    }

    public class Range : MeerJager.Entities.Range
    {
        public int Id { get; set; }
    }
}
