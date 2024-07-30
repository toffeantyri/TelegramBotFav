﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotFav
{
    internal class TestMessageHandlerImpl : IMessageHandler
    {
        public ReceiverOptions GetReceiverOptions()
        {
             return new ReceiverOptions // Также присваем значение настройкам бота
            {
                AllowedUpdates = new[] // типы получаемых Update`ов, https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery
            },
                // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
                // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
                ThrowPendingUpdates = true,
            };
        }

        Task IMessageHandler.OnErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        async Task IMessageHandler.OnUpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
                // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
                try
                {
                    // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                    switch (update.Type)
                    {
                        case UpdateType.Message:
                            {
                                // Эта переменная будет содержать в себе все связанное с сообщениями
                                var message = update.Message;

                                // From - это от кого пришло сообщение (или любой другой Update)
                                var user = message.From;

                                // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                                Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                                // Chat - содержит всю информацию о чате
                                var chat = message.Chat;

                                // Добавляем проверку на тип Message
                                switch (message.Type)
                                {
                                    // Тут понятно, текстовый тип
                                    case MessageType.Text:
                                        {
                                            // тут обрабатываем команду /start, остальные аналогично
                                            if (message.Text == "/start")
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    chat.Id,
                                                    "Выбери клавиатуру:\n" +
                                                    "/inline\n" +
                                                    "/reply\n");
                                                return;
                                            }

                                            if (message.Text == "/inline")
                                            {
                                                // Тут создаем нашу клавиатуру
                                                var inlineKeyboard = new InlineKeyboardMarkup(
                                                    new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                                    {
                                        // Каждый новый массив - это дополнительные строки,
                                        // а каждая дополнительная кнопка в массиве - это добавление ряда

                                        new InlineKeyboardButton[] // тут создаем массив кнопок
                                        {
                                            InlineKeyboardButton.WithUrl("Это кнопка с сайтом", "https://habr.com/"),
                                            InlineKeyboardButton.WithCallbackData("А это просто кнопка", "button1"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Тут еще одна", "button2"),
                                            InlineKeyboardButton.WithCallbackData("И здесь", "button3"),
                                        },
                                                    });

                                                await botClient.SendTextMessageAsync(
                                                    chat.Id,
                                                    "Это inline клавиатура!",
                                                    replyMarkup: inlineKeyboard); // Все клавиатуры передаются в параметр replyMarkup

                                                Console.WriteLine("Inline keyboard created");

                                                return;
                                            }

                                            if (message.Text == "/reply")
                                            {
                                                // Тут все аналогично Inline клавиатуре, только меняются классы
                                                // НО! Тут потребуется дополнительно указать один параметр, чтобы
                                                // клавиатура выглядела нормально, а не как абы что

                                                var replyKeyboard = new ReplyKeyboardMarkup(
                                                    new List<KeyboardButton[]>()
                                                    {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Привет!"),
                                            new KeyboardButton("Пока!"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Позвони мне!")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Напиши моему соседу!")
                                        }
                                                    })
                                                {
                                                    // автоматическое изменение размера клавиатуры, если не стоит true,
                                                    // тогда клавиатура растягивается чуть ли не до луны,
                                                    // проверить можете сами
                                                    ResizeKeyboard = true,
                                                };

                                                await botClient.SendTextMessageAsync(
                                                    chat.Id,
                                                    "Это reply клавиатура!",
                                                    replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup

                                                return;
                                            }

                                            if (message.Text == "Позвони мне!")
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    chat.Id,
                                                    "Хорошо, присылай номер!",
                                                    replyToMessageId: message.MessageId);

                                                return;
                                            }

                                            if (message.Text == "Напиши моему соседу!")
                                            {
                                                await botClient.SendTextMessageAsync(
                                                    chat.Id,
                                                    "А самому что, трудно что-ли ?",
                                                    replyToMessageId: message.MessageId);

                                                return;
                                            }

                                            return;
                                        }

                                    // Добавил default , чтобы показать вам разницу типов Message
                                    default:
                                        {
                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Используй только текст!");
                                            return;
                                        }
                                }

                                return;
                            }

                        case UpdateType.CallbackQuery:
                            {
                                // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                                var callbackQuery = update.CallbackQuery;

                                // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
                                var user = callbackQuery.From;

                                // Выводим на экран нажатие кнопки
                                Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                                // Вот тут нужно уже быть немножко внимательным и не путаться!
                                // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
                                // кнопка привязана к сообщению, то мы берем информацию от сообщения.
                                var chat = callbackQuery.Message.Chat;

                                // Добавляем блок switch для проверки кнопок
                                switch (callbackQuery.Data)
                                {
                                    // Data - это придуманный нами id кнопки, мы его указывали в параметре
                                    // callbackData при создании кнопок. У меня это button1, button2 и button3

                                    case "button1":
                                        {
                                            // В этом типе клавиатуры обязательно нужно использовать следующий метод
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                            // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                $"Вы нажали на {callbackQuery.Data}");
                                            return;
                                        }

                                    case "button2":
                                        {
                                            // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Тут может быть ваш текст!");

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                $"Вы нажали на {callbackQuery.Data}");
                                            return;
                                        }

                                    case "button3":
                                        {
                                            // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "А это полноэкранный текст!", showAlert: true);

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                $"Вы нажали на {callbackQuery.Data}");
                                            return;
                                        }
                                }

                                return;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

        }
}
