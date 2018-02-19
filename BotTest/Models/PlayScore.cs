using System;
using Newtonsoft.Json;

namespace RockPaperScissors.Models
{
    public sealed class PlayScore
    {
        public PlayScore(GameResult gameResult)
        {
            Date = DateTime.Now;
            GameResult = gameResult;
        }

        [JsonConstructor]
        public PlayScore(GameResult gameResult, DateTime date)
        {
            Date = date;
            GameResult = gameResult;
        }

        public DateTime Date { get; }
        public GameResult GameResult { get; }
    }
}