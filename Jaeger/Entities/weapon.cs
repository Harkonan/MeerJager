using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public class Weapon
    {
        public int Damage { get; set; }
        public int ReloadTime { get; set; }
        public int HitPercent { get; set; }
        public Boolean Loaded { get; set; }

        public WeaponType Type { get; set; }
    }

public enum WeaponType {MainBattery, Torpedo, SecondaryBattery }
}
