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
            Enemy = new Enemy();

            using (var db = new Data.Database.AppDataContext())
            {

                var Ships = db.Ships
                    .Include(x => x.Class)
                    .Include(x => x.Health)
                    .Include(x => x.Profile)
                    .Include(x => x.WeaponMounts).ThenInclude(m => m.PossibleWeapons).ThenInclude(w => w.Weapon).ThenInclude(w => w.Damage)
                    .Include(x => x.WeaponMounts).ThenInclude(m => m.PossibleWeapons).ThenInclude(w => w.Weapon).ThenInclude(w => w.Range)
                    .ToList();

                Data.Database.Ship RandomEnemy = (Data.Database.Ship)Dice.RandomFromList(Ships);


                Enemy.UIName = RandomEnemy.Name + " Class " + RandomEnemy.Class.ClassName;
                Enemy.MaxHealth = RandomEnemy.Health.WeightedRandom(2);
                Enemy.CurrentHealth = Enemy.MaxHealth;
                Enemy.Profile = RandomEnemy.Profile.WeightedRandom(2);
                Enemy.DistanceToPlayer = Dice.RandomBetweenTwo(10000, 20000);
                Enemy.DBID = RandomEnemy.ShipID;
                
                foreach (Data.Database.WeaponMount Mount in RandomEnemy.WeaponMounts)
                {
                    var PossibleWeapons = Mount.PossibleWeapons.Select(x => x.Weapon).ToList();
                    var DBWeapon = PossibleWeapons[Dice.RandomBetweenTwo(0, PossibleWeapons.Count())];
                    Weapon W = new Weapon(DBWeapon, Mount);
                    Enemy.Armament.Add(W);
                }

            }

            Console Combat = new Console(Program.Width, Program.Height);


            LogScreen Log = new LogScreen();
            Log.Position = new Point((int)((Global.RenderWidth / Global.FontDefault.Size.X) * 0.30), 0);
            Log.Parent = Combat;
            Program.CurrentLog = Log;

            CombatMenuScreen Menu = new CombatMenuScreen(Enemy);
            Menu.Parent = Combat;

            new MessageScreen("Captain, acoustics has picked up something. Might be nothing though.", Menu.StartCombat, null, null, null, null);
        }

        public static void EndGame()
        {
            new MessageScreen("You Died", SadConsole.Game.Instance.Exit, null, Color.Crimson, null, Color.Crimson);
        }
    }
}
