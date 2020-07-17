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
            UIScreen.DisplayLines.Add("Captain, acoustics has picked up something. Might be nothing though.");
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
            if (enemy.PlayerCanSee)
            {
                var Weapons = new MenuOption()
                {
                    Display = "Weapons Systems",
                    Key = 'W',
                    Id = OptionNumber++,
                    Action = GetPlayerWeaponChoice
                };
                menuOptions.Add(Weapons);
            }

            var ContinueOnCourse = new MenuOption()
            {
                Display = "Continue on Course",
                Key = 'C',
                Id = OptionNumber++,
                Action = () =>
                {
                    enemy.ChangeDistance(-player.Speed);
                }
            };
            menuOptions.Add(ContinueOnCourse);

            var IncreaseSpeed = new MenuOption()
            {
                Display = "Increase Speed",
                Key = 'I',
                Id = OptionNumber++,
                Action = () =>
                {
                    player.ChangeSpeed(10);
                    GetPlayerChoice();
                }
            };
            menuOptions.Add(IncreaseSpeed);

            var DecreaseSpeed = new MenuOption()
            {
                Display = "Decrease Speed",
                Key = 'D',
                Id = OptionNumber++,
                Action = () =>
                {
                    player.ChangeSpeed(-10);
                    GetPlayerChoice();
                }
            };
            menuOptions.Add(DecreaseSpeed);

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
            UIScreen.DisplayLines.Add(String.Format("Current Target is {0} at Range {1}", enemy.UIName, enemy.DistanceToPlayer));

            foreach (var weapon in player.Armament)
            {
                weapon.Target = enemy;
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
                        if (weapon.ReloadRoundsLeft == 0)
                        {
                            UIScreen.DisplayLines.Add(string.Format("{0} is empty and we have no more ammunition in stores", weapon.UIName));
                        }
                        else
                        {
                            UIScreen.DisplayLines.Add(string.Format("{0} is empty", weapon.UIName));
                        }
                        
                        OptionNumber++;
                        break;
                    case WeaponStatus.loaded:
                        if (weapon.FiringDepths.Contains(player.Depth) && weapon.Range.Max > weapon.Target.DistanceToPlayer && weapon.Range.Min < weapon.Target.DistanceToPlayer)
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
                                    GetPlayerWeaponChoice();
                                }
                            };
                            menuOptions.Add(loaded);
                        }
                        else
                        {
                            if (!weapon.FiringDepths.Contains(player.Depth))
                            {
                                UIScreen.DisplayLines.Add(string.Format("{1} can only be fired at {0} depth", weapon.ListValidFirindDeapths(), weapon.UIName));
                                OptionNumber++;
                            }
                            else if(weapon.Range.Max < weapon.Target.DistanceToPlayer || weapon.Range.Min > weapon.Target.DistanceToPlayer)
                            {
                                UIScreen.DisplayLines.Add(string.Format("{0} can only be fired at ranges between {1} and {2}",weapon.UIName, weapon.Range.Min, weapon.Range.Max, weapon.Target.DistanceToPlayer));
                                OptionNumber++;
                            }

                            
                        }

                        break;
                    default:
                        break;
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

            if (enemy.PlayerCanSee)
            {
                UIScreen.DisplayLines.Add(String.Format("Captain, we have one enemy {0} at {1}m ahead", enemy.UIName, enemy.DistanceToPlayer));
                UIScreen.DisplayLines.Add("The " + enemy.GetDamageReport());
                if (enemy.CanSeePlayer)
                {
                    UIScreen.DisplayLines.Add("They are aware of our presence and are moving to engage");
                }
            }
            UIScreen.DisplayLines.Add(String.Format("We have {0} torpedos in the stores with {1} loaded into tubes", player.Torpedos, player.Armament.Where(x => x.Type == WeaponType.Torpedo && x.Status == WeaponStatus.loaded).Count()));
            UIScreen.DisplayLines.Add(String.Format("Currently we are at {0} depth", player.Depth.DepthName));
            UIScreen.DisplayLines.Add(String.Format("Helm is set at {0} speed", player.Speed));
            UIScreen.DisplayLines.Add(player.GetDamageReport());
            if (player.ActiveTorpedos.Count > 0)
            {
                UIScreen.DisplayLines.Add(string.Format("We have {0} Torpedos in the water", player.ActiveTorpedos.Count));
                foreach (var ActiveTorpedo in player.ActiveTorpedos)
                {
                    UIScreen.DisplayLines.Add(string.Format("   {0} is {1}m from the {2}", ActiveTorpedo.UIName, ActiveTorpedo.DistanceToTarget, ActiveTorpedo.Target.UIName));
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


            enemy.CheckDetection(player);
            if (enemy.CanSeePlayer)
            {
                enemy.CycleWeapons(player);
            }
            player.ReloadWeapons();
        }

        

    }
}
