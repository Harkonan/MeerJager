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

            using (StreamReader r = new StreamReader(".\\Data\\Enemy.json"))
            {
                string json = r.ReadToEnd();
                var e = JsonSerializer.Deserialize<EnemyDirectory>(json);
                EnemyType RandomEnemy = (EnemyType)Dice.RandomFromList(e.Frigates);

                Enemy.UIName = RandomEnemy.UIName + " Class " + "Frigate";
                Enemy.Health = RandomEnemy.Health.WeightedRandom(2);
                Enemy.Profile = RandomEnemy.Profile.WeightedRandom(2);
                Enemy.DistanceToPlayer = Dice.RandomBetweenTwo(10000, 20000);

            }

            

            MessageScreen Message = new MessageScreen();
            Message.Message = "Captain, acoustics has picked up something. Might be nothing though.";
            Message.RenderMessage();
            Global.CurrentScreen = Message;
            Global.CurrentScreen.IsFocused = true;


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




    }
}
;