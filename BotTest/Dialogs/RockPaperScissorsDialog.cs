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

        private static async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = (Activity)await result;
            
            var game = new Game();
            var message = game.Play(activity.Text);
            await context.PostAsync(message);

            context.Wait(MessageReceivedAsync);
        }
    }
}