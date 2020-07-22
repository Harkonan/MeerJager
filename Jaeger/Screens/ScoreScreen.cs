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
        private Timer TypingTimer;
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

            


            //ScoreTimer = new Timer(TimeSpan.FromSeconds(10.0));
            //ScoreTimer.Repeat = true;

            //ScoreTimer.TimerElapsed += (timer, e) =>
            //{
            //    if (progress < Program.Player.SunkShips.Count())
            //    {
            //        AddScore(Program.Player.SunkShips[progress]);
            //    }
            //    progress++;
            //};
            //ScoreConsole.Components.Add(ScoreTimer);



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

            IncreaseScore(ScoreLine.Position, OldScore, ScoreLine.Score, "+ " + Ship.UIName);
        }

        public void ClearPostScore()
        {
            foreach (var Line in ClassScorePosition)
            {
                ScoreConsole.DrawLine(new Point(Line.Position.X + 1, Line.Position.Y), new Point(Line.Position.X + ScoreBoxWidth, Line.Position.Y), Color.Black, Color.Black);
            }
        }

        private Boolean Finished = false;
        public void ScoreProcessing()
        {
            
            TypingTimer = new Timer(TimeSpan.FromSeconds(0.01));
            TypingTimer.Repeat = true;
            ScoreConsole.Components.Add(TypingTimer);

            if (progress < Program.Player.SunkShips.Count())
            {
                AddScore(Program.Player.SunkShips[progress]);
                progress++;
            }
            else if (!Finished)
            {
                Finished = true;
                ClearPostScore();
                var Total = new Point(ClassScorePosition.Last().Position.X, ClassScorePosition.Last().Position.Y+1);
                IncreaseScore(Total, 0, Program.Player.SunkShips.Count() * 100, "Total");
            }
        }

        public void IncreaseScore(Point StartPosition, int OldScore, int NewScore, string Line)
        {
            
            int CurrentScore = OldScore;
            int Letter = 0;

            TypingTimer.TimerElapsed += (timer, e) =>
            {
                var LetterTest = (Letter < Line.Length);
                var Scoretest = (CurrentScore < NewScore);
                if (LetterTest)
                {
                    ScoreConsole.Print(StartPosition.X + 1 + Letter, StartPosition.Y, Line[Letter].ToString(), Color.Green);
                    Letter++;
                }

                if (Scoretest)
                {
                    var Score = CurrentScore++;
                    ScoreConsole.Print(StartPosition.X - CurrentScore.ToString().Length , StartPosition.Y, CurrentScore.ToString(), Color.White);
                }

                if (!Scoretest && !LetterTest)
                {
                    ScoreConsole.Components.Remove(TypingTimer);
                    ScoreProcessing();
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
