using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFav.network;

namespace TelegramBotFav.Controllers
{
    internal class CommandController
    {
        private ITelegramBotClient tgBotClient;
        private ApiService apiService;
        
        internal CommandController(ITelegramBotClient botClient, ApiService api)
        {
            tgBotClient = botClient;
            apiService = api;
        }
        public async Task ExecuteCommand(ChatId chatIdent,string command)
        {
            switch (command)
            {
                case "/help":
                    {
                        await tgBotClient.SendTextMessageAsync(
                             chatIdent,
                             "Помощь по командам : \n" +
                             "/help - вызов помощи по командам \n" +
                             "/getTransportTypes - получение данных от доступных видах транспорта\n"
                            );
                        break;
                    }

                case "/start":
                    {
                        Console.WriteLine("Bot Запущен");
                        await tgBotClient.SendTextMessageAsync(
                             chatIdent,
                             "Бот запущен"
                            );
                        break;
                    }

                case "/getTransportTypes": {
                        var transports = await apiService.GetTransportTypes();
                        foreach (var item in transports)
                        {
                            Console.WriteLine($" getTransportTypes {item.Id} {item.Title} {item.Slug} {item.Max_Distance_Km} {item.Max_Weight_Kg}");
                        }
                        
                        break;
                    }

                default:
                    {
                        await tgBotClient.SendTextMessageAsync(
                             chatIdent,
                             "Извините - не знаю такой команды. Помощь по командам /help"
                            );
                        break;
                    }

            }
        }
    }
}
