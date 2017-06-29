using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot.Types;

namespace Weather_Helper
{
    public class BotCoreWH
    {
        string BotToken = "395855585:AAHnPvZLP-GlzPZw-IxGj1OSSm7Uk1GPJGk";
        private string ChatKeyID = "67468540";
      
        public string _ChatKeyID { get => ChatKeyID; set => ChatKeyID = value; }
        public string _BotToken { get => BotToken1; set => BotToken1 = value; }
        public string _BotCityName { get => BotCityName; set => BotCityName = value; }
        public string _TempText { get => TempText; set => TempText = value; }
        public string WeatherAppID1 { get => WeatherAppID; set => WeatherAppID = value; }
        public string BotToken1 { get => BotToken; set => BotToken = value; }

        BackgroundWorker bw;
        private string BotCityName;
        string TempText;
        string WeatherAppID= "3e99f3f48abcb686f41634b539bbf59e";
        

        public void MainWH()
        {
            this.bw = new BackgroundWorker();
            this.bw.DoWork += this.bw_DoWork;
        }

        public void LoadForm()
        {
            if (this.bw.IsBusy != true || bw.IsBusy == false) // если не запущен
            {
                this.bw.RunWorkerAsync(); // запускаем
            }
            var text = _BotToken; 
            if (bw.IsBusy != true)
            {
                this.bw.RunWorkerAsync(text); // передаем эту переменную в виде аргумента методу bw_DoWork
            }
        }

        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker; // получаем ссылку на класс вызвавший событие
            var key = e.Argument as String; // получаем ключ из аргументов
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(_BotToken); // инициализируем API
                try
                {
                    await Bot.SetWebhookAsync("");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                
                
                int offset = 0; // отступ по сообщениям
                while (true)
                {
                    try {
                            var updates = await Bot.GetUpdatesAsync(offset); // получаем массив обновлений

                            foreach (var update in updates) // Перебираем все обновления
                            {
                                Console.WriteLine(update.Type);
                                offset = update.Id + 1;
                                if (offset > update.Id + 1)
                                {
                                    while (offset > update.Id + 1)
                                    {
                                        offset = offset - 1;
                                    }
                                }
                             }
                        foreach (var update in updates) // Перебираем все обновления
                        {
                            var message = update.Message;
                            if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                            {
                                if (message.Text == "/help" || message.Text == "/help@WeatherHelper_bot" || message.Text == "/start")
                                {
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Бот выводит информацию о температуре воздуха за окном.Введите одну из команд.\n" +
                                        "Список команд :\n" +
                                        "/whkharkiv - Температура в Харькове\n" +
                                        "/whkiev - Температура в Киеве\n" +
                                        "/whlviv - Температура во Львове\n" +
                                        "/whpoltava - Температура в Полтаве\n" +
                                        "/whdonetsk - Температура В Донецке\n" +
                                        "/whzhytomyr - Температура в Житомире\n");
                                }
                                if (message.Text == "/saysomething")
                                {
                                    // в ответ на команду /saysomething выводим сообщение
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "тест",
                                            replyToMessageId: message.MessageId);
                                }
                                if (message.Text == "/whdonetsk" || message.Text == "/whdonetsk@WeatherHelper_bot")
                                {
                                    _BotCityName = "Donetsk";
                                    GetWeatherBot(sender, null);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + _BotCityName);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);
                                }
                                if (message.Text == "/whzhytomyr" || message.Text == "/whzhytomyr@WeatherHelper_bot")
                                {
                                    _BotCityName = "Zhytomyr";
                                    GetWeatherBot(sender, null);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + _BotCityName);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);
                                }
                                if (message.Text == "/whkharkiv" || message.Text == "/whkharkiv@WeatherHelper_bot")
                                {
                                    _BotCityName = "Kharkiv";
                                    GetWeatherBot(sender, null);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + _BotCityName);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);
                                }
                                if (message.Text == "/whkiev@WeatherHelper_bot" || message.Text == "/whkiev")
                                {
                                    _BotCityName = "Kiev";
                                    GetWeatherBot(sender, null);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + _BotCityName);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);
                                }
                                if (message.Text == "/whlviv@WeatherHelper_bot" || message.Text == "/whlviv")
                                {
                                    _BotCityName = "Lviv";
                                    GetWeatherBot(sender, null);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + _BotCityName);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);
                                }
                                if (message.Text == "/whpoltava@WeatherHelper_bot" || message.Text == "/whpoltava")
                                {
                                    _BotCityName = "Poltava";
                                    GetWeatherBot(sender, null);

                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + _BotCityName);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);
                                }

                            }
                            offset = update.Id + 1;
                        }
                        }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }

        }



        async private void GetWeatherBot(object sender, object e)
        {
            var WeatherBotClient = new HttpClient();//Http слушатель

            var WeatherResponse = await WeatherBotClient.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?q=" + _BotCityName + "&mode=json&units=metric&appid=" + WeatherAppID1);/*WEB-Запрос ,результатом которого является JSON*/

            dynamic x = JsonConvert.DeserializeObject(WeatherResponse);
            /*Динамичесґкая переменная,которая позволяет работать с данными ,
            которые отсутствуют на момент компиляции
            NewtonSoft библиотека для десериализации Json*/

            _TempText = $"{x.main.temp}" + "C°";
            Thread.Sleep(1000);
        }
    }
}
