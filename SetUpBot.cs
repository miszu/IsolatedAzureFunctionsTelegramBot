using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace LocalFunctionProj
{
    public class SetUpBot
    {
        private readonly TelegramBotClient _botClient;

        public SetUpBot()
        {
            _botClient = new TelegramBotClient(System.Environment.GetEnvironmentVariable("TelegramBotToken", EnvironmentVariableTarget.Process));
        }

        private const string SetUpFunctionName = "setup";
        private const string UpdateFunctionName = "handleupdate";

        [Function(SetUpFunctionName)]
        public async Task RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var handleUpdateFunctionUrl = req.Url.ToString().Replace(SetUpFunctionName, UpdateFunctionName,
                                                ignoreCase: true, culture: CultureInfo.InvariantCulture);
            await _botClient.SetWebhookAsync(handleUpdateFunctionUrl);
        }

        [Function(UpdateFunctionName)]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var request = await req.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Telegram.Bot.Types.Update>(request);

            if (update.Type != UpdateType.Message)
                return;
            if (update.Message!.Type != MessageType.Text)
                return;

            await _botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: GetBotResponseForInput(update.Message.Text));
        }

        private string GetBotResponseForInput(string text)
        {
            try
            {
                if (text.Contains("pod bay doors", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "I'm sorry Miszu, I'm afraid I can't do that 🛰";
                }

                return new DataTable().Compute(text, null).ToString();
            }
            catch
            {
                return $"Dear human, I can solve math for you, try '2 + 2 * 3' 👀";
            }
        }
    }
}