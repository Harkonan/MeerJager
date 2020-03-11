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
                GetPlayerChoice(enemy);
            }
        }

        private static void GetPlayerChoice(Enemy enemy)
        {
            var menuOptions = new List<MenuOption>();
            var closeDistance = new MenuOption()
            {
                Display = "Close Distance",
                Id = 1,
            };

            var openDistance = new MenuOption()
            {
                Display = "Open Distance",
                Id = 2,
            };
            var raiseDepth = new MenuOption()
            {
                Display = "Raise Depth",
                Id = 2,
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
                switch (selected.Id)
                {
                    case 1:
                        enemy.ChangeDistance(-100);
                        break;
                    case 2:
                        enemy.ChangeDistance(100);
                        break;
                    default:
                        break;
                }
            }
        }



        public static void CombatRound(Enemy enemy)
        {

            Console.WriteLine("Round Start");
            if (!enemy.isEngaged)
            {
                EnemySearching(enemy); 
            }
            if (!enemy.playerCanSee)
            {
                PlayerSearching(enemy);
            }

            Console.WriteLine("Round End");

        }

        private static void PlayerSearching(Enemy enemy)
        {
            //TODO: For Daryl
        }

        public static void EnemySearching(Enemy enemy)
        {
            // if the distance between ships is > 100 auto detect
            if (enemy.DistanceToPlayer < 100)
            {
                enemy.Engage();
            }
            else
            {
                double playerProfileAfterDepth = player.Profile * player.Depth.ProfileMultiplier;
                double baseDetectionChancePerHundredMeters = enemy.DetectionAbility * playerProfileAfterDepth; //players depth profile modified by the enemy detection ability
                double hundredMetersToPlayer = enemy.DistanceToPlayer / 100;
                double finalDetectionChance = baseDetectionChancePerHundredMeters * Math.Floor(hundredMetersToPlayer);
                int roll = Dice.RollPercentage();

                Console.WriteLine("DetectionChance: " + finalDetectionChance);
                Console.WriteLine("Roll: " + roll);
                if (finalDetectionChance > roll)
                {
                    enemy.Engage();
                }
                else
                {
                    Console.WriteLine("The Enemy fails to detect you");
                }

            }
        }
    }
}
