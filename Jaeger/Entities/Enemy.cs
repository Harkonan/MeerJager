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
        public int HealthMax { get; set; }
        public int HealthMin { get; set; }
        public int ProfileMin { get; set; }
        public int ProfileMax { get; set; }
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
            CanSeePlayer = false;
            PlayerCanSee = false;
            DistanceToPlayer = 40000;
            AccousticDetectionAbility = 0.5f;
            VisualDetectionAbility = 0.5f;
            Weapon Gun = new Weapon()
            {
                Damage = 10,
                ReloadRounds = 1,
                Status = WeaponStatus.loaded,
                Type = WeaponType.MainBattery
            };
            Armament = new Weapon[1];
            Armament[0] = Gun;
            Depth = Depths.GetDepths[0];
        }

        public void CycleWeapons(Player Target)
        {
            var loadedGuns = Armament.Where(x => x.Status == WeaponStatus.loaded);
            foreach (var gun in loadedGuns)
            {
                //double profile = Target.GetProfile(DetectionAbility, DistanceToPlayer);
                //int damage = gun.FireWeapon(profile);
                //UIScreen.DisplayLines.Add(String.Format("{0} has fired!", UIName));
                //Target.SetDamage(damage);

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
            CanSeePlayer = true;
            UIScreen.DisplayLines.Add(string.Format("Captain, {0} is moving to engage!", UIName));
        }
        public void Disengage()
        {
            CanSeePlayer = false;
            UIScreen.DisplayLines.Add(string.Format("Captain, {0} has lost us!", UIName));
        }

        public void Spotted()
        {
            if (!PlayerCanSee)
            {
                PlayerCanSee = true;
                UIScreen.DisplayLines.Add(string.Format("{0} Spotted at {1}m!", UIName, DistanceToPlayer));
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

        public void SetDamage(int amount, string WeaponSystem)
        {
            if (amount > 0)
            {
                UIScreen.DisplayLines.Add(string.Format("{0} has hit {1}!", WeaponSystem, UIName));
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

        public double GetProfile(double _playerDetectionAbility)
        {
            double ProfileAfterDepth = (Profile / 10) * Depth.ChanceToSeeModifier;
            double baseDetectionChancePerHundredMeters = _playerDetectionAbility * ProfileAfterDepth; //players depth profile modified by the enemy detection ability
            double hundredMetersToPlayer = DistanceToPlayer / 100;
            double finalDetectionChance = baseDetectionChancePerHundredMeters * Math.Floor(hundredMetersToPlayer);
            return finalDetectionChance;
        }

        public void CheckDetection(Player player)
        {
            //Can the Enemy See the player
            //Visual Check
            var BaseProfile = Profile * Depth.ChanceToSeeModifier;
            var VisualDetectionChance = BaseProfile + (BaseProfile * ((10000 * DistanceToPlayer) / 10000));

            var AcousticalDetectionChance = Noise * Math.Pow(1.2 / (DistanceToPlayer / 1000), 0.25) * Depth.ChanceToHearModifer;

            int VisualRoll = Dice.RollPercentage();
            int AcousticalRoll = Dice.RollPercentage();

            if (VisualDetectionChance > VisualRoll || AcousticalDetectionChance > AcousticalRoll )
            {
                Engage();
            }
            else
            {
                Disengage();
            }

            //Can the Player See this enemy
            //Visual Check
            var PlayerBaseProfile = player.Profile * player.Depth.ChanceToSeeModifier;
            var PlayerVisualDetectionChance = PlayerBaseProfile + (PlayerBaseProfile * ((10000 * DistanceToPlayer) / 10000));

            var PlayerAcousticalDetectionChance = player.Noise * Math.Pow(1.2 / (DistanceToPlayer / 1000), 0.25) * player.Depth.ChanceToHearModifer;

            int PlayerVisualRoll = Dice.RollPercentage();
            int PlayerAcousticalRoll = Dice.RollPercentage();

            if (PlayerVisualDetectionChance > PlayerVisualRoll || PlayerAcousticalDetectionChance > PlayerAcousticalRoll)
            {
                Spotted();
            }
            else
            {
                Unspotted();
            }
        }

    }
}
