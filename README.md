# Azure Functions + Telegram bots = ❤️

Check out the full article here - https://www.codingwithmiszu.com/2021/12/12/create-a-telegram-bot-with-azure-functions-net-6-isolated-process/

# Set up

✅ Azure Functions V4 in isolated process execution mode

✅ NET 6

## How to run

```
az functionapp config appsettings set --name FUNCTIONS_APP_NAME --resource-group RESOURCE_GROUP_NAME --settings "TelegramBotToken=YOUR_BOT_TOKEN"
func azure functionapp publish FUNCTIONS_APP_NAME
```

Open the /setup link (the last command above will output it), then the bot should start responding.

## Preview
![Image](https://github.com/miszu/IsolatedAzureFunctionsTelegramBot/blob/main/preview.gif?raw=true)
