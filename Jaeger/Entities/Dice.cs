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
            return list[R.Next(0, list.Count())];
        }

        public static int RandomBetweenTwo(int min, int max)
        {
            return R.Next(min, max);
        }

        public static int UpToMax(int Max)
        {
            return R.Next(Max);
        }

        public static int WeightedRandomBetweenTwo(int Min, int Max, int Weight)
        {
            var result = WeightedRandomUpToMax(Max - Min, Weight);
            return result + Min;
        }

        public static int WeightedRandomUpToMax(int Max, int Weight)
        {
            int result = 0;
            for (int i = 0; i < Weight; i++)
            {
                result += Dice.UpToMax(Max) / Weight;
            }
            return result;
        }
    }
}
