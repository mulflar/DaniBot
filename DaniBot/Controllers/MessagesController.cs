using DaniBot.Dialog;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DaniBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity.Type == ActivityTypes.Message)
            {
                //var response_raw = await Luis.GetResponse(activity.Text);
                //Activity reply = activity.CreateReply(MessageProcesor.IdentificarIntent(response_raw));
                //await connector.Conversations.ReplyToActivityAsync(reply);
                await Conversation.SendAsync(activity, () => new RootLuisDialog());
            }
            else
            {
                var reply = HandleSystemMessage(activity);
                if (reply != null)
                    await connector.Conversations.ReplyToActivityAsync(reply);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                string replyMessage = string.Empty;
                if(DateTime.Now.Hour>6 && DateTime.Now.Hour <14)
                    replyMessage += $"Buenos días\n\n";
                else if(DateTime.Now.Hour > 14 && DateTime.Now.Hour < 20)
                    replyMessage += $"Buenas tardes\n\n";
                else
                    replyMessage += $"Buenas noches\n\n";
                replyMessage += $"Le atiende Informatica Ros, ¿en qué puedo ayudarle?  \n";
                if ((DateTime.Now.Hour > 18 && DateTime.Now.Hour > 0)
                    || (DateTime.Now.Hour < 0 && DateTime.Now.Hour > 8)
                    || (DateTime.Now.Hour < 14 && DateTime.Now.Hour > 16))
                {
                    replyMessage += $"Le recordamos que el horario de nuestros técnicos es\n\n";
                    replyMessage += $"de 8:00 a 14:00 y de 16:00 a 18:30 de Lunes a Jueves\n\n";
                    replyMessage += $"Y los viernes de 8:00 a 14:00.\n\n";
                }
                return message.CreateReply(replyMessage);
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}