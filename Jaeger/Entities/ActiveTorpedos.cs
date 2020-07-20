using Jaeger.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Entities
{
    public class ActiveTorpedos
    {
        public string UIName { get; set; }
        public int Damage { get; set; }
        public int HitPercent { get; set; }
        public int Speed { get; set; }
        public int DistanceToTarget { get; set; }
        public Enemy Target { get; set; }
        public bool Finished { get; set; }

        public void TorpedoTurn()
        {
            DistanceToTarget -= Speed;
            if (DistanceToTarget > 0)
            {
                Program.CurrentLog.WriteToLog(string.Format("{0} {1} meters to target", UIName, DistanceToTarget));
            }
            else
            {
                Target.SetDamage(Damage, UIName);
                Finished = true;
                
            }
        }

    }
}
