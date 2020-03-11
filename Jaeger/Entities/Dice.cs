using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public static class Dice
    {
        private static readonly Random R = new Random();

        public static int RollPercentage()
        {
            return R.Next(0, 100);
        }
    }
}
