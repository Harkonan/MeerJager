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
        
        
        public string UIName { get; set; }
        public Depth Depth { get; set; }
        public int BaseRange { get; set; }
        public int Profile { get; set; } //profile is how easy it is to see the vessel
        
        public double AccousticDetectionAbility { get; set; }
        public int Noise { get; set; } //Noise is the volume of the vessel for detection Purposes ~60 for a submarine, ~90 for a frigate ~250 for a dreadnaught seems balanced at optimal torp range




    }
}
