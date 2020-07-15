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
                    ReloadRounds = 120,
                    HitPercent = 30,
                    Loaded = true,
                    UIName = "Tube "+i++
                };
                Armament[i] = weapon;
            }
            Weapon BowGun = new Weapon()
            {
                Type = WeaponType.MainBattery,
                Damage = 10,
                HitPercent = 80,
                ReloadRounds = 5
            };
            Armament[4] = BowGun;
            Supplies = 200;
            PoliticalCapital = 10;
            Torpedos = 22;
        }

        public void FireWeapon(int x){
            
        
        }

        public void RaiseDepth()
        {
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == Depth.DepthOrder + 1))
            {
                Depth = depths.Where(x => x.DepthOrder == Depth.DepthOrder + 1).FirstOrDefault();
                UIScreen.DisplayLines.Add(String.Format("Depth set to {0} aye", Depth.DepthName));
            }
        }

        public void LowerDepth()
        {
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == Depth.DepthOrder - 1))
            {
                Depth = depths.Where(x => x.DepthOrder == Depth.DepthOrder - 1).FirstOrDefault();
                UIScreen.DisplayLines.Add(String.Format("Depth set to {0} aye", Depth.DepthName));
            }
        }

        public double GetProfile(double _enemyDetectionAbility, int _enemyDistance)
        {
            double playerProfileAfterDepth = Profile * Depth.ProfileMultiplier;
            double baseDetectionChancePerHundredMeters = _enemyDetectionAbility * playerProfileAfterDepth; //players depth profile modified by the enemy detection ability
            double hundredMetersToPlayer = _enemyDistance / 100;
            double finalDetectionChance = baseDetectionChancePerHundredMeters * Math.Floor(hundredMetersToPlayer);
            return finalDetectionChance;
        }

        public string GetDamageReport()
        {
            if (Health <= 20)
            {
                return "Our hull is critically compormised";
            }
            else if (20 < Health && Health <= 60)
            {
                return "Our hull is heavily damaged";
            }
            else if (20 < Health && Health <= 60)
            {
                return "Our hull is damaged";
            }
            else if (60 < Health && Health <= 99)
            {
                return "Our hull is slightly damaged";
            }
            else
            {
                return "Our hull is untouched";
            }
        } 

        public void SetDamage(int amount)
        {
            if (amount > 0)
            {
                UIScreen.DisplayLines.Add("Hit!");
                UIScreen.DisplayLines.Add(GetDamageReport());

                if (Health - amount <= 0)
                {
                    Health = 0;
                }
                else
                {
                    Health -= amount;
                }
            }
            else
            {
                UIScreen.DisplayLines.Add("They missed!");
            }
        }

        

    }

    
}
