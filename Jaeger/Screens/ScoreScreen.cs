using MeerJager.Entities;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console = SadConsole.Console;

namespace MeerJager.Screens
{
    class ScoreScreen : ContainerConsole
    {
        public Console ScoreConsole { get; }
        private List<ScoreLine> ClassScorePosition { get; set; } = new List<ScoreLine>();
        private readonly Timer ScoreTimer;
        private readonly Timer TypingTimer;
        private readonly Timer PointsTimer;
        private int progress = 0;
        private int ScoreBoxWidth = Program.Width / 3;

        public ScoreScreen()
        {
            ScoreConsole = new Console(Program.Width, Program.Height);
            ScoreConsole.Parent = this;
            ScoreConsole.Print(1, 1, "Your Combined Score:");
            Global.CurrentScreen = ScoreConsole;
            this.Position = new Point(0, 0);

            DrawScoreGrid();

            TypingTimer = new Timer(TimeSpan.FromSeconds(0.05));
            TypingTimer.Repeat = true;
            ScoreConsole.Components.Add(TypingTimer);

            PointsTimer = new Timer(TimeSpan.FromSeconds(0.1));
            PointsTimer.Repeat = true;
            ScoreConsole.Components.Add(PointsTimer);



            ScoreTimer = new Timer(TimeSpan.FromSeconds(10.0));
            ScoreTimer.Repeat = true;

            ScoreTimer.TimerElapsed += (timer, e) =>
            {
                if (progress < Program.Player.SunkShips.Count())
                {
                    AddScore(Program.Player.SunkShips[progress]);
                }
                progress++;
            };
            ScoreConsole.Components.Add(ScoreTimer);



            ScoreProcessing();
        }

        public void DrawScoreGrid()
        {


            using (var db = new Data.Database.AppDataContext())
            {
                int YPos = 5;
                foreach (var DBClass in db.Class)
                {
                    String Class = DBClass.ClassName;
                    String Number = "0";
                    string Dots = "";
                    int NumberOfDots = ScoreBoxWidth - (Number.Length + Class.Length);
                    for (int i = 0; i < NumberOfDots; i++)
                    {
                        Dots += ".";
                    }
                    string ScoreLine = String.Format("{0}{1}{2}", Class, Dots, Number);
                    int ScoreLineStart = Program.Width - (2 * ScoreBoxWidth);
                    ScoreConsole.Print(ScoreLineStart, YPos, ScoreLine);

                    ClassScorePosition.Add(new ScoreLine { Class = Class, Position = new Point(ScoreLineStart + ScoreBoxWidth, YPos) });
                    YPos++;


                }
            }
        }

        public void AddScore(Enemy Ship)
        {
            ClearPostScore();

            var ScoreLine = ClassScorePosition.Where(x => x.Class == Ship.Class).FirstOrDefault();
            var OldScore = ScoreLine.Score;
            ScoreLine.Score += 100;

            //ScoreConsole.Print(ScoreLine.Position.X - ScoreLine.Score.ToString().Length, ScoreLine.Position.Y, ScoreLine.Score.ToString());
            //coreConsole.Print(ScoreLine.Position.X + 1, ScoreLine.Position.Y, "+" + Ship.UIName, Color.Green);
            IncreaseScore(ScoreLine.Position, OldScore, ScoreLine.Score);
            TypeLine(new Point(ScoreLine.Position.X + 1, ScoreLine.Position.Y), "+" + Ship.UIName);
        }

        public void ClearPostScore()
        {
            foreach (var Line in ClassScorePosition)
            {
                ScoreConsole.DrawLine(new Point(Line.Position.X + 1, Line.Position.Y), new Point(Line.Position.X + ScoreBoxWidth, Line.Position.Y), Color.Black, Color.Black);
            }
        }

        public void ScoreProcessing()
        {
            if (progress < Program.Player.SunkShips.Count())
            {
                AddScore(Program.Player.SunkShips[progress]);
            }
            progress++;
        }

        public void TypeLine(Point StartPosition, string Line)
        {
            int letter = 0;
            

            TypingTimer.TimerElapsed += (timer, e) =>
            {
                if (letter < Line.Length)
                {
                    ScoreConsole.Print(StartPosition.X + letter, StartPosition.Y, Line[letter].ToString(), Color.Green);
                    letter++;
                }
                
                
            };
            
        }

        public void IncreaseScore(Point StartPosition, int OldScore, int NewScore)
        {
            int CurrentScore = OldScore;

            TypingTimer.TimerElapsed += (timer, e) =>
            {
                if (CurrentScore < NewScore)
                {
                    var Score = CurrentScore++;
                    ScoreConsole.Print(StartPosition.X - CurrentScore.ToString().Length , StartPosition.Y, CurrentScore.ToString(), Color.White);
                }
            };
        }

    }

    public class ScoreLine
    {
        public string Class { get; set; }
        public int Score { get; set; } = 0;
        public Point Position { get; set; }
    }
}
