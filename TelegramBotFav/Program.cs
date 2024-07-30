using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotFav;

class Program
{
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;

    static async Task Main()
    {
        _botClient = new TelegramBotClient("7119492116:AAGi4U5l-hgZpxq-DPEtUpv3mr3VMrh9kk4"); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather

        using var cts = new CancellationTokenSource();

        IMessageHandler messageHandler = new TestMessageHandlerImpl(); 
       
        _botClient.StartReceiving(
            messageHandler.OnUpdateHandler,  // UpdateHander - обработчик приходящих Update`ов
            messageHandler.OnErrorHandler,  // ErrorHandler - обработчик ошибок, связанных с Bot API
            messageHandler.GetReceiverOptions(),
            cts.Token
            ); // Запускаем бота
        
        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

}