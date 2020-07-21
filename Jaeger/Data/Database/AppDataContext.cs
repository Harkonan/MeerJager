using MeerJager.Entities;
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
        public DbSet<WeaponMount> Mounts { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<WeaponToMountMap> WeaponToMountMap { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=.\\data\\Database\\AppData.db");
            options.EnableSensitiveDataLogging();
        }
           

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Range>().HasData(
                new Range { Id = 1, Min = 100, Max = 130 }, //Hunt Health
                new Range { Id = 2, Min = 8, Max = 85 }, //Hunt Profile
                new Range { Id = 3, Min = 120, Max = 130 }, //Black Swan Health
                new Range { Id = 4, Min = 11, Max = 91 }, //Black Swan Profile
                new Range { Id = 5, Min = 140, Max = 150 }, //Loch Health
                new Range { Id = 6, Min = 11, Max = 94 }, //Loch Profile
                new Range { Id = 7, Min = 30, Max = 50 }, //Merchant Health
                new Range { Id = 8, Min = 90, Max = 150 }, //Merchant Profile
                new Range { Id = 9, Min = 20, Max = 50 }, //Weapon 1 Damage
                new Range { Id = 10, Min = 500, Max = 19850 }, //Weapon 1 Ramge
                new Range { Id = 15, Min = 5, Max = 10 }, //Weapon 2 Damage
                new Range { Id = 16, Min = 500, Max = 3960 }, //Weapon 2 Ramge
                new Range { Id = 11, Min = 80, Max = 110 }, //Weapon Hedgehog Damage
                new Range { Id = 12, Min = 0, Max = 1000 }, //Weapon Hedgehog Ramge
                new Range { Id = 13, Min = 80, Max = 110 }, //Weapon Depth Charge Damage
                new Range { Id = 14, Min = 0, Max = 1000 } //Weapon Depth Charge Ramge
                , new Range { Id = 17, Min = 15, Max = 25} //Weapon 3 Damage
                , new Range { Id = 18, Min = 500, Max = 15000} //Weapon 3 Ramge

                );

            modelBuilder.Entity<Class>().HasData(
                new Class{ Id = 1, ClassName = "Frigate" },
                new Class { Id = 2, ClassName = "Merchant" },
                new Class { Id = 3, ClassName = "Sloop" },
                new Class { Id = 4, ClassName = "Destroyer" }
                );

            var Ship1 = new Ship { ShipID = 1, ClassId = 4, HealthId = 1, ProfileId = 2, Name = "Hunt" };
            var Ship2 = new Ship { ShipID = 2, ClassId = 3, HealthId = 3, ProfileId = 4, Name = "Black Swan" };
            var Ship3 = new Ship { ShipID = 3, ClassId = 1, HealthId = 5, ProfileId = 6, Name = "Loch" };
            var Ship4 = new Ship { ShipID = 4, ClassId = 2, HealthId = 7, ProfileId = 8, Name = "Vessel"};
            modelBuilder.Entity<Ship>().HasData(Ship1, Ship2, Ship3, Ship4);

            
            var HuntMount1 = new WeaponMount { Id = 1, ShipId = 1, AlwaysExists = false, MountName = "Fore Mounted Battery 1" }; //Hunt Mount
            var HuntMount2 = new WeaponMount { Id = 2, ShipId = 1, AlwaysExists = false, MountName = "Fore Mounted Battery 2" }; //Hunt Mount
            var HuntMount3 = new WeaponMount { Id = 3, ShipId = 1, AlwaysExists = false, MountName = "Aft Mounted Battery 1" }; //Hunt Mount
            var HuntMount4 = new WeaponMount { Id = 4, ShipId = 1, AlwaysExists = false, MountName = "Aft Mounted Battery 2" }; //Hunt Mount
            var HuntMount5 = new WeaponMount { Id = 17, ShipId = 1, AlwaysExists = true, MountName = "Rack" };//Hunt Mount

            var BlackSwanMount1 = new WeaponMount { Id = 5, ShipId = 2, AlwaysExists = true, MountName = "Rack" };
            var BlackSwanMount2= new WeaponMount { Id = 6, ShipId = 2, AlwaysExists = true, MountName = "AA Mount 1" };
            var BlackSwanMount3 = new WeaponMount { Id = 7, ShipId = 2, AlwaysExists = true, MountName = "AA Mount 2" };
            var BlackSwanMount4 = new WeaponMount { Id = 8, ShipId = 2, AlwaysExists = true, MountName = "AA Mount 3" };
            var BlackSwanMount5 = new WeaponMount { Id = 9, ShipId = 2, AlwaysExists = true, MountName = "AA Mount 4" };
            var BlackSwanMount6 = new WeaponMount { Id = 10, ShipId = 2, AlwaysExists = true, MountName = "AA Mount 5" };
            var BlackSwanMount7 = new WeaponMount { Id = 11, ShipId = 2, AlwaysExists = true, MountName = "AA Mount 6" };

            var LochMount1 = new WeaponMount { Id = 12, ShipId = 3, AlwaysExists = true, MountName = "Aft Battery 1" };
            var LochMount2 = new WeaponMount { Id = 13, ShipId = 3, AlwaysExists = true, MountName = "AA Battery 1" };
            var LochMount3 = new WeaponMount { Id = 14, ShipId = 3, AlwaysExists = true, MountName = "ASM Rack 1" };
            var LochMount4 = new WeaponMount { Id = 15, ShipId = 3, AlwaysExists = true, MountName = "ASM Rack 2" };
            var LochMount5 = new WeaponMount { Id = 16, ShipId = 3, AlwaysExists = true, MountName = "ASM Rack 3" };

            var MerchantMount1 = new WeaponMount { Id = 18, ShipId = 3, AlwaysExists = false, MountName = "Hidden Mount 1" };
            var MerchantMount2 = new WeaponMount { Id = 19, ShipId = 3, AlwaysExists = false, MountName = "Hidden Mount 2" };

            modelBuilder.Entity<WeaponMount>().HasData(
                HuntMount1
                , HuntMount2
                , HuntMount3
                , HuntMount4
                , HuntMount5
                , BlackSwanMount1
                , BlackSwanMount2
                , BlackSwanMount3
                , BlackSwanMount4
                , BlackSwanMount5
                , BlackSwanMount6
                , BlackSwanMount7
                , LochMount1
                , LochMount2
                , LochMount3
                , LochMount4
                , LochMount5
                , MerchantMount1
                , MerchantMount2
                );
            
            var Weapon1 = new Weapon { Id = 1, DamageId = 9, RangeId = 10, HitPercent = 80, ReloadRounds = 2,  Type = WeaponType.Main_Battery, Name = "Twin Mounted QF 4-inch Mark XVI" };
            var Weapon2 = new Weapon { Id = 2, DamageId = 15, RangeId = 16, HitPercent = 80, ReloadRounds = 1, Type = WeaponType.Secondary_Battery, Name = "Quad Mounted QF 2-pounder Mk. VIII AA" };
            var HedgeHogWeapon = new Weapon { Id = 3, DamageId = 11, RangeId = 12, HitPercent = 80, ReloadRounds = 5, Type = WeaponType.Depth_Charge, Name = "Hedgehog Depth Charge" };
            var SquidWeapon = new Weapon { Id = 6, DamageId = 11, RangeId = 12, HitPercent = 90, ReloadRounds = 3, Type = WeaponType.Depth_Charge, Name = "Squid Depth Charge" };
            var DepthChargeWeapon = new Weapon { Id = 4, DamageId = 13, RangeId = 14, HitPercent = 90, ReloadRounds = 4, Type = WeaponType.Depth_Charge, Name = "Depth Charge" };
            var Weapon3 = new Weapon { Id = 5, DamageId = 17, RangeId = 18, HitPercent = 70, ReloadRounds = 4, Type = WeaponType.Main_Battery, Name = "QF 4 inch gun MK V" };

            modelBuilder.Entity<Weapon>().HasData(
                Weapon1
                , Weapon2
                , Weapon3
                , HedgeHogWeapon
                , SquidWeapon
                , DepthChargeWeapon
                );

            //Hunt
            var WeaponToMountMap1 = new WeaponToMountMap { Id = 1, MountId = HuntMount1.Id, WeaponId = Weapon1.Id };
            var WeaponToMountMap2 = new WeaponToMountMap { Id = 2, MountId = HuntMount2.Id, WeaponId = Weapon1.Id };
            var WeaponToMountMap3 = new WeaponToMountMap { Id = 3, MountId = HuntMount3.Id, WeaponId = Weapon1.Id };
            var WeaponToMountMap4 = new WeaponToMountMap { Id = 4, MountId = HuntMount4.Id, WeaponId = Weapon1.Id };
            var WeaponToMountMap5 = new WeaponToMountMap { Id = 5, MountId = HuntMount1.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap6 = new WeaponToMountMap { Id = 6, MountId = HuntMount2.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap7 = new WeaponToMountMap { Id = 7, MountId = HuntMount3.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap8 = new WeaponToMountMap { Id = 8, MountId = HuntMount4.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap9 = new WeaponToMountMap { Id = 9, MountId = HuntMount5.Id, WeaponId = HedgeHogWeapon.Id };


            //Black Swan
            var WeaponToMountMap10 = new WeaponToMountMap { Id = 10, MountId = BlackSwanMount1.Id, WeaponId = DepthChargeWeapon.Id };
            var WeaponToMountMap11 = new WeaponToMountMap { Id = 11, MountId = BlackSwanMount2.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap12 = new WeaponToMountMap { Id = 12, MountId = BlackSwanMount3.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap13 = new WeaponToMountMap { Id = 13, MountId = BlackSwanMount4.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap14 = new WeaponToMountMap { Id = 14, MountId = BlackSwanMount5.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap15 = new WeaponToMountMap { Id = 15, MountId = BlackSwanMount6.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap16 = new WeaponToMountMap { Id = 16, MountId = BlackSwanMount7.Id, WeaponId = Weapon2.Id };

            //Loch
            var WeaponToMountMap17 = new WeaponToMountMap { Id = 17, MountId = LochMount1.Id, WeaponId = Weapon3.Id };
            var WeaponToMountMap18 = new WeaponToMountMap { Id = 18, MountId = LochMount2.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap19 = new WeaponToMountMap { Id = 19, MountId = LochMount3.Id, WeaponId = SquidWeapon.Id };
            var WeaponToMountMap20 = new WeaponToMountMap { Id = 20, MountId = LochMount4.Id, WeaponId = SquidWeapon.Id };
            var WeaponToMountMap21 = new WeaponToMountMap { Id = 21, MountId = LochMount5.Id, WeaponId = DepthChargeWeapon.Id };

            //Merchant
            var WeaponToMountMap22 = new WeaponToMountMap { Id = 22, MountId = MerchantMount1.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap23 = new WeaponToMountMap { Id = 23, MountId = MerchantMount1.Id, WeaponId = Weapon3.Id };
            var WeaponToMountMap24 = new WeaponToMountMap { Id = 25, MountId = MerchantMount2.Id, WeaponId = Weapon2.Id };
            var WeaponToMountMap25 = new WeaponToMountMap { Id = 24, MountId = MerchantMount2.Id, WeaponId = Weapon3.Id };

            modelBuilder.Entity<WeaponToMountMap>().HasData(
                WeaponToMountMap1
                , WeaponToMountMap2
                , WeaponToMountMap3
                , WeaponToMountMap4
                , WeaponToMountMap5
                , WeaponToMountMap6
                , WeaponToMountMap7
                , WeaponToMountMap8
                , WeaponToMountMap9
                , WeaponToMountMap10
                , WeaponToMountMap11
                , WeaponToMountMap12
                , WeaponToMountMap13
                , WeaponToMountMap14
                , WeaponToMountMap15
                , WeaponToMountMap16
                , WeaponToMountMap17
                , WeaponToMountMap18
                , WeaponToMountMap19
                , WeaponToMountMap20
                , WeaponToMountMap21
                , WeaponToMountMap22
                , WeaponToMountMap23
                , WeaponToMountMap24
                , WeaponToMountMap25
                );



        }
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
        public ICollection<WeaponMount> WeaponMounts { get; set; }
    }

    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public ICollection<Ship> Ships { get; } = new List<Ship>();
    }

    public class Range : MeerJager.Entities.Range
    {
        public int Id { get; set; }
    }

    public class WeaponMount
    {
        public int Id { get; set; }
        public string MountName { get; set; }
        public int? ShipId { get; set; }
        public virtual Ship Ship { get; set;}
        public bool AlwaysExists { get; set; }
        public ICollection<WeaponToMountMap> PossibleWeapons { get; } = new List<WeaponToMountMap>();
    }

    public class WeaponToMountMap {
        public int Id { get; set; }
        public int? WeaponId { get; set; }
        public virtual Weapon Weapon { get; set; }
        public int? MountId { get; set; }
        public virtual WeaponMount Mount { get; set; }
    }


    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RangeId { get; set; }
        public virtual Range Range { get; set; }
        public int DamageId { get; set; }
        public virtual Range Damage { get; set; }
        public int ReloadRounds { get; set; }
        public int HitPercent { get; set; }

        public WeaponType Type { get; set; }
        public ICollection<WeaponToMountMap> PossibleMounts { get; } = new List<WeaponToMountMap>();
    }


}
