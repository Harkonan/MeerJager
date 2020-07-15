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

    public class Enemy
    {
        public int Health { get; set; }
        public int Profile { get; set; }
        public Weapon[] Armament { get; set; }
        public Boolean isEngaged { get; set; }
        public Boolean playerCanSee { get; set; }
        public int DistanceToPlayer { get; set; } //in meters
        public double DetectionAbility { get; set; } // 0 to 1 what percentage of the profile does the detection calculation use
        public string UIName { get; set; }


        public Enemy()
        {
            UIName = "Dover Class frigate";
            Health = 100;
            Profile = 50;
            isEngaged = false;
            playerCanSee = false;
            DistanceToPlayer = 2000;
            DetectionAbility = 1;
            Weapon Gun = new Weapon()
            {
                Damage = 10,
                ReloadRounds = 1,
                Status = WeaponStatus.loaded,
                Type = WeaponType.MainBattery
            };
            Armament = new Weapon[1];
            Armament[0] = Gun;
        }

        public void CycleWeapons(Player Target){
            var loadedGuns = Armament.Where(x => x.Status == WeaponStatus.loaded);
            foreach (var gun in loadedGuns)
            {
                double profile = Target.GetProfile(DetectionAbility, DistanceToPlayer);
                int damage = gun.FireWeapon(profile);
                UIScreen.DisplayLines.Add(String.Format("{0} has fired!", UIName));
                Target.SetDamage(damage);
                   
            }

            ReloadGuns();
        }

        public void PlayerCloseDistance()
        {
            ChangeDistance(-100);
        }

        public void PlayerOpenDistance()
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


            UIScreen.DisplayLines.Add(string.Format("Captain, {0} is now at {1}km", UIName, DistanceToPlayer));
        }

        public void Engage()
        {
            isEngaged = true;
            UIScreen.DisplayLines.Add(string.Format("Captain, {0} is moving to engage!", UIName));
        }

        public void Spotted()
        {
            if (!playerCanSee)
            {
                playerCanSee = true;
                UIScreen.DisplayLines.Add(string.Format("{0} Spotted!", UIName));
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
    }
}
