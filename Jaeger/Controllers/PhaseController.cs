using System;
using SadConsole;
using System.Collections.Generic;
using System.Text;
using Console = SadConsole.Console;
using MeerJager.Screens;
using Microsoft.Xna.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MeerJager.Entities
{
    static class PhaseController
    {
        private static Enemy Enemy;

        
        public static void StartCombat()
        {    
            Enemy = Enemy.GetRandomEnemy();
            //Console Combat = new Console(Program.Width, Program.Height);


            //LogScreen Log = new LogScreen();
            //Log.Position = new Point((int)((Global.RenderWidth / Global.FontDefault.Size.X) * 0.30), 0);
            //Log.Parent = Combat;
            //Program.CurrentLog = Log;

            //CombatMenuScreen Menu = new CombatMenuScreen(Enemy);
            //Menu.Parent = Combat;

            //new MessageScreen("Captain, acoustics has picked up something. Might be nothing though.", Menu.StartCombat, null, null, null, null);
            for (int i = 0; i < 10; i++)
            {
                Program.Player.SunkShips.Add(Enemy.GetRandomEnemy());
            }

            ScoreScreen();
        }

        public static void EndGame()
        {
            new MessageScreen("You Died", ScoreScreen, null, Color.Crimson, null, Color.Crimson);
        }

        public static void ScoreScreen()
        {
            ScoreScreen Score = new ScoreScreen();
        }
    }
}
