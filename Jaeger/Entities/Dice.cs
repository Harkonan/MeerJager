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

        public static object RandomFromList<T>(List<T> list)
        {
            return list[R.Next(0, list.Count() - 1)];
        }

        public static int RandomBetweenTwo(int min, int max)
        {
            return R.Next(min, max);
        }
    }
}
