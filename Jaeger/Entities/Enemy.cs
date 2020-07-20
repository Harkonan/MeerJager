using Jaeger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    class Convoy
    {
        public Enemy[] Vessels { get; set; }
    }

    public class EnemyDirectory
    {
        public List<EnemyType> Frigates { get; set; }
    }
    public class EnemyType
    {
        public string UIName { get; set; }
        public Range Health { get; set; }
        public Range Profile { get; set; }
    }

    public class Enemy : Vessel
    {
        public Boolean CanSeePlayer { get; set; }
        public AquisitionBonus AquisitionBonus { get; set; }
        public Boolean PlayerCanSee { get; set; }
        public AquisitionBonus PlayerAquisitionBonus { get; set; }
        public int DistanceToPlayer { get; set; } //in meters



        public Enemy()
        {
            UIName = "Dover Class frigate";
            Health = 100;
            Profile = 50;
            Noise = 90;
            Torpedos = int.MaxValue;
            Shells = int.MaxValue;
            CanSeePlayer = false;
            PlayerCanSee = false;
            DistanceToPlayer = 40000;
            AccousticDetectionAbility = 1.0f;
            AquisitionBonus = new AquisitionBonus();
            PlayerAquisitionBonus = new AquisitionBonus();

            Depth = Depths.GetDepths[0];

            Weapon ForeBattery = new Weapon()
            {
                Damage = new Range(20, 50),
                EffectiveDepths = Depths.GetDepths[0..2].ToList(),
                HitPercent = 90,
                Range = new Range(500, 19850),
                ReloadRounds = 4,
                Type = WeaponType.Main_Battery,
                UIName = "QF 4-inch Mark XVI fore-mounted Battery"
            };

            Weapon DepthCharge = new Weapon()
            {
                Damage = new Range(80, 110),
                EffectiveDepths = Depths.GetDepths[1..4].ToList(),
                HitPercent = 90,
                Range = new Range(0, 1000),
                ReloadRounds = 2,
                Type = WeaponType.Depth_Charge,
                UIName = "Hedgehog Depth Charge"
            };

            Armament = new Weapon[2];
            Armament[0] = DepthCharge;
            Armament[1] = ForeBattery;
        }

        public void CycleWeapons(Player Target)
        {
            var LoadedArmament = Armament.Where(x => x.Status == WeaponStatus.loaded && x.EffectiveDepths.Contains(Target.Depth) && x.Range.Min < DistanceToPlayer && x.Range.Max > DistanceToPlayer);
            foreach (var Armament in LoadedArmament)
            {
                double profile = Math.Max(AcousticalDetectionCalculation(this, Target), VisualDetectionCalculation(this, Target));
                int damage = Armament.FireWeapon(profile);
                if (PlayerCanSee)
                {
                    Program.CurrentLog.WriteToLog(String.Format("{0} has fired {1}!", UIName, Armament.UIName));
                }
                else
                {
                    Program.CurrentLog.WriteToLog(String.Format("Incoming {0} fire from unknown vessel!", Armament.Type.ToString().Replace('_', ' ')));
                }
                Armament.Status = WeaponStatus.reloading;
                Target.SetDamage(damage);
            }
            ReloadGuns();
        }


        public void ChangeDistance(int change)
        {
            if (DistanceToPlayer + change > 0)
            {
                DistanceToPlayer += change;
            }
            else
            {
                DistanceToPlayer = 0;
            }

            if (PlayerCanSee)
            {
                Program.CurrentLog.WriteToLog(string.Format("Captain, {0} is now at {1}m", UIName, DistanceToPlayer));

            }
        }

        public void Engage()
        {
            if (PlayerCanSee && !CanSeePlayer)
            {
                CanSeePlayer = true;
                AquisitionBonus.AddAquisitionBonus();
                Program.CurrentLog.WriteToLog(string.Format("Captain, {0} is moving to engage!", UIName));
            }

        }
        public void Disengage()
        {
            if (PlayerCanSee && CanSeePlayer)
            {
                CanSeePlayer = false;
                Program.CurrentLog.WriteToLog(string.Format("Captain, {0} has lost us!", UIName));
            }
        }

        public void Spotted(string DetectionMethod)
        {
            if (!PlayerCanSee)
            {
                PlayerCanSee = true;
                PlayerAquisitionBonus.AddAquisitionBonus();
                Program.CurrentLog.WriteToLog(string.Format("{0} Spotted on {2} at {1}m!", UIName, DistanceToPlayer, DetectionMethod));
            }
        }
        public void Unspotted()
        {
            if (PlayerCanSee)
            {
                PlayerCanSee = false;
                Program.CurrentLog.WriteToLog(string.Format("We have lost track on the Enemy Ship!"));
            }
        }

        public void ReloadGuns()
        {
            foreach (var Weapon in Armament)
            {
                Weapon.Reload();
            }
        }

        public string GetDamageReport()
        {
            if (PlayerCanSee)
            {
                if (Health <= 0)
                {
                    return string.Format("{0}'s is sinking!", UIName);
                }
                if (Health <= 20)
                {
                    return string.Format("{0}'s hull is critically compormised", UIName);
                }
                else if (20 < Health && Health <= 60)
                {
                    return string.Format("{0}'s hull is heavily damaged", UIName);
                }
                else if (20 < Health && Health <= 60)
                {
                    return string.Format("{0}'s hull is damaged", UIName);
                }
                else if (60 < Health && Health <= 100)
                {
                    return string.Format("{0}'s hull is slightly damaged", UIName);
                }
                else
                {
                    return string.Format("{0}'s hull is untouched", UIName);
                }
            }
            else
            {
                if (Health <= 0)
                {
                    return string.Format("Detecting a sinking vessel!");
                }
                return "";
            }

        }

        public void SetDamage(int amount, string WeaponSystem)
        {
            if (amount > 0)
            {
                if (PlayerCanSee)
                {
                    Program.CurrentLog.WriteToLog(string.Format("{0} has hit {1}!", WeaponSystem, UIName));
                }
                else
                {
                    Program.CurrentLog.WriteToLog(string.Format("{0} has detonated!", WeaponSystem));
                }
                Program.CurrentLog.WriteToLog(GetDamageReport());

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
                Program.CurrentLog.WriteToLog(string.Format("{0} missed!", WeaponSystem));
            }
        }


        public void CheckDetection(Player player)
        {
            //Program.CurrentLog.WriteToLog(string.Format("Current Range {0}", DistanceToPlayer));

            //Can the Enemy See the player
            //Visual Check
            var EnemyVisualCheck = VisualDetectionRoll(this, player, PlayerAquisitionBonus.CurrentAquisitionBonus);
            var EnemyAcousticalCheck = AcousticalDetectionRoll(this, player, PlayerAquisitionBonus.CurrentAquisitionBonus);
            if (EnemyVisualCheck || EnemyAcousticalCheck)
            {
                Engage();
            }
            else
            {
                Disengage();
            }

            //Can the Player See this enemy
            //Visual Check
            var PlayerVisualCheck = VisualDetectionRoll(player, this, AquisitionBonus.CurrentAquisitionBonus);
            var PlayerAcousticalCheck = AcousticalDetectionRoll(player, this, AquisitionBonus.CurrentAquisitionBonus);
            if (PlayerVisualCheck || PlayerAcousticalCheck)
            {
                if (PlayerVisualCheck)
                {
                    Spotted("Periscope");
                }
                else
                {
                    Spotted("Sonar");
                }

            }
            else
            {
                Unspotted();
            }
        }

        public double getHighestDetection(Vessel SeekingVessel, Vessel HidingVessel)
        {
            return Math.Max(VisualDetectionCalculation(SeekingVessel, HidingVessel), AcousticalDetectionCalculation(SeekingVessel, HidingVessel));
        }

        private double VisualDetectionCalculation(Vessel SeekingVessel, Vessel HidingVessel)
        {
            double BaseProfile = HidingVessel.Profile * SeekingVessel.Depth.ChanceToSeeModifier;
            double DistanceVariance = (10000 - DistanceToPlayer);
            double DistanceFraction = DistanceVariance / 10000;
            double DistanceBase = DistanceFraction * BaseProfile;
            double Result = BaseProfile + DistanceBase;
            return Result;
        }
        private bool VisualDetectionRoll(Vessel SeekingVessel, Vessel HidingVessel, double AquisitionBonus)
        {
            var VisualDetectionChance = VisualDetectionCalculation(SeekingVessel, HidingVessel);
            double VisualRoll = Dice.RollPercentage() * AquisitionBonus;
            //Program.CurrentLog.WriteToLog(string.Format("{0} has a {1} detecting chance on vessel using visual and rolled {2}", SeekingVessel.UIName, VisualDetectionChance, VisualRoll));
            return (VisualDetectionChance > VisualRoll);

        }


        private double AcousticalDetectionCalculation(Vessel SeekingVessel, Vessel HidingVessel)
        {
            double DistanceMod = (double)DistanceToPlayer / (double)15000;

            double DistancePower = Math.Pow(DistanceMod, 0.33);

            double DistanceMultiplier = 0.5 / DistancePower;

            double Result = HidingVessel.Noise * DistanceMultiplier * SeekingVessel.AccousticDetectionAbility * HidingVessel.Depth.ChanceToHearModifer;

            return Result;
        }
        private bool AcousticalDetectionRoll(Vessel SeekingVessel, Vessel HidingVessel, double AquisitionBonus)
        {
            var AcousticalDetectionChance = AcousticalDetectionCalculation(SeekingVessel, HidingVessel);
            double AcousticalRoll = Dice.RollPercentage() * AquisitionBonus;
            //Program.CurrentLog.WriteToLog(string.Format("{0} has a {1} detecting chance on vessel using acoustics and rolled {2}", SeekingVessel.UIName, AcousticalDetectionChance, AcousticalRoll));
            return (AcousticalDetectionChance > AcousticalRoll);
        }

    }
}
