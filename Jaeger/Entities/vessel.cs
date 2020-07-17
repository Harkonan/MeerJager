using Jaeger.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Entities
{
    public class Vessel
    {
        public int Health { get; set; }
        public Weapon[] Armament { get; set; }
        public int Speed { get; set; }


        public string UIName { get; set; }
        public Depth Depth { get; set; }
        public int BaseRange { get; set; }
        public int Profile { get; set; } //profile is how easy it is to see the vessel
        public int Torpedos { get; set; }
        public int Shells { get; set; }

        public double AccousticDetectionAbility { get; set; }
        public int Noise { get; set; } //Noise is the volume of the vessel for detection Purposes ~60 for a submarine, ~90 for a frigate ~250 for a dreadnaught seems balanced at optimal torp range

        public void ReloadWeapons()
        {
            foreach (var Weapon in Armament)
            {
                if (Weapon.Status == WeaponStatus.reloading)
                {
                    Weapon.Reload();
                }
                if (Weapon.Status == WeaponStatus.empty)
                {
                    if (Weapon.Type == WeaponType.Torpedo && Torpedos > 0)
                    {
                        Torpedos--;
                        Weapon.Reload();
                    }
                    if (Weapon.Type != WeaponType.Torpedo && Shells > 0)
                    {
                        Shells--;
                        Weapon.Reload();
                    }
                }
            }
        }


    }
}
