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

    class Enemy
    {
        public int Health { get; set; }
        public int Profile { get; set; }
        public Weapon[] Armament { get; set; }
        public Boolean isEngaged { get; set; }
        public Boolean playerCanSee { get; set; }
        public int DistanceToPlayer { get; set; } //in meters
        public double DetectionAbility { get; set; } // 0 to 1 what percentage of the profile does the detection calculation use


        public Enemy()
        {
            Health = 100;
            Profile = 50;
            isEngaged = false;
            playerCanSee = false;
            DistanceToPlayer = 2000;
            DetectionAbility = 1;
            Weapon Gun = new Weapon()
            {
                Damage =  20,
                HitPercent = 20,
                ReloadTime = 10,
                Type = WeaponType.MainBattery
            };
            Armament = new Weapon[1];
            Armament[0] = Gun;
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

            Console.WriteLine("Captain, enemy is now at {0}km", DistanceToPlayer);
        }

        public void Engage()
        {
            isEngaged = true;
            Console.WriteLine("Captain, Enemy is moving to engage!");
        }

        public void Spotted()
        {
            if (!playerCanSee)
            {
                playerCanSee = true;
                Console.WriteLine("Enemy Spotted!");
            }
        }
    }
}
