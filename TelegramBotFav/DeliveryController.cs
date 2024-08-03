using Refit;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotFav.Controllers;
using TelegramBotFav.network;

namespace TelegramBotFav
{
    public class DeliveryController : IMessageHandler
    {
        private ApiService api;

        public DeliveryController(string newBaseUrl)
        {
            api = RestService.For<ApiService>(newBaseUrl);
        }

        public ReceiverOptions GetReceiverOptions()
        {
            return new ReceiverOptions
            {
                AllowedUpdates = new[]{
                UpdateType.Message,
                UpdateType.CallbackQuery
            },
                ThrowPendingUpdates = false,
            };
        }

        public Task OnErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task OnUpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                //Логаем что пришло
                var message = update.Message;
                if (message != null && message.From != null)
                {
                    var user = message.From;
                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");
                }

                var commandContr = new CommandController(botClient);

                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            switch (message.Type) { 
                                case MessageType.Text: {
                                        if (message.Text.StartsWith("/"))
                                        {
                                            await commandContr.ExecuteCommand(message.Chat.Id, message.Text);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        await botClient.SendTextMessageAsync(
                                            message.Chat.Id,
                                            "Используй только текст!");
                                        return;
                                    }
                            }

                            break;
                        }
                    case UpdateType.CallbackQuery:
                        {

                            break;
                        }
                    default:
                        {

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnUpdate Error: {ex.ToString}");
            }

        }
    }
}
