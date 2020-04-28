// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        LuisRecognizer rec;
        CountryData CData;

        public EchoBot(LuisRecognizer rec)
        {
            this.rec = rec;
            var f = System.IO.Path.Combine(Environment.CurrentDirectory, @"worldcities.csv");
            CData = new CountryData(f);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var res = await rec.RecognizeAsync(turnContext, cancellationToken);
            var (intent, score) = res.GetTopScoringIntent();
            if (score>0.3)
            {
                await ProcessLuisResult(turnContext, intent, res.Entities);
            }
            else
            {
                await turnContext.SendActivityAsync("I am not sure I understand you fully");
            }
        }

        protected async Task ProcessLuisResult(ITurnContext<IMessageActivity> turnContext, string intent, JObject entities)
        {
            if (intent=="play_game")
            {
                await turnContext.SendActivityAsync("This feature is not yet implemented");
                return;
            }
            var geo = entities["geographyV2"];
            if (geo==null || geo.Count()!=1)
            {
                await turnContext.SendActivityAsync("I am not sure which location you mean");
                return;
            }
            var loc = geo[0]["location"].ToString();
            switch (intent)
            {
                case "get_capital":
                    var cap = CData.GetCapital(loc);
                    await turnContext.SendActivityAsync(cap==null
                            ? $"I do not know the capital of {loc}"
                            : $"The capital of {loc} is {cap}");
                    break;
                case "get_country":
                    var cou = CData.GetCountry(loc);
                    await turnContext.SendActivityAsync(cou == null
                            ? $"I do not know the city named {loc}"
                            : $"{loc} is the capital of {cou}");
                    break;
                case "get_population":
                    var pop = CData.GetPopulation(loc);
                    await turnContext.SendActivityAsync(pop==null
                            ? $"I do not know the population of {loc}"
                            : $"The population of {loc} is {pop}");
                    break;
                default:
                    await turnContext.SendActivityAsync("I do not know that one");
                    break;
            }
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
