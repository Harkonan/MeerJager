using Jaeger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public sealed class Player : Vessel
    {
        private static Player Instance = null;


        public int Supplies { get; set; }
        public int PoliticalCapital { get; set; }
        public List<ActiveTorpedos> ActiveTorpedos { get; set; }
        public int? DesieredDistance { get; set; }
        public List<Enemy> SunkShips { get; set; } = new List<Enemy>();


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
            UIName = "Player";
            CurrentHealth = 100;
            MaxHealth = 100;
            Profile = 60;
            Noise = 60;
            Depth = Depths.GetDepths[2];
            Speed = Depth.MaxSpeedAtDepth;
            Armament = new List<Weapon>();
            for (int i = 0; i < 4; i++)
            {
                List<Depth> ValidDepths = new List<Depth>();
                ValidDepths.Add(Depths.GetDepths[0]);
                ValidDepths.Add(Depths.GetDepths[1]);

                Weapon weapon = new Weapon()
                {
                    Damage = new Range(40, 60),
                    Type = WeaponType.Torpedo,
                    ReloadRounds = 3,
                    HitPercent = 30,
                    Status = WeaponStatus.loaded,
                    UIName = "Tube " + (i + 1) +" - G7e/T4 Falke",
                    Speed = 500,
                    FiringDepths = Depths.GetDepths[0..2].ToList(),
                    Range = new Range(500,7500)
                };
                Armament.Add(weapon);
            }
            Weapon BowGun = new Weapon()
            {
                Type = WeaponType.Main_Battery,
                Damage = new Range(10, 20),
                HitPercent = 80,
                ReloadRounds = 1,
                UIName = "8.8cm SK C/35 - Bow Mounted",
                FiringDepths = new List<Depth>() { Depths.GetDepths[0] },
                Range = new Range(0, 11950)
            };
            Armament.Add(BowGun);
            Supplies = 200;
            PoliticalCapital = 10;
            Torpedos = 10;
            Shells = 220;
            ActiveTorpedos = new List<ActiveTorpedos>();
            AccousticDetectionAbility = 1.0f;
        }

        public void ExecuteAttack()
        {
            foreach (var weapon in Armament.Where(x => x.Status == WeaponStatus.firing))
            {
                if (weapon.Type == WeaponType.Torpedo)
                {
                    Program.CurrentLog.WriteToLog(weapon.UIName + " Launched");
                    ActiveTorpedos.Add(new ActiveTorpedos()
                    {
                        Damage = weapon.Damage.WeightedRandom(10),
                        Target = weapon.Target,
                        DistanceToTarget = weapon.Target.DistanceToPlayer,
                        HitPercent = weapon.HitPercent,
                        Speed = weapon.Speed.Value,
                        UIName = "Active Torpedo " + (ActiveTorpedos.Count() + 1)
                    });
                }
                else
                {
                    int Damage = weapon.FireWeapon(weapon.Target.getHighestDetection(this, weapon.Target));
                    weapon.Target.SetDamage(weapon.Damage.WeightedRandom(10), weapon.UIName);
                }
                weapon.Status = WeaponStatus.empty;
            }
            
        }

        public void RaiseDepth()
        {
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == Depth.DepthOrder + 1))
            {
                Depth = depths.Where(x => x.DepthOrder == Depth.DepthOrder + 1).FirstOrDefault();
                Program.CurrentLog.WriteToLog(String.Format("Depth set to {0}, aye", Depth.DepthName));
                ChangeSpeed(0);
            }

        }

        public void LowerDepth()
        {
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == Depth.DepthOrder - 1))
            {
                Depth = depths.Where(x => x.DepthOrder == Depth.DepthOrder - 1).FirstOrDefault();
                Program.CurrentLog.WriteToLog(String.Format("Depth set to {0}, aye", Depth.DepthName));
            }
        } 

        public void ChangeSpeed(int SpeedChange)
        {
            if (Speed + SpeedChange > Depth.MaxSpeedAtDepth)
            {
                Speed = Depth.MaxSpeedAtDepth;
                Program.CurrentLog.WriteToLog(String.Format("Speed set to {0} (Maximum for current Depth), aye", Speed));
            } else if( Speed + SpeedChange < -Depth.MaxSpeedAtDepth)
            {
                Speed = -Depth.MaxSpeedAtDepth;
                Program.CurrentLog.WriteToLog(String.Format("Speed set to {0} (Minimum for current Depth), aye", Speed));
            }
            else
            {
                Speed += SpeedChange;
                Program.CurrentLog.WriteToLog(String.Format("Speed set to {0} aye", Speed));
            }    

        }


        public string GetDamageReport()
        {
            int HealthPercent = (CurrentHealth / MaxHealth) * 100;

            if (HealthPercent <= 20)
            {
                return "Our hull is critically compormised";
            }
            else if (20 < HealthPercent && HealthPercent <= 60)
            {
                return "Our hull is heavily damaged";
            }
            else if (20 < HealthPercent && HealthPercent <= 60)
            {
                return "Our hull is damaged";
            }
            else if (60 < HealthPercent && HealthPercent < 100)
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
                Program.CurrentLog.WriteToLog("Hit!");
                if (CurrentHealth - amount <= 0)
                {
                    CurrentHealth = 0;
                }
                else
                {
                    CurrentHealth -= amount;
                }
                Program.CurrentLog.WriteToLog(GetDamageReport());
            }
            else
            {
                Program.CurrentLog.WriteToLog("They missed!");
            }
        }
    }


}
