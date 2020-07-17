using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaeger.Entities
{
    public class Depths
    {
        public static Depths Instance { get; set; }

        public Depth[] Options { get; set; }

        public static Depth[] GetDepths
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new Depths();
                }
                return Instance.Options;
            }
        }

        Depths()
        {
            Options = new Depth[4];
            Options[0] = new Depth() { DepthName = "Surface", ChanceToSeeModifier = 1, ChanceToHearModifer = 1, DepthOrder = 3, MaxSpeedAtDepth = 300 };
            Options[1] = new Depth() { DepthName = "Periscope", ChanceToSeeModifier = 0.6, ChanceToHearModifer = 0.6, DepthOrder = 2, MaxSpeedAtDepth = 100 };
            Options[2] = new Depth() { DepthName = "Cruising", ChanceToSeeModifier = 0.001, ChanceToHearModifer = 0.4, DepthOrder = 1, MaxSpeedAtDepth = 50 };
            Options[3] = new Depth() { DepthName = "Evasion", ChanceToSeeModifier = 0.001, ChanceToHearModifer = 0.2, DepthOrder = 0, MaxSpeedAtDepth = 10 };
        }

    }

    public class Depth
    {
        public double ChanceToSeeModifier { get; set; }
        public double ChanceToHearModifer { get; set; }
        public string DepthName { get; set; }
        public int DepthOrder { get; set; }
        public int MaxSpeedAtDepth { get; set; }
    }

}
