using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jaeger.Entities;
using MeerJager.Entities;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using SadConsole.Components;
using Microsoft.Xna.Framework.Input;
using MeerJager.Screens;
using MeerJager.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MeerJager
{
    class Program
    {
        private static int Width = 150;
        private static int Height = 50;
        internal static LogScreen CurrentLog;

        private static Enemy Enemy;

        static void Main(string[] args)
        {
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            //Console.ReadKey();

            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            var console = new Console(Width, Height);

            ////Draw Log
            //console.Fill(new Rectangle(0, 0, Width-40, Height), Color.LightSeaGreen, Color.Navy, 0, 0);
            //console.Print(1, 1, "Ships Log");


            ////Draw Orders
            //console.Fill(new Rectangle(Width-40, 0, 40, Height), Color.White, Color.Black, 0, 0);
            //console.Print(Width-39, 1, "Orders?");
            //console.Components.Add(new KBcomp());


            //console.IsVisible = true;
            
            
            Player player = Player.GetPlayer;

            Enemy = new Enemy();

            using (var db = new Data.Database.AppDataContext())
            {

                var Ships = db.Ships
                    .Include(x => x.Class)
                    .Include(x => x.Health)
                    .Include(x => x.Profile)
                    .ToList();
                Data.Database.Ship RandomEnemy = (Data.Database.Ship)Dice.RandomFromList(Ships);
                

                Enemy.UIName = RandomEnemy.Name + " Class " + RandomEnemy.Class.ClassName;
                Enemy.MaxHealth= RandomEnemy.Health.WeightedRandom(2);
                Enemy.CurrentHealth = Enemy.MaxHealth;
                Enemy.Profile = RandomEnemy.Profile.WeightedRandom(2);
                Enemy.DistanceToPlayer = Dice.RandomBetweenTwo(10000, 20000);
            }

            new MessageScreen("Captain, acoustics has picked up something. Might be nothing though.", StartCombat, null, null, null, null);
        }

        public static void StartCombat()
        {
            Console Combat = new Console(Width, Height);


            LogScreen Log = new LogScreen();
            Log.Position = new Point((int)((Global.RenderWidth / Global.FontDefault.Size.X) * 0.30), 0);
            Log.Parent = Combat;
            CurrentLog = Log;

            CombatMenuScreen Menu = new CombatMenuScreen(Enemy);
            Menu.Parent = Combat;
            
            Global.CurrentScreen = Combat;
            Menu.IsFocused = true;
            Menu.StartCombat();
        }

        public static void EndGame()
        {
            SadConsole.Game.Instance.Exit();
        }




    }
}
;