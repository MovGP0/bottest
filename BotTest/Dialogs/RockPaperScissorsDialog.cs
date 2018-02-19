using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using RockPaperScissors.Models;

namespace RockPaperScissors.Dialogs
{
    [Serializable]
    public class RockPaperScissorsDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = (Activity)await result;

            if (activity.Type != ActivityTypes.Message)
            {
                HandleSystemMessage(activity);
                return;
            }

            var message = await GetMessageAsync(activity);
            
            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        private static async Task<string> GetMessageAsync(Activity activity)
        {
            var state = new GameState();
            var userText = activity.Text.ToLowerInvariant();

            if (userText.Contains("score"))
            {
                return await state.GetScoresAsync(activity);
            }

            if (userText.Contains("delete"))
            {
                return await state.DeleteScoresAsync(activity);
            }
            
            var game = new Game();
            var result = game.Play(activity.Text);

            if (result.Contains("tie"))
            {
                await state.UpdateScoresAsync(activity, GameResult.Tie);
            }
            else if (result.Contains("win"))
            {
                await state.UpdateScoresAsync(activity, GameResult.UserWin);
            }
            else
            {
                await state.UpdateScoresAsync(activity, GameResult.BotWin);
            }

            return result;
        }

        private void HandleSystemMessage(IActivity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.DeleteUserData:
                case ActivityTypes.ConversationUpdate:
                case ActivityTypes.ContactRelationUpdate:
                case ActivityTypes.Typing:
                case ActivityTypes.Ping:
                default:
                    return;
            }
        }
    }
}