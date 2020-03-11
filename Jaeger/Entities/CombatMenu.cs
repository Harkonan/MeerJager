using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaeger.Entities;
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
            menuOptions.Add(closeDistance);
            var openDistance = new MenuOption()
            {
                Display = "Open Distance",
                Id = 2,
            };
            menuOptions.Add(openDistance);
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder+1))
            {
                var raiseDepth = new MenuOption()
                {
                    Display = "Raise Depth to "+ depths.Where(x => x.DepthOrder == player.Depth.DepthOrder + 1).FirstOrDefault().DepthName,
                    Id = 3,
                };
                menuOptions.Add(raiseDepth);
            }

            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder - 1))
            {
                var lowerDepth = new MenuOption()
                {
                    Display = "Lower Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder - 1).FirstOrDefault().DepthName,
                    Id = 4,
                };
                menuOptions.Add(lowerDepth);
            }

            var sitRep = new MenuOption()
            {
                Display = "Situation Report",
                Id = 5,
            };
            menuOptions.Add(sitRep);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Combat Options:");
            foreach (var option in menuOptions.OrderBy(x => x.Id))
            {
                Console.WriteLine("{0}. {1}", option.Id, option.Display);
            }
            var key = Console.ReadKey().KeyChar;
            Console.Clear();
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
                    case 3:
                        player.RaiseDepth();
                        break;
                    case 4:
                        player.LowerDepth();
                        break;
                    case 5:
                        SituationReport(enemy);
                        break;
                    default:
                        break;
                }
            }
        }


        public static void SituationReport(Enemy enemy)
        {
            Console.Clear();
            Console.WriteLine("XO's report:");
            if (enemy.playerCanSee)
            {
                Console.WriteLine("Captain, we have one enemy at {0}km ahead", enemy.DistanceToPlayer/100);
                if (enemy.isEngaged)
                {
                    Console.WriteLine("They are aware of our presence and are moving to engage");
                }
            }
            Console.WriteLine("We have {0} torpedos in the stores with {1} loaded into tubes", player.Torpedos, 0);
            Console.WriteLine("Currently we are at {0} depth", player.Depth.DepthName);
            if (0 < player.Health && player.Health >= 20)
            {
                Console.WriteLine("Our hull is critically compormised");
            }
            else if (20 < player.Health && player.Health >= 60)
            {
                Console.WriteLine("Our hull is heavily damaged");
            }
            else if (20 < player.Health && player.Health >= 60)
            {
                Console.WriteLine("Our hull is damaged");
            }
            else if (60 < player.Health && player.Health >= 99)
            {
                Console.WriteLine("Our hull is slightly damaged");
            }
            else
            {
                Console.WriteLine("Our hull untouched");
            }

            Console.WriteLine("What are your orders?");
            GetPlayerChoice(enemy);
        }

        public static void CombatRound(Enemy enemy)
        {

            Console.WriteLine("Round Start");
            Console.WriteLine();
            if (!enemy.isEngaged)
            {
                EnemySearching(enemy); 
            }
            if (!enemy.playerCanSee)
            {
                PlayerSearching(enemy);
            }
            Console.WriteLine();
            Console.WriteLine("Round End");

        }

        private static void PlayerSearching(Enemy enemy)
        {
            //TODO: For Daryl
            enemy.Spotted();
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
