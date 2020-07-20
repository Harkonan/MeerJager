using System;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Entities
{
    public class AquisitionBonus
    {
        public double CurrentAquisitionBonus { get; set; }
        public double NextAquisitionBonus { get; set; }

        public AquisitionBonus()
        {
            CurrentAquisitionBonus = 1;
            NextAquisitionBonus = 0.5;
        }

        public void AddAquisitionBonus()
        {
            CurrentAquisitionBonus += NextAquisitionBonus;
            NextAquisitionBonus /= 2;
        }
    
    }
}
