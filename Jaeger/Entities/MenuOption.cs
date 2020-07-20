using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public class MenuOption
    {
        public int Id { get; set; }
        public Microsoft.Xna.Framework.Input.Keys Key { get; set; }
        public string Display { get; set; }
        public Action Action { get; set; }
        public Color Forground { get; set; }
        public Color Background { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Boolean Selected { get; set; }
        public MenuOption()
        {
            Forground = Color.White;
            Background = Color.Black;
        }
    }

    
}
