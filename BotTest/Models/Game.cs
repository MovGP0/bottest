using System;
using System.Collections.Generic;

namespace RockPaperScissors.Models
{
    public class Game
    {
        private readonly IDictionary<PlayType, string> _rockPlays = new Dictionary<PlayType, string>
        {
            [PlayType.Paper] = "Paper covers rock - You lose!",
            [PlayType.Scissors] = "Rock crushes scissors - You win!"
        };

        private readonly IDictionary<PlayType, string> _paperPlays = new Dictionary<PlayType, string>
        {
            [PlayType.Rock] = "Paper covers rock - You win!",
            [PlayType.Scissors] = "Scissors cuts paper - You lose!"
        };

        private readonly IDictionary<PlayType, string> _scissorsPlays = new Dictionary<PlayType, string>
        {
            [PlayType.Rock] = "Rock crushes scissors - You lose!",
            [PlayType.Paper] = "Scissors cut paper - You win!"
        };

        public string Play(string userText)
        {
            var isValidPlay = Enum.TryParse(userText, true, out PlayType userPlay);

            return isValidPlay
                ? Compare(userPlay, GetBotPlay())
                : "Type \"Rock\", \"Paper\", or \"Scissors\" to play.";
        }

        public PlayType GetBotPlay()
        {
            var seed = DateTime.Now.Ticks;
            var rnd = new Random(unchecked((int)seed));
            var position = rnd.Next(3);
            return (PlayType)position;
        }

        public string Compare(PlayType userPlay, PlayType botPlay)
        {
            var plays = $" You: {userPlay}, Bot: {botPlay}";
            var message = GetMessage(userPlay, botPlay);
            return $"{plays}. {message}";
        }

        private string GetMessage(PlayType userPlay, PlayType botPlay)
        {
            if (userPlay == botPlay)
            {
                return "Tie";
            }

            switch (userPlay)
            {
                case PlayType.Rock:
                    return _rockPlays[botPlay];
                case PlayType.Paper:
                    return _paperPlays[botPlay];
                case PlayType.Scissors:
                    return _scissorsPlays[botPlay];
                default:
                    throw new ArgumentOutOfRangeException(nameof(userPlay), userPlay, null);
            }
        }
    }
}