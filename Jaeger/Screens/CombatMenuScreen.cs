using Jaeger.Entities;
using MeerJager.Entities;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Console = SadConsole.Console;

namespace MeerJager.Screens
{
    class CombatMenuScreen : MenuScreen
    {
        private static Player player = Player.GetPlayer;
        private Enemy enemy { get; set; }

        public CombatMenuScreen(Enemy _enemy) : base()
        {
            enemy = _enemy;   
        }

        private void GetPlayerChoice()
        {
            ClearMenu();
            int OptionNumber = 1;

            if (enemy.PlayerCanSee)
            {

                var Weapons = new MenuOption()
                {
                    Display = "Weapons Systems",
                    Key = Microsoft.Xna.Framework.Input.Keys.W,
                    Id = OptionNumber++,
                    Action = GetPlayerWeaponChoice
                };
                AddOption(Weapons);

                var ContinueToDistance = new MenuOption()
                {
                    Display = "Close to torpedo range ",
                    Key = Microsoft.Xna.Framework.Input.Keys.V,
                    Id = OptionNumber++,
                    Forground = Color.Red,
                    Action = () =>
                    {
                        Program.CurrentLog.ClearLog();
                        ContinueOnCourseTillDistance();
                    }
                };
                AddOption(ContinueToDistance);

                var CaptainsInte = new MenuOption()
                {
                    Display = "Check Intelligence Logbook for ship",
                    Key = Microsoft.Xna.Framework.Input.Keys.N,
                    Id = OptionNumber++,
                    Action = () =>
                    {
                        Program.CurrentLog.ClearLog();
                        foreach (var item in enemy.GetCaptainsIntel())
                        {
                            Program.CurrentLog.WriteToLog(item);
                        }
                        GetPlayerChoice();
                    }
                };
                AddOption(CaptainsInte);
            }

            var ContinueOnCourse = new MenuOption()
            {
                Display = "Continue on Course",
                Key = Microsoft.Xna.Framework.Input.Keys.C,
                Id = OptionNumber++,
                Forground = Color.Red,
                Action = () =>
                {
                    Program.CurrentLog.ClearLog();
                    Program.CurrentLog.WriteToLog("Contining on couse, aye Captain");
                    enemy.ChangeDistance(-player.Speed);
                    EndPlayerChoice();
                }
            };
            AddOption(ContinueOnCourse);

            



            var IncreaseSpeed = new MenuOption()
            {
                Display = "Increase Speed",
                Key = Microsoft.Xna.Framework.Input.Keys.I,
                Id = OptionNumber++,
                Action = () =>
                {
                    Program.CurrentLog.ClearLog();
                    player.ChangeSpeed(10);
                }
            };
            AddOption(IncreaseSpeed);

            var DecreaseSpeed = new MenuOption()
            {
                Display = "Decrease Speed",
                Key = Microsoft.Xna.Framework.Input.Keys.D,
                Id = OptionNumber++,
                Action = () =>
                {
                    Program.CurrentLog.ClearLog();
                    player.ChangeSpeed(-10);
                }
            };
            AddOption(DecreaseSpeed);

            var depths = Depths.GetDepths;
            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder + 1))
            {
                var raiseDepth = new MenuOption()
                {
                    Display = "Raise Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder + 1).FirstOrDefault().DepthName,
                    Key = Microsoft.Xna.Framework.Input.Keys.R,
                    Id = OptionNumber++,
                    Forground = Color.Red,
                    Action = () => {
                        Program.CurrentLog.ClearLog();
                        player.RaiseDepth();
                        EndPlayerChoice();
                    }
                };
                AddOption(raiseDepth);
            }

            if (depths.Any(x => x.DepthOrder == player.Depth.DepthOrder - 1))
            {
                AddOption(new MenuOption()
                {
                    Display = "Lower Depth to " + depths.Where(x => x.DepthOrder == player.Depth.DepthOrder - 1).FirstOrDefault().DepthName,
                    Key = Microsoft.Xna.Framework.Input.Keys.L,
                    Id = OptionNumber++,
                    Forground = Color.Red,
                    Action = () => {
                        Program.CurrentLog.ClearLog();
                        player.LowerDepth();
                        EndPlayerChoice();
                    }
                });
            }

            AddOption(new MenuOption()
            {
                Display = "Situation Report",
                Key = Microsoft.Xna.Framework.Input.Keys.S,
                Id = OptionNumber++,
                Action = SituationReport
            });
            RenderMenuOptions();
        }

        private void ContinueOnCourseTillDistance()
        {
            player.DesieredDistance = 7500;
            EndPlayerChoice();
        }

        public void StartCombat()
        {
            Global.CurrentScreen = this.Parent;
            this.IsFocused = true;
            GetPlayerChoice();
        }

        private void EndPlayerChoice()
        {
            ClearMenu();
            CombatRound();
        }

        public void SituationReport()
        {
            ClearMenu();
            Program.CurrentLog.ClearLog();
            Program.CurrentLog.WriteToLog("XO's report:");

            if (enemy.PlayerCanSee)
            {
                Program.CurrentLog.WriteToLog(String.Format("Captain, we have one enemy {0} at {1}m ahead", enemy.UIName, enemy.DistanceToPlayer));
                Program.CurrentLog.WriteToLog("The " + enemy.GetDamageReport());
                if (enemy.CanSeePlayer)
                {
                    Program.CurrentLog.WriteToLog("They are aware of our presence and are moving to engage");
                }
            }
            Program.CurrentLog.WriteToLog(String.Format("We have {0} torpedos in the stores with {1} loaded into tubes", player.Torpedos, player.Armament.Where(x => x.Type == WeaponType.Torpedo && x.Status == WeaponStatus.loaded).Count()));
            Program.CurrentLog.WriteToLog(String.Format("Currently we are at {0} depth", player.Depth.DepthName));
            Program.CurrentLog.WriteToLog(String.Format("Helm is set at {0} speed", player.Speed));
            Program.CurrentLog.WriteToLog(player.GetDamageReport());
            if (player.ActiveTorpedos.Count > 0)
            {
                Program.CurrentLog.WriteToLog(string.Format("We have {0} Torpedos in the water", player.ActiveTorpedos.Count));
                foreach (var ActiveTorpedo in player.ActiveTorpedos)
                {
                    Program.CurrentLog.WriteToLog(string.Format("   {0} is {1}m from the {2}", ActiveTorpedo.UIName, ActiveTorpedo.DistanceToTarget, ActiveTorpedo.Target.UIName));
                }
            }


            GetPlayerChoice();
        }

        private void GetPlayerWeaponChoice()
        {
            ClearMenu();
            Program.CurrentLog.ClearLog();
            int OptionNumber = 1;
            var menuOptions = new List<MenuOption>();
            Program.CurrentLog.WriteToLog("Weapons Report Captain: ");
            Program.CurrentLog.WriteToLog(String.Format("Current Target is {0} at Range {1}", enemy.UIName, enemy.DistanceToPlayer));

            foreach (var weapon in player.Armament)
            {
                weapon.Target = enemy;
                switch (weapon.Status)
                {
                    case WeaponStatus.firing:
                        Program.CurrentLog.WriteToLog(string.Format("{0} is prepairing to fire!", weapon.UIName));

                        var firing = new MenuOption()
                        {
                            Display = string.Format("Belay {0} firing", weapon.UIName),
                            Key = (Microsoft.Xna.Framework.Input.Keys)Convert.ToChar(OptionNumber.ToString()),
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
                        Program.CurrentLog.WriteToLog(string.Format("{0} is loading ({1} turns remaining)", weapon.UIName, weapon.ReloadRoundsLeft));
                        OptionNumber++;
                        break;
                    case WeaponStatus.empty:
                        if (weapon.ReloadRoundsLeft == 0)
                        {
                            Program.CurrentLog.WriteToLog(string.Format("{0} is empty and we have no more ammunition in stores", weapon.UIName));
                        }
                        else
                        {
                            Program.CurrentLog.WriteToLog(string.Format("{0} is empty", weapon.UIName));
                        }

                        OptionNumber++;
                        break;
                    case WeaponStatus.loaded:
                        if (weapon.FiringDepths.Contains(player.Depth) && weapon.Range.Max > weapon.Target.DistanceToPlayer && weapon.Range.Min < weapon.Target.DistanceToPlayer)
                        {
                            Program.CurrentLog.WriteToLog(string.Format("{0} is ready to fire!", weapon.UIName));

                            AddOption(new MenuOption()
                            {
                                Display = "Fire " + weapon.UIName,
                                Key = (Microsoft.Xna.Framework.Input.Keys)Convert.ToChar(OptionNumber.ToString()),
                                Id = OptionNumber++,
                                Action = () =>
                                {
                                    weapon.Status = WeaponStatus.firing;
                                    GetPlayerWeaponChoice();
                                }
                            });
                        }
                        else
                        {
                            if (!weapon.FiringDepths.Contains(player.Depth))
                            {
                                Program.CurrentLog.WriteToLog(string.Format("{1} can only be fired at {0} depth", weapon.ListValidFirindDeapths(), weapon.UIName));
                                OptionNumber++;
                            }
                            else if (weapon.Range.Max < weapon.Target.DistanceToPlayer || weapon.Range.Min > weapon.Target.DistanceToPlayer)
                            {
                                Program.CurrentLog.WriteToLog(string.Format("{0} can only be fired at ranges between {1} and {2}", weapon.UIName, weapon.Range.Min, weapon.Range.Max, weapon.Target.DistanceToPlayer));
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
                AddOption(new MenuOption()
                {
                    Display = "Execute Attack",
                    Key = Microsoft.Xna.Framework.Input.Keys.E,
                    Id = OptionNumber++,
                    Forground = Color.Red,
                    Action = () => {
                        Program.CurrentLog.ClearLog();
                        player.ExecuteAttack();
                        EndPlayerChoice();
                        }
                    ,
                    Selected = true
                });
            }
            else
            {
                AddOption(new MenuOption()
                {
                    Display = "Back",
                    Key = Microsoft.Xna.Framework.Input.Keys.B,
                    Id = OptionNumber++,
                    Action = GetPlayerChoice,
                    Selected = true
                });
            }
            RenderMenuOptions();
        }

        private void CombatRound()
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
            
            if (player.CurrentHealth > 0 && enemy.CurrentHealth > 0)
            {
                if (player.DesieredDistance.HasValue && player.DesieredDistance < enemy.DistanceToPlayer && enemy.PlayerCanSee)
                {
                    Program.CurrentLog.ClearLog();
                    player.ChangeSpeed(player.Depth.MaxSpeedAtDepth);
                    enemy.ChangeDistance(-player.Speed);
                    CombatRound();
                }
                else
                {
                    player.DesieredDistance = null;
                    GetPlayerChoice();
                }
                
            }
            else if(player.CurrentHealth <= 0)
            {
                PhaseController.EndGame();
                
            }
            else
            {
                new MessageScreen("Enemy Destroyed", PhaseController.StartCombat, null, null, null, null);
            }
        }


    }
}
