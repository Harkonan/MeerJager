using Jaeger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public class Weapon
    {
        public Range Damage { get; set; }

        public int ReloadRounds { get; set; }
        public int ReloadRoundsLeft { get; set; }
        public int HitPercent { get; set; }
        public int? Speed { get; set; }
        public string UIName { get; set; }
        public WeaponType Type { get; set; }
        public Enemy Target { get; set; }
        public WeaponStatus Status { get; set; }
        public List<Depth> FiringDepths { get; set; }
        public List<Depth> EffectiveDepths { get; set; }
        public Range Range { get; set; }

        public Weapon()
        {
            FiringDepths = new List<Depth>();
            EffectiveDepths = new List<Depth>();
            Status = WeaponStatus.loaded;
        }

        public int FireWeapon(double _targetProfile)
        {
            Status = WeaponStatus.empty;
            ReloadRoundsLeft = ReloadRounds;

            int Roll = Dice.RollPercentage();

            if (_targetProfile > Roll)
            {
                return Damage.WeightedRandom(10);
            }
            else
            {

                return 0;
            }
        }

        public void Reload()
        {
            if (Status == WeaponStatus.reloading)
            {
                if (ReloadRoundsLeft > 0)
                {
                    ReloadRoundsLeft--;
                    Status = WeaponStatus.reloading;
                }
                else
                {
                    Status = WeaponStatus.loaded;
                }
            }
            if (Status == WeaponStatus.empty)
            {
                ReloadRoundsLeft = ReloadRounds;
                Status = WeaponStatus.reloading;
            }
        }

        public string ListValidFirindDeapths()
        {
            string returnValue = "";
            foreach (var Depth in FiringDepths)
            {
                returnValue += Depth.DepthName;
                
                if (Depth != FiringDepths.Last())
                {
                    if (Depth == FiringDepths[FiringDepths.Count - 2])
                    {
                        returnValue += " or ";
                    }
                    else
                    {
                        returnValue += ", ";
                    }
                    
                }
            }

            return returnValue;
        }
    }



    public enum WeaponType { Main_Battery, Torpedo, Secondary_Battery, Depth_Charge }
    public enum WeaponStatus { firing, reloading, empty, loaded}

}


