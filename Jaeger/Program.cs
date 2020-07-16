using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jaeger.Entities;
using MeerJager.Entities;

namespace MeerJager
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player = Player.GetPlayer;

            Enemy enemy = new Enemy();

            using (StreamReader r = new StreamReader(".\\Data\\Enemy.json")) 
            {
                string json = r.ReadToEnd();
                var e = JsonSerializer.Deserialize<EnemyDirectory>(json);
                EnemyType RandomEnemy = (EnemyType)Dice.RandomFromList(e.Frigates);

                enemy.UIName = RandomEnemy.UIName +" Class "+ "Frigate";
                enemy.Health = RandomEnemy.Health.WeightedRandom(2);
                enemy.Profile = RandomEnemy.Profile.WeightedRandom(2);
                enemy.DistanceToPlayer = Dice.RandomBetweenTwo(10000, 20000);

            }

            CombatMenu.StartCombat(enemy);
            Console.ReadKey();
        }
    }
}
