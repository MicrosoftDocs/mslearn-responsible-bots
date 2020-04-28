// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        LuisRecognizer rec;

        public EchoBot(LuisRecognizer rec)
        {
            this.rec = rec;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var res = await rec.RecognizeAsync(turnContext, cancellationToken);
            await turnContext.SendActivityAsync(res.GetTopScoringIntent().ToString());
            await turnContext.SendActivityAsync(res.Entities.ToString());
        }

        protected string GetCapital(string name)
        {
            var f = System.IO.Path.Combine(Environment.CurrentDirectory, @"worldcities.csv");
            var cd = new CountryData(f);
            return cd.GetCapital(name);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
