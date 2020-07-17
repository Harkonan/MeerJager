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
        public char? Key { get; set; }
        public string Display { get; set; }
        public Action Action { get; set; }
        public ConsoleColor Forground { get; set; }
        public ConsoleColor Background { get; set; }

        public MenuOption()
        {
            Forground = ConsoleColor.White;
            Background = ConsoleColor.Black;
        }
    }

    
}
