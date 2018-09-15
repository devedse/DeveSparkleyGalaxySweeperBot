using System;
using System.Collections.Generic;
using System.Text;

namespace DeveSparkleyGalaxySweeperBot.Models
{
    public class LastMove
    {
        public int row { get; set; }
        public int column { get; set; }
        public string value { get; set; }
        public string player { get; set; }
    }

    public class Opponent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string challengeToken { get; set; }
    }

    public class GalaxySweeperGame
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime lastMoveExecuted { get; set; }
        public List<LastMove> lastMoves { get; set; }
        public Opponent opponent { get; set; }
        public List<string> field { get; set; }
        public string gridType { get; set; }
        public string gridVariant { get; set; }
        public int myFlags { get; set; }
        public int opponentFlags { get; set; }
        public int totalMines { get; set; }
        public bool myTurn { get; set; }
        public bool isFinished { get; set; }
        public object isWinner { get; set; }
    }
}
