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
        public Boolean PlayerCanSee { get; set; }
        public int DistanceToPlayer { get; set; } //in meters



        public Enemy()
        {
            UIName = "Dover Class frigate";
            Health = 100;
            Profile = 50;
            Noise = 90;
            CanSeePlayer = false;
            PlayerCanSee = false;
            DistanceToPlayer = 40000;
            AccousticDetectionAbility = 1.0f;
            Depth = Depths.GetDepths[0];

            Weapon DepthCharge = new Weapon()
            {
                Damage = new Range(80, 110),
                EffectiveDepths = Depths.GetDepths[1..4].ToList(),
                HitPercent = 90,
                Range = new Range(0, 1000),
                ReloadRounds = 2,
                Type = WeaponType.DepthCharge,
                UIName = "Hedgehog Depth Charge"
            };

            Armament = new Weapon[1];
            Armament[0] = DepthCharge;







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
                    UIScreen.DisplayLines.Add(String.Format("{0} has fired {1}!", UIName, Armament.UIName));
                }
                else
                {
                    UIScreen.DisplayLines.Add(String.Format("Incoming {0} fire from unknown vessel!", Armament.Type.ToString()));
                }
                Armament.Status = WeaponStatus.reloading;
                Target.SetDamage(damage);
            }
            ReloadGuns();
        }

        public void CloseDistance()
        {
            ChangeDistance(-100);
        }

        public void OpenDistance()
        {
            ChangeDistance(100);
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
                UIScreen.DisplayLines.Add(string.Format("Captain, {0} is now at {1}m", UIName, DistanceToPlayer));
            }
        }

        public void Engage()
        {
            if (PlayerCanSee && !CanSeePlayer)
            {
                CanSeePlayer = true;
                UIScreen.DisplayLines.Add(string.Format("Captain, {0} is moving to engage!", UIName));
            }

        }
        public void Disengage()
        {
            if (PlayerCanSee && CanSeePlayer)
            {
                CanSeePlayer = false;
                UIScreen.DisplayLines.Add(string.Format("Captain, {0} has lost us!", UIName));
            }
        }

        public void Spotted(string DetectionMethod)
        {
            if (!PlayerCanSee)
            {
                PlayerCanSee = true;
                UIScreen.DisplayLines.Add(string.Format("{0} Spotted on {2} at {1}m!", UIName, DistanceToPlayer, DetectionMethod));
            }
        }
        public void Unspotted()
        {
            if (PlayerCanSee)
            {
                PlayerCanSee = false;
                UIScreen.DisplayLines.Add(string.Format("We have lost track on the Enemy Ship!"));
            }
        }

        public void ReloadGuns()
        {
            foreach (var gun in Armament)
            {
                gun.Reload();
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
                else if (60 < Health && Health <= 99)
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
                    UIScreen.DisplayLines.Add(string.Format("{0} has hit {1}!", WeaponSystem, UIName));
                }
                else
                {
                    UIScreen.DisplayLines.Add(string.Format("{0} has detonated!", WeaponSystem));
                }
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
                UIScreen.DisplayLines.Add(string.Format("{0} missed!", WeaponSystem));
            }
        }


        public void CheckDetection(Player player)
        {
            //UIScreen.DisplayLines.Add(string.Format("Current Range {0}", DistanceToPlayer));

            //Can the Enemy See the player
            //Visual Check
            var EnemyVisualCheck = VisualDetectionRoll(this, player);
            var EnemyAcousticalCheck = AcousticalDetectionRoll(this, player);
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
            var PlayerVisualCheck = VisualDetectionRoll(player, this);
            var PlayerAcousticalCheck = AcousticalDetectionRoll(player, this);
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

        private double VisualDetectionCalculation(Vessel SeekingVessel, Vessel HidingVessel)
        {
            double BaseProfile = HidingVessel.Profile * SeekingVessel.Depth.ChanceToSeeModifier;
            double DistanceVariance = (10000 - DistanceToPlayer);
            double DistanceFraction = DistanceVariance / 10000;
            double DistanceBase = DistanceFraction * BaseProfile;
            double Result = BaseProfile + DistanceBase;
            return Result;
        }
        private bool VisualDetectionRoll(Vessel SeekingVessel, Vessel HidingVessel)
        {
            var VisualDetectionChance = VisualDetectionCalculation(SeekingVessel, HidingVessel);
            int VisualRoll = Dice.RollPercentage();
            //UIScreen.DisplayLines.Add(string.Format("{0} has a {1} detecting chance on vessel using visual and rolled {2}", SeekingVessel.UIName, VisualDetectionChance, VisualRoll));
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
        private bool AcousticalDetectionRoll(Vessel SeekingVessel, Vessel HidingVessel)
        {
            var AcousticalDetectionChance = AcousticalDetectionCalculation(SeekingVessel, HidingVessel);
            int AcousticalRoll = Dice.RollPercentage();
            //UIScreen.DisplayLines.Add(string.Format("{0} has a {1} detecting chance on vessel using acoustics and rolled {2}", SeekingVessel.UIName, AcousticalDetectionChance, AcousticalRoll));
            return (AcousticalDetectionChance > AcousticalRoll);
        }

    }
}
