using Microsoft.Bot.Connector;

namespace RockPaperScissors.Models
{
    public static class ActivityExtensions
    {
        public static Activity BuildMessageActivity(this Activity userActivity, string message, string locale = "en-US")
        {
            return new Activity(ActivityTypes.Message)
            {
                From = new ChannelAccount(userActivity.Recipient.Id, userActivity.Recipient.Name),
                Recipient = new ChannelAccount(userActivity.From.Id, userActivity.From.Name),
                Conversation = new ConversationAccount(userActivity.Conversation.IsGroup, userActivity.Conversation.Id, userActivity.Conversation.Name),
                ReplyToId = userActivity.Id,
                Text = message,
                Locale = locale
            };
        }
    }
}