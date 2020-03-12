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
            while (player.Health > 0 && enemy.Health > 0)
            {
                CombatRound(enemy);
                GetPlayerChoice(enemy);
            }
        }

        private static void GetPlayerChoice(Enemy enemy)
        {
            int OptionNumber = 1;
            var menuOptions = new List<MenuOption>();
            var closeDistance = new MenuOption()
            {
                Display = "Close Distance",
                Key = 'C',
                Id = OptionNumber,
            };
            OptionNumber++;
            menuOptions.Add(closeDistance);
            var openDistance = new MenuOption()
            {
                Display = "Open Distance",
                Key = 'O',
                Id = OptionNumber,
            };
            OptionNumber++;
            menuOptions.Add(openDistance);
            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder + 1))
            {
                var raiseDepth = new MenuOption()
                {
                    Display = "Raise Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder + 1).FirstOrDefault().DepthName,
                    Key = 'R',
                    Id = OptionNumber,
                };
                OptionNumber++;
                menuOptions.Add(raiseDepth);
            }

            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder - 1))
            {
                var lowerDepth = new MenuOption()
                {
                    Display = "Lower Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder - 1).FirstOrDefault().DepthName,
                    Key = 'L',
                    Id = OptionNumber,
                };
                OptionNumber++;
                menuOptions.Add(lowerDepth);
            }

            var sitRep = new MenuOption()
            {
                Display = "Situation Report",
                Key = 'S',
                Id = OptionNumber,
            };
            OptionNumber++;
            menuOptions.Add(sitRep);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("What are your orders?");
            foreach (var option in menuOptions.OrderBy(x => x.Id))
            {
                Console.WriteLine("{0}. {1} [{2}]", option.Id, option.Display, option.Key);
            }

            Boolean isValidKey = false;
            do
            {
                Char key = Console.ReadKey().KeyChar;
                if (menuOptions.Any(x => char.ToLower(x.Key) == char.ToLower(key)))
                {
                    isValidKey = true;
                    Console.Clear();
                    Console.WriteLine();
                    switch (char.ToLower(key))
                    {
                        case 'c':
                            enemy.ChangeDistance(-100);
                            break;
                        case 'o':
                            enemy.ChangeDistance(100);
                            break;
                        case 'r':
                            player.RaiseDepth();
                            break;
                        case 'l':
                            player.LowerDepth();
                            break;
                        case 's':
                            SituationReport(enemy);
                            break;
                        case 't':
                            break;
                        default:
                            break;
                    }
                }
            } while (!isValidKey);
        }


        public static void SituationReport(Enemy enemy)
        {
            Console.Clear();
            Console.WriteLine("XO's report:");
            if (enemy.playerCanSee)
            {
                Console.WriteLine("Captain, we have one enemy at {0}km ahead", enemy.DistanceToPlayer / 100);
                if (enemy.isEngaged)
                {
                    Console.WriteLine("They are aware of our presence and are moving to engage");
                }
            }
            Console.WriteLine("We have {0} torpedos in the stores with {1} loaded into tubes", player.Torpedos, player.Armament.Where(x => x.Type == WeaponType.Torpedo && x.Loaded == true).Count());
            Console.WriteLine("Currently we are at {0} depth", player.Depth.DepthName);
            Console.WriteLine(player.GetDamageReport());

            GetPlayerChoice(enemy);
        }

        public static void CombatRound(Enemy enemy)
        {
            if (!enemy.playerCanSee)
            {
                //TODO: Make it so enemy can loose site of you
                PlayerSearching(enemy);
            }
            if (!enemy.isEngaged)
            {
                //TODO: make it so you can loose site of the enemy
                EnemySearching(enemy);
            }
            else
            {
                var loadedGuns = enemy.Armament.Where(x => x.Loaded);
                foreach (var gun in loadedGuns)
                {
                    double profile = player.GetProfile(enemy.DetectionAbility, enemy.DistanceToPlayer);
                    int damage = gun.FireWeapon(profile);
                    player.SetDamage(damage);
                   
                }

                enemy.ReloadGuns();
            }
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
                double profile = player.GetProfile(enemy.DetectionAbility, enemy.DistanceToPlayer);
                int roll = Dice.RollPercentage();
                if (profile > roll)
                {
                    enemy.Spotted();
                    enemy.Engage();
                }
            }
        }
    }
}
