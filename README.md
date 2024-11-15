[![Logo](static/TrueVote_Logo_Text_on_Black.png)](https://truevote.org)

[![Twitter](https://img.shields.io/twitter/follow/TrueVoteOrg?style=social)](https://twitter.com/TrueVoteOrg)
[![Keybase Chat](https://img.shields.io/badge/chat-on%20keybase-7793d8)](https://keybase.io/team/truevote)

[![TrueVote.Bot](https://github.com/TrueVote/TrueVote.Bot/actions/workflows/truevote-bot-github.yml/badge.svg)](https://github.com/TrueVote/TrueVote.Bot/actions/workflows/truevote-bot-github.yml)
[![CodeQL](https://github.com/TrueVote/TrueVote.Bot/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/TrueVote/TrueVote.Bot/actions/workflows/github-code-scanning/codeql)

# TrueVote.Bot

## 🌈 Overview

TrueVote.Bot is an implementation of interacting with the TrueVote Voting Suite of Applications via bots. It's deployed as an Azure Functions project.

The main technology stack platform is [.NET Core](https://dotnet.microsoft.com/) 8.0.

## 🛠 Prerequisites

* Install Visual Studio 2022 (preview) or later, or Visual Studio Code. Ensure that `$ dotnet --version` is at least 8.0.

## ⌨️ Install, Build, and Run the Bot

[Telegram](https://telegram.org/) is the first bot implemented. Additional Bot frameworks will be added in the future.

Create a new file at the root of the TrueVote.Bot project named `local.settings.json`. To use the functions of the Telegram Bot locally, create a new Bot using ['BotFather'](https://core.telegram.org/bots#3-how-do-i-create-a-bot) and place the key in the `local.settings.json` file.

Get the `ServiceBusConnectionString` from Azure portal. Currently Service Bus is not available to run locally.

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "BaseApiUrl": "https://localhost:7253/api",
    "TelegramBotKey": "<TelegramBotKey>",
    "TelegramRuntimeChannel": "TrueVote_Api_Runtime_Channel_Dev",
    "ServiceBusConnectionString": "<ServiceBusConnectionString>",
    "ServiceBusApiEventQueueName": "apieventqueue-dev"
  }
}
```

### Install the packages

```bash
$ dotnet restore
$ dotnet tool restore
```
Open TrueVote.Bot.sln solution in Visual Studio, and build the solution.

## 🎛️ Refreshing the models from TrueVote.Api

TrueVote.Bot makes REST calls to [TrueVote.Api](https://github.com/TrueVote/TrueVote.Api/) and uses C# models via the OpenAPI spec from the TrueVote.Api schema.

To refresh the models, use `nswag`.

Nswag is installed in this project as a `dotnet tool`.

Local: `$ dotnet nswag swagger2csclient /client-language:csharp /input:https://localhost:7253/swagger/v1/swagger.json /output:TrueVote.Api.cs /namespace:TrueVote.Api`

Production: `$ dotnet nswag swagger2csclient /client-language:csharp /input:https://api.truevote.org/swagger/v1/swagger.json /output:TrueVote.Api.cs /namespace:TrueVote.Api`

## 🎁 Versioning

TrueVote.Bot uses [sementic versioning](https://semver.org/), starting with 1.0.0.

The patch (last segment of the 3 segments) is auto-incremented via a GitHub action when a pull request is merged to master. The GitHub action is configured in [.github/workflows/truevote-bot-version.yml](.github/workflows/truevote-bot-version.yml). To update the major or minor version, follow the instructions specified in the [bumping section of the action](https://github.com/anothrNick/github-tag-action#bumping) - use #major or #minor in the commit message to auto-increment the version.

## ❤️ Contributing

We welcome useful contributions. Please read our [contributing guidelines](CONTRIBUTING.md) before submitting a pull request.

## 📜 License

TrueVote.Bot is licensed under the MIT license.

[![License](https://img.shields.io/github/license/TrueVote/TrueVote.Bot)]((https://github.com/TrueVote/TrueVote.Bot/master/LICENSE))

[truevote.org](https://truevote.org)
<!---
Icons used from: https://emojipedia.org/
--->