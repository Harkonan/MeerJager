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
            Options[0] = new Depth() { DepthName = "Surface", ProfileMultiplier = 1 };
            Options[1] = new Depth() { DepthName = "Periscope", ProfileMultiplier = 0.6 };
            Options[2] = new Depth() { DepthName = "Cruising", ProfileMultiplier = 0.2 };
            Options[3] = new Depth() { DepthName = "Evasion", ProfileMultiplier = 0.01 };
        }

    }

    public class Depth
    {
        public double ProfileMultiplier { get; set; }
        public string DepthName { get; set; }
    }
}
