using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBotFav
{
    internal interface IMessageHandler
    {
        // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
        public ReceiverOptions GetReceiverOptions();

        public Task OnUpdateHandler(
            ITelegramBotClient botClient, Update update, CancellationToken cancellationToken
            );

        public Task OnErrorHandler
            (ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken
            );
    }
}
