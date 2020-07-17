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
            NextAquisitionBonus = 0.2;
        }

        public void AddAquisitionBonus()
        {
            CurrentAquisitionBonus += NextAquisitionBonus;
            if (CurrentAquisitionBonus > 1)
            {
                CurrentAquisitionBonus = 1;
            }
            NextAquisitionBonus /= 2;
        }
    
    }
}
