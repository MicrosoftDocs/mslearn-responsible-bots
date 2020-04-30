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
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        readonly string welcomeText = 
                "Hello!\n"+
                "I am a teaching assistant bot that will help you learn **Geography**. I will not be able to teach you, but I can definitely help! Feel free to ask me about different countries and their capitals. If not sure, start with **what can I say?**";
        
        readonly string unknownText = "I am not sure I understand you fully. If you are not sure what to say, ask **what can I say?**";

        LuisRecognizer rec;
        CountryData CData;
        QnAMaker QnA;

        public EchoBot(LuisRecognizer rec,QnAMaker QnA)
        {
            this.rec = rec;
            this.QnA = QnA;
            var f1 = System.IO.Path.Combine(Environment.CurrentDirectory, @"worldcities.csv");
            var f2 = System.IO.Path.Combine(Environment.CurrentDirectory, @"countryflags.csv");
            CData = new CountryData(f1,f2);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var res = await rec.RecognizeAsync(turnContext, cancellationToken);
            var (intent, luis_score) = res.GetTopScoringIntent();
            var ans = await QnA.GetAnswersAsync(turnContext);
            var qna_score = ans == null || ans.Count() == 0 ? 0.0 : ans[0].Score;
            if (luis_score>0.3 && luis_score>qna_score)
            {
                await ProcessLuisResult(turnContext, intent, res.Entities);
            }
            else
            {
                if (ans == null || ans.Count() == 0)
                {
                    await turnContext.SendActivityAsync(unknownText);
                }
                else
                {
                    await turnContext.SendActivityAsync(ans[0].Answer);
                }
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
                await turnContext.SendActivityAsync(unknownText);
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
                    var fl = CData.GetFlag(cou);
                    if (fl != null)
                    {
                        // Use one of the following two options:
                        await turnContext.SendActivityAsync($"{loc} is the capital of {cou}\n![]({fl})");
                        /* await turnContext.SendActivityAsync(
                            MessageFactory.ContentUrl(fl, "image/png", 
                            text: $"{loc} is the capital of {cou}"));
                        */
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(cou == null
                                ? $"I do not know the city named {loc}"
                                : $"{loc} is the capital of {cou}");
                    }
                    break;
                case "get_population":
                    var pop = CData.GetPopulation(loc);
                    await turnContext.SendActivityAsync(pop==null
                            ? $"I do not know the population of {loc}"
                            : $"The population of {loc} is {pop}");
                    break;
                default:
                    await turnContext.SendActivityAsync(unknownText);
                    break;
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(welcomeText);
                }
            }
        }
    }
}
