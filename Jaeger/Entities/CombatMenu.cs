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
        }

        private static void GetPlayerChoice()
        {
            int OptionNumber = 1;
            var menuOptions = new List<MenuOption>();
            var closeDistance = new MenuOption()
            {
                Display = "Close Distance",
                Key = 'C',
                Id = OptionNumber,
                Action = enemy.PlayerCloseDistance
            };
            OptionNumber++;
            menuOptions.Add(closeDistance);
            var openDistance = new MenuOption()
            {
                Display = "Open Distance",
                Key = 'O',
                Id = OptionNumber,
                Action = enemy.PlayerOpenDistance
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
                    Action = player.RaiseDepth
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
                    Action = player.LowerDepth
                };
                OptionNumber++;
                menuOptions.Add(lowerDepth);
            }

            var Weapons = new MenuOption(){
                Display = "Weapons Systems",
                Key = 'W',
                Id = OptionNumber,
                Action = GetPlayerWeaponChoice
            };
            OptionNumber++;
            menuOptions.Add(Weapons);

            var sitRep = new MenuOption()
            {
                Display = "Situation Report",
                Key = 'S',
                Id = OptionNumber,
                Action = SituationReport
            };
            OptionNumber++;
            menuOptions.Add(sitRep);


            UIScreen.MenuTitle = "What are your orders Captain?";
            UIScreen.Menu = menuOptions;
            UIScreen.RenderScreen();
        }

        public static void GetPlayerWeaponChoice(){
            int OptionNumber = 1;
            var menuOptions = new List<MenuOption>();

            foreach (var weapon in player.Armament)
	        {
                if (weapon.Type == WeaponType.Torpedo)
	            {
                    var torp = new MenuOption(){
                    Display = weapon.UIName,
                    Key = '1',
                    Id = OptionNumber,
                    Action = GetPlayerChoice
                    };
	            }
	        }

            var back = new MenuOption(){
                Display= "Back",
                Key = 'B',
                Id = OptionNumber,
                Action = GetPlayerChoice
            };
            OptionNumber++;
            menuOptions.Add(back);

            UIScreen.MenuTitle = "Weapons Systems";
            UIScreen.Menu = menuOptions;
            UIScreen.RenderScreen();
        }


        public static void SituationReport()
        {
            UIScreen.DisplayLines.Add("XO's report:");
            
            if (enemy.playerCanSee)
            {

                UIScreen.DisplayLines.Add(String.Format("Captain, we have one enemy at {0}km ahead", enemy.DistanceToPlayer / 100));
                if (enemy.isEngaged)
                {
                    UIScreen.DisplayLines.Add("They are aware of our presence and are moving to engage");
                }
            }
            UIScreen.DisplayLines.Add(String.Format("We have {0} torpedos in the stores with {1} loaded into tubes", player.Torpedos, player.Armament.Where(x => x.Type == WeaponType.Torpedo && x.Loaded == true).Count()));
            UIScreen.DisplayLines.Add(String.Format("Currently we are at {0} depth", player.Depth.DepthName));
            UIScreen.DisplayLines.Add(player.GetDamageReport());

            GetPlayerChoice();
        }

        public static void CombatRound()
        {
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
