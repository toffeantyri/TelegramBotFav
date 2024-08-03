using Telegram.Bot;
using TelegramBotFav;
using TelegramBotFav.Models;

class Startup
{
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;

    static async Task Main()
    {
        _botClient = new TelegramBotClient(AppSettings.Token); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather

        using var cts = new CancellationTokenSource();

        IMessageHandler messageHandler = new DeliveryController(AppSettings.BaseUrl); 
       
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