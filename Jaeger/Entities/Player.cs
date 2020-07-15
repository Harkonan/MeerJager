﻿using Jaeger.Entities;
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
        public List<ActiveTorpedos> ActiveTorpedos { get; set; }
        

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
                List<Depth> ValidDepths = new List<Depth>();
                ValidDepths.Add(Depths.GetDepths[0]);
                ValidDepths.Add(Depths.GetDepths[1]);

                Weapon weapon = new Weapon()
                {
                    Damage = 55,
                    Type = WeaponType.Torpedo,
                    ReloadRounds = 3,
                    HitPercent = 30,
                    Status = WeaponStatus.loaded,
                    UIName = "Tube " + (i + 1),
                    Speed = 500,
                    FiringDepths = ValidDepths
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
            ActiveTorpedos = new List<ActiveTorpedos>();
        }

        public void ExecuteAttack(){
            foreach (var weapon in Armament.Where(x => x.Status == WeaponStatus.firing))
            {
                if (weapon.Type == WeaponType.Torpedo)
                {
                    UIScreen.DisplayLines.Add(weapon.UIName + " Launched");
                    weapon.ReloadRoundsLeft = weapon.ReloadRounds;
                    Torpedos--;
                    weapon.Status = WeaponStatus.reloading;
                    ActiveTorpedos.Add(new ActiveTorpedos()
                    {
                        Damage = weapon.Damage,
                        Target = weapon.Target,
                        DistanceToTarget = weapon.Target.DistanceToPlayer,
                        HitPercent = weapon.HitPercent,
                        Speed = weapon.Speed.Value,
                        UIName = "Active Torpedo "+ (ActiveTorpedos.Count()+1)
                    });
                }
            }
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

        public void ReloadGuns()
        {
            foreach (var Weapon in Armament)
            {
                Weapon.Reload();
            }
        }

    }

    
}
