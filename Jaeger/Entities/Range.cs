using System;
using System.Collections.Generic;
using System.Text;

namespace MeerJager.Entities
{
    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public Range()
        {

        }

        public Range(int _Min, int _Max)
        {
            Min = _Min;
            Max = _Max;
        }

        public int GetRandom()
        {
            return Dice.RandomBetweenTwo(Min, Max);
        }

        public int WeightedRandom(int Weight)
        {
            return Dice.WeightedRandomBetweenTwo(Min, Max, Weight);
        }
    }
}
