using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeerJager.Entities;

namespace MeerJager.Entities
{
    static class CombatMenu
    {
        private static Player player = Player.GetPlayer;

        public static void StartCombat(Enemy enemy)
        {
            while (player.Health > 0 || enemy.Health > 0)
            {
                CombatRound(enemy);
                GetPlayerChoice();
            }
        }

        private static void GetPlayerChoice()
        {
            var menuOptions = new List<MenuOption>();
            var closeDistance = new MenuOption()
            {
                Display = "Close Distance",
                Id = 1
            };
            var openDistance = new MenuOption()
            {
                Display = "Open Distance",
                Id = 2
            };
            menuOptions.Add(closeDistance);
            menuOptions.Add(openDistance);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Combat Options:");
            foreach (var option in menuOptions)
            {
                Console.WriteLine("{0}. {1}", option.Id, option.Display);
            }
            var key = Console.ReadKey().KeyChar;
            
            if (char.IsDigit(key) && menuOptions.Any(x => x.Id.ToString() == key.ToString()))
            {
                var selected = menuOptions.Where(x => x.Id.ToString() == key.ToString()).FirstOrDefault();
                Console.WriteLine();
                Console.WriteLine(selected.Display);
            }
        }



        public static void CombatRound(Enemy enemy)
        {   
            if (!enemy.isEngaged)
            {  
                // if the distance between ships is > 100 auto detect
                if (enemy.DistanceToPlayer < 100)
                {
                    enemy.isEngaged = true;
                }
                else
                {
                    int depthModifier = (int)player.Depth / 100; //Gets the depth modifier as a decimal percent
                    double playerProfileAfterDepth = player.Profile * depthModifier;
                    double baseDetectionChancePerHundredMeters = enemy.DetectionAbility * playerProfileAfterDepth; //players depth profile modified by the enemy detection ability
                    double hundredMetersToPlayer = enemy.DistanceToPlayer / 100;
                    double finalDetectionChance = baseDetectionChancePerHundredMeters * Math.Floor(hundredMetersToPlayer);
                    int roll = Dice.RollPercentage();
                    if (finalDetectionChance < roll)
                    {
                        enemy.isEngaged = true;
                        Console.WriteLine("The Enemy has detected you");
                    }
                    else
                    {
                        Console.WriteLine("The Enemy fails to detect you");
                    }

                }



            }

        }
    }
}
