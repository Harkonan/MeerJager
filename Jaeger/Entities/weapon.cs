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
        public int ReloadRounds { get; set; }
        public int ReloadRoundsLeft { get; set; }
        public int HitPercent { get; set; }
        public Boolean Loaded { get; set; }

        public WeaponType Type { get; set; }

        public int FireWeapon(double _targetProfile)
        {
            Loaded = false;
            ReloadRoundsLeft = ReloadRounds;
            Console.WriteLine("Profile: "+_targetProfile);

            int Roll = Dice.RollPercentage();
            Console.WriteLine("Roll: "+Roll);
            Console.WriteLine("Enemy has Fired!");
            
            if (Roll > _targetProfile)
            {
                return Damage;
            }
            else
            {
                return 0;
            }
        }

        public void Reload()
        {
            if (!Loaded)
            {
                ReloadRoundsLeft--;
                if (ReloadRoundsLeft == 0)
                {
                    Loaded = true;
                }
            }
        }
    }

public enum WeaponType {MainBattery, Torpedo, SecondaryBattery }
}
