using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace RockPaperScissors.Models
{
    public sealed class GameState
    {
        public async Task<string> GetScoresAsync(Activity activity)
        {
            using (var stateClient = activity.GetStateClient())
            {
                var chatbotState = stateClient.BotState;
                var chatbotData = await chatbotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var scoreQueue = chatbotData.GetProperty<Queue<PlayScore>>("scores");
                if (scoreQueue == null)
                {
                    return "Try typing 'Rock', 'Paper', or 'Scissors' to play first.";
                }

                var plays = scoreQueue.Count;
                var userWins = scoreQueue.Count(q => q.GameResult == GameResult.UserWin);
                var chatbotWins = scoreQueue.Count(q => q.GameResult == GameResult.BotWin);
                var ties = scoreQueue.Count(q => q.GameResult == GameResult.Tie); ;

                return $"Out of the last {plays} contests, you scored {userWins} and Chatbot scored {chatbotWins}. You've also had {ties} ties since playing.";
            }
        }

        public async Task UpdateScoresAsync(Activity activity, GameResult gameResult)
        {
            using (var stateClient = activity.GetStateClient())
            {
                var chatbotState = stateClient.BotState;
                var chatbotData = await chatbotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var scoreQueue = chatbotData.GetProperty<Queue<PlayScore>>("scores") ?? new Queue<PlayScore>();

                if (scoreQueue.Count > 10)
                {
                    scoreQueue.Dequeue();
                }

                scoreQueue.Enqueue(new PlayScore(gameResult));
                chatbotData.SetProperty("scores", scoreQueue);
                await chatbotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, chatbotData);
            }
        }

        public async Task<string> DeleteScoresAsync(Activity activity)
        {
            using (var stateClient = activity.GetStateClient())
            {
                var chatBotState = stateClient.BotState;
                await chatBotState.DeleteStateForUserAsync(activity.ChannelId, activity.From.Id);
                return "All scores deleted.";
            }
        }
    }
}