---
page_type: sample
languages:
- csharp
products:
- dotnet
- azure-bot-service
description: "This sample shows how to create a simple Responsible Educational Assistant bot using Azure Bot Service, LUIS, and Q&A Maker"
urlFragment: "learn-responsible-bots"
---

# Creating Responsible Educational Assistant Bot

<!-- 
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

This sample shows how to create simple Educational Assistant bot in the field of Geography. This bot tries
to follow [Responsible AI Guidelines][Guidelines10].

This repository shows step-by-step process for creating a bot. Different stages are represented by GitHub
commit history. Here is a short guideline:

 * [Country Capital Dictionary]()
 * [LUIS Bot to answer capital questions]()
 * [Integration of Q&A Maker]()
 * [Final Version]()

## Contents


| File/folder       | Description                                |
|-------------------|--------------------------------------------|
| `src`             | Sample source code.                        |
| `.gitignore`      | Define what to ignore at commit time.      |
| `CHANGELOG.md`    | List of changes to the sample.             |
| `CONTRIBUTING.md` | Guidelines for contributing to the sample. |
| `README.md`       | This README file.                          |
| `LICENSE`         | The license for the sample.                |

## Prerequisites

In this sample, we will be using [Azure Bot Service][BotService] and [C# Programming Language][CSharp] to build a bot. Thus, you will need the following:
* Basic knowledge of C#. You may want to [take this course][CSCourse] if you are not familiar with the language
* Azure Account. [This module][AzAccount] describes getting one.
* We suggest using [Visual Studio 2019][VS] as source code editor. Please, download and install Visual Studio on your computer, selecting "Azure Development". If your computer is not powerful enough, or you do not want to perform the installation - you should be able to use **Visual Studio Online**, though it is not recommended.

## Setup

To roll out the sample, do the following:
* Create new [Azure Bot Service][BotService] through Azure Portal
* Download the Bot Source Code and unzip it to some temporary directory (`c:\temp\`)
* Clone this repository to some working directory (eg. `c:\work`)
* Copy `appsettings.json` file with your App Id and App Secret values from temporary directory to the `src` subdirectory in the working directory.
* You should now be able to check out different stages of bot development from different checkpoints in commit history,
  and at each stage run the bot code locally in the bot emulator, or deploy it to Azure.

> **Important:** This lab also involves some manual operations like setting up LUIS model in the cloud, or training Q&A Maker model. Those steps are described in the corresponding Microsoft Learn course. The final bot code might not run without those steps.

## Key concepts

This sample will make us familiar with the following:
* Concepts of Responsible Conversational UI
* Using [Language Understanding Intelligent Service][LUIS] to perform intent classification and entity extraction from natural text
* Using [Q&A NMaker][QAMaker] to set up simple question answering logic

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.


[Guidelines10]: https://www.microsoft.com/research/publication/responsible-bots/
[BotService]: https://azure.microsoft.com/services/bot-service/
[CSharp]: https://dotnet.microsoft.com/learn/csharp
[CSCourse]: https://docs.microsoft.com/dotnet/csharp/tutorials/
[AzAccount]: https://docs.microsoft.com/learn/modules/create-an-azure-account/
[VS]: https://www.visualstudio.com/downloads/
[LUIS]: https://docs.microsoft.com/azure/cognitive-services/luis/
[QAMaker]: https://docs.microsoft.com/azure/cognitive-services/qnamaker/