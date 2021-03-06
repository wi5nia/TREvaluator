﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TREvalBot
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
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                //Activity reply = activity.CreateReply();

                // return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //activity.Attachments.Add(new Attachment()
                //{
                //    ContentUrl = "http://ledgeviewpartners.com/wp-content/uploads/2015/06/Power-BI-10.png",
                //    ContentType = "image/png",
                //    Name = "Chart"
                //});
                //activity.Text = "This is a chart for you";
                //Activity reply = activity.CreateReply("How can I help you?");

                //activity.Attachments.Add(new Attachment()
                //{
                //    ContentUrl = "http://ledgeviewpartners.com/wp-content/uploads/2015/06/Power-BI-10.png",
                //    ContentType = "image/png",
                //    Name = "Power-BI-10.png"
                //});


                Activity reply = activity.CreateReply();
                reply.Text = "TechReady 23 session evaluation";
                reply.Attachments = new List<Attachment>();
                //reply.Attachments.Add(new Attachment()
                //{
                //    ContentUrl = "https://devtw.blob.core.windows.net/images/powerbi.png",
                //    ContentType = "image/png",
                //    Name = "Power-BI-10.png"
                //});

                reply.Attachments.Add(
                    new HeroCard
                    {
                        Title = "Your report",
                        Images = new List<CardImage>
                        {
                            new CardImage
                            {
                                Url = "https://devtw.blob.core.windows.net/images/powerbi.png"
                            }
                        }
                    }.ToAttachment()
                    );

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
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