﻿using Jaeger.Entities;
using Microsoft.EntityFrameworkCore;
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


    public class Enemy : Vessel
    {
        public Boolean CanSeePlayer { get; set; }
        public AquisitionBonus AquisitionBonus { get; set; }
        public Boolean PlayerCanSee { get; set; }
        public AquisitionBonus PlayerAquisitionBonus { get; set; }
        public int DistanceToPlayer { get; set; } //in meters
        public int DBID { get; set; }
        public string Class { get; set; }


        public Enemy()
        {
            UIName = "Dover Class frigate";
            CurrentHealth = 100;
            MaxHealth = 100;
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
            Armament = new List<Weapon>();
        }

        public static Enemy GetRandomEnemy()
        {
            Enemy Enemy = new Enemy();

            using (var db = new Data.Database.AppDataContext())
            {

                var Ships = db.Ships
                    .Include(x => x.Class)
                    .Include(x => x.Health)
                    .Include(x => x.Profile)
                    .Include(x => x.WeaponMounts).ThenInclude(m => m.PossibleWeapons).ThenInclude(w => w.Weapon).ThenInclude(w => w.Damage)
                    .Include(x => x.WeaponMounts).ThenInclude(m => m.PossibleWeapons).ThenInclude(w => w.Weapon).ThenInclude(w => w.Range)
                    .ToList();

                Data.Database.Ship RandomEnemy = (Data.Database.Ship)Dice.RandomFromList(Ships);

                Enemy.Class = RandomEnemy.Class.ClassName;
                Enemy.UIName = RandomEnemy.Name + " Class " + RandomEnemy.Class.ClassName;
                Enemy.MaxHealth = RandomEnemy.Health.WeightedRandom(2);
                Enemy.CurrentHealth = Enemy.MaxHealth;
                Enemy.Profile = RandomEnemy.Profile.WeightedRandom(2);
                Enemy.DistanceToPlayer = Dice.RandomBetweenTwo(10000, 20000);
                Enemy.DBID = RandomEnemy.ShipID;

                foreach (Data.Database.WeaponMount Mount in RandomEnemy.WeaponMounts)
                {
                    var PossibleWeapons = Mount.PossibleWeapons.Select(x => x.Weapon).ToList();
                    var DBWeapon = PossibleWeapons[Dice.RandomBetweenTwo(0, PossibleWeapons.Count())];
                    Weapon W = new Weapon(DBWeapon, Mount);
                    Enemy.Armament.Add(W);
                }

            }

            return Enemy;
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
            int HealthPercent = (int)(((double)CurrentHealth / (double)MaxHealth) * 100);
            if (PlayerCanSee)
            {
                if (HealthPercent <= 0)
                {
                    return string.Format("{0}'s is sinking!", UIName);
                }
                if (HealthPercent <= 20)
                {
                    return string.Format("{0}'s hull is critically compormised", UIName);
                }
                else if (20 < HealthPercent && HealthPercent <= 60)
                {
                    return string.Format("{0}'s hull is heavily damaged", UIName);
                }
                else if (20 < HealthPercent && HealthPercent <= 60)
                {
                    return string.Format("{0}'s hull is damaged", UIName);
                }
                else if (60 < HealthPercent && HealthPercent < 100)
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
                if (HealthPercent <= 0)
                {
                    return string.Format("Detecting a sinking vessel!");
                }
                return "";
            }

        }

        public List<string> GetCaptainsIntel()
        {
            using (var db = new Data.Database.AppDataContext())
            {
                List<String> Intel = new List<string>();
                var DBShip = db.Ships
                    .Include(x => x.Health)
                    .Include(x => x.WeaponMounts).ThenInclude(m => m.PossibleWeapons).ThenInclude(w => w.Weapon).ThenInclude(w => w.Damage)
                    .Include(x => x.WeaponMounts).ThenInclude(m => m.PossibleWeapons).ThenInclude(w => w.Weapon).ThenInclude(w => w.Range)
                    .Where(x => x.ShipID == DBID).FirstOrDefault();

                Intel.Add(String.Format("Captains Intel Book: {0}", UIName));
                Intel.Add("");
                Intel.Add(String.Format("Aprox resiliance to weapon systems"));
                foreach (var Weapon in Program.Player.Armament)
                {
                    int LowDam = (int)Math.Ceiling((double)DBShip.Health.Min / (double)Weapon.Damage.Max);
                    int HighDam = (int)Math.Ceiling((double)DBShip.Health.Max / (double)Weapon.Damage.Min);
                    Intel.Add(String.Format("{0} ({1}-{2}m): ~{3}-{4} to sink", Weapon.UIName, Weapon.Range.Min, Weapon.Range.Max, LowDam, HighDam));
                }
                Intel.Add("");
                Intel.Add(String.Format("Possible Armament:"));
                foreach (var Mount in DBShip.WeaponMounts)
                {
                    Intel.Add(String.Format("{0}", Mount.MountName, Mount.AlwaysExists ? "": " not always on Class"));
                    foreach (var Weapon in Mount.PossibleWeapons.Select(x => x.Weapon))
                    {
                        int LowDam = (int)Math.Ceiling((double)Program.Player.CurrentHealth / (double)Weapon.Damage.Max);
                        int HighDam = (int)Math.Ceiling((double)Program.Player.CurrentHealth / (double)Weapon.Damage.Min);

                        Intel.Add(String.Format("   - {0}: {1}-{2}m Range. ~{3}-{4} Shots to kill at our current state", Weapon.Name, Weapon.Range.Min, Weapon.Range.Max, LowDam, HighDam));
                    }
                }







                return Intel;
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

                if (CurrentHealth - amount <= 0)
                {
                    CurrentHealth = 0;
                }
                else
                {
                    CurrentHealth -= amount;
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
            //Program.CurrentLog.WriteToLog(string.Format("{0} has a {1} detecting chance on vessel using visual and rolled {2}, (with a bonus of {3})", SeekingVessel.UIName, VisualDetectionChance, VisualRoll, AquisitionBonus ));
            //Program.CurrentLog.WriteToLog("");
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
            //Program.CurrentLog.WriteToLog(string.Format("{0} has a {1} detecting chance on vessel using acoustics and rolled {2}, (with a bonus of {3})", SeekingVessel.UIName, AcousticalDetectionChance, AcousticalRoll, AquisitionBonus));
            //Program.CurrentLog.WriteToLog("");
            return (AcousticalDetectionChance > AcousticalRoll);
        }

    }
}
