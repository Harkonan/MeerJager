using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeerJager.Entities;

namespace MeerJager
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player = Player.GetPlayer;

            Enemy enemy = new Enemy();

            CombatMenu.StartCombat(enemy);
            Console.ReadKey();
        }
    }
}
