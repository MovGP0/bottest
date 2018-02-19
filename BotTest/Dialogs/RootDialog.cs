using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace RockPaperScissors.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private static async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            if(!(await result is Activity activity)) throw new InvalidOperationException("activity was null");

            // calculate something for us to return
            var length = (activity.Text ?? string.Empty).Length;

            var reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters", "en-US");

            // return our reply to the user
            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }
    }
}