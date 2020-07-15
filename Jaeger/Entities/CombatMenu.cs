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
        private static Enemy enemy;

        public static void StartCombat(Enemy _enemy)
        {
            enemy = _enemy;
            while (player.Health > 0 && _enemy.Health > 0)
            {
                CombatRound();
                GetPlayerChoice();
            }
            if (player.Health <= 0)
            {
                Console.WriteLine("You Died");
            }
            if (_enemy.Health <= 0)
            {
                Console.WriteLine("Enemy Destroyed");
            }
        }

        private static void GetPlayerChoice()
        {
            int OptionNumber = 1;
            var menuOptions = new List<MenuOption>();
            var closeDistance = new MenuOption()
            {
                Display = "Close Distance",
                Key = 'C',
                Id = OptionNumber++,
                Action = enemy.PlayerCloseDistance
            };
            menuOptions.Add(closeDistance);
            var openDistance = new MenuOption()
            {
                Display = "Open Distance",
                Key = 'O',
                Id = OptionNumber++,
                Action = enemy.PlayerOpenDistance
            };
            menuOptions.Add(openDistance);
            var holdPosition = new MenuOption()
            {
                Display = "Hold Possition",
                Key = 'H',
                Id = OptionNumber++,
                Action = () => { }
            };
            menuOptions.Add(holdPosition);

            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder + 1))
            {
                var raiseDepth = new MenuOption()
                {
                    Display = "Raise Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder + 1).FirstOrDefault().DepthName,
                    Key = 'R',
                    Id = OptionNumber++,
                    Action = player.RaiseDepth
                };
                menuOptions.Add(raiseDepth);
            }

            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder - 1))
            {
                var lowerDepth = new MenuOption()
                {
                    Display = "Lower Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder - 1).FirstOrDefault().DepthName,
                    Key = 'L',
                    Id = OptionNumber++,
                    Action = player.LowerDepth
                };
                menuOptions.Add(lowerDepth);
            }

            var Weapons = new MenuOption()
            {
                Display = "Weapons Systems",
                Key = 'W',
                Id = OptionNumber++,
                Action = GetPlayerWeaponChoice
            };
            menuOptions.Add(Weapons);

            var sitRep = new MenuOption()
            {
                Display = "Situation Report",
                Key = 'S',
                Id = OptionNumber++,
                Action = SituationReport
            };
            menuOptions.Add(sitRep);


            UIScreen.MenuTitle = "What are your orders Captain?";
            UIScreen.Menu = menuOptions;
            UIScreen.RenderScreen();
        }

        public static void GetPlayerWeaponChoice()
        {
            int OptionNumber = 1;
            var menuOptions = new List<MenuOption>();
            UIScreen.DisplayLines.Add("Weapons Report Captain: ");

            foreach (var weapon in player.Armament)
            {
                if (weapon.Type == WeaponType.Torpedo)
                {
                    switch (weapon.Status)
                    {
                        case WeaponStatus.firing:
                            UIScreen.DisplayLines.Add(string.Format("{0} is prepairing to fire!", weapon.UIName));

                            var firing = new MenuOption()
                            {
                                Display = string.Format("Belay {0} firing", weapon.UIName),
                                Key = Convert.ToChar(OptionNumber.ToString()),
                                Id = OptionNumber++,
                                Action = () =>
                                {
                                    weapon.Status = WeaponStatus.loaded;
                                    GetPlayerWeaponChoice();
                                }
                            };
                            menuOptions.Add(firing);
                            break;
                        case WeaponStatus.reloading:
                            UIScreen.DisplayLines.Add(string.Format("{0} is loading ({1} turns remaining)", weapon.UIName, weapon.ReloadRoundsLeft));
                            OptionNumber++;
                            break;
                        case WeaponStatus.empty:
                            UIScreen.DisplayLines.Add(string.Format("{0} is empty", weapon.UIName));
                            OptionNumber++;
                            break;
                        case WeaponStatus.loaded:
                            if (weapon.FiringDepths.Contains(player.Depth))
                            {
                                UIScreen.DisplayLines.Add(string.Format("{0} is ready to fire!", weapon.UIName));

                                var loaded = new MenuOption()
                                {
                                    Display = "Fire " + weapon.UIName,
                                    Key = Convert.ToChar(OptionNumber.ToString()),
                                    Id = OptionNumber++,
                                    Action = () =>
                                    {
                                        weapon.Status = WeaponStatus.firing;
                                        weapon.Target = enemy;
                                        GetPlayerWeaponChoice();
                                    }
                                };
                                menuOptions.Add(loaded);
                            }
                            else {
                                

                                UIScreen.DisplayLines.Add(string.Format("{1} can only be fired at {0} depth", weapon.ListValidFirindDeapths(), weapon.UIName));
                                OptionNumber++;
                            }
                            
                            break;
                        default:
                            break;
                    }



                }
            }

            if (player.Armament.Any(x => x.Status == WeaponStatus.firing))
            {
                var back = new MenuOption()
                {
                    Display = "Execute Attack",
                    Key = 'E',
                    Id = OptionNumber++,
                    Action = player.ExecuteAttack
                };
                menuOptions.Add(back);

            }
            else
            {
                var back = new MenuOption()
                {
                    Display = "Back",
                    Key = 'B',
                    Id = OptionNumber++,
                    Action = GetPlayerChoice
                };
                menuOptions.Add(back);
            }



            UIScreen.MenuTitle = "Weapons Systems";
            UIScreen.Menu = menuOptions;
            UIScreen.RenderScreen();
        }


        public static void SituationReport()
        {
            UIScreen.DisplayLines.Add("XO's report:");

            if (enemy.playerCanSee)
            {

                UIScreen.DisplayLines.Add(String.Format("Captain, we have one enemy {0} at {1}km ahead", enemy.UIName, enemy.DistanceToPlayer / 100));
                UIScreen.DisplayLines.Add("The " + enemy.GetDamageReport());
                if (enemy.isEngaged)
                {
                    UIScreen.DisplayLines.Add("They are aware of our presence and are moving to engage");
                }
            }
            UIScreen.DisplayLines.Add(String.Format("We have {0} torpedos in the stores with {1} loaded into tubes", player.Torpedos, player.Armament.Where(x => x.Type == WeaponType.Torpedo && x.Status == WeaponStatus.loaded).Count()));
            UIScreen.DisplayLines.Add(String.Format("Currently we are at {0} depth", player.Depth.DepthName));
            UIScreen.DisplayLines.Add(player.GetDamageReport());
            if (player.ActiveTorpedos.Count > 0)
            {
                UIScreen.DisplayLines.Add(string.Format("We currently have {0} Torpedos in the water", player.ActiveTorpedos.Count));
                foreach (var ActiveTorpedo in player.ActiveTorpedos)
                {
                    UIScreen.DisplayLines.Add(string.Format("{0} is {1}km from the {2}", ActiveTorpedo.UIName, ActiveTorpedo.DistanceToTarget/100, ActiveTorpedo.Target.UIName));
                }
            }


            GetPlayerChoice();
        }

        public static void CombatRound()
        {
            foreach (var ActiveTorpedo in player.ActiveTorpedos)
            {
                ActiveTorpedo.TorpedoTurn();
            }
            player.ActiveTorpedos.RemoveAll(x => x.Finished);


            if (!enemy.playerCanSee)
            {
                //TODO: Make it so enemy can loose site of you
                PlayerSearching();
            }
            if (!enemy.isEngaged)
            {
                //TODO: make it so you can loose site of the enemy
                EnemySearching();
            }
            else
            {
                enemy.CycleWeapons(player);
            }
            player.ReloadGuns();
        }

        private static void PlayerSearching()
        {
            //TODO: For Daryl
            enemy.Spotted();
        }

        public static void EnemySearching()
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
