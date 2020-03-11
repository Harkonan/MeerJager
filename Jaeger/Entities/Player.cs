using Jaeger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public sealed class Player
    {
        private static Player Instance = null;

        public int Health { get; set; }
        public Weapon[] Armament { get; set; }
        public int Torpedos { get; set; }
        public int Supplies { get; set; }
        public int Profile { get; set; } //profile is the size of the ships profile
        public int PoliticalCapital { get; set; }
        public Depth Depth { get; set; }
        

        public static Player GetPlayer
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new Player();
                }
                return Instance;
            }
        }


        Player()
        {
            Health = 100;
            Profile = 10;
            Depth = Depths.GetDepths[2];
            Armament = new Weapon[5];
            for (int i = 0; i < 4; i++)
            {
                Weapon weapon = new Weapon()
                {
                    Damage = 55,
                    Type = WeaponType.Torpedo,
                    ReloadTime = 120,
                    HitPercent = 30
                };
                Armament[i] = weapon;
            }
            Weapon BowGun = new Weapon()
            {
                Type = WeaponType.MainBattery,
                Damage = 10,
                HitPercent = 80,
                ReloadTime = 5
            };
            Armament[4] = BowGun;
            Supplies = 200;
            PoliticalCapital = 10;
            Torpedos = 22;
        }

        public void RaiseDepth()
        {
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == Depth.DepthOrder + 1))
            {
                Depth = depths.Where(x => x.DepthOrder == Depth.DepthOrder + 1).FirstOrDefault();                
            }
        }

        public void LowerDepth()
        {
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == Depth.DepthOrder - 1))
            {
                Depth = depths.Where(x => x.DepthOrder == Depth.DepthOrder - 1).FirstOrDefault();
            }
        }

    }

    
}
