using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Telegram.Bot.Types;

namespace Weather_Helper
{
    public class BotCoreWH
    {
        private string ChatKeyID = "390705252";
        string BotToken = "395855585:AAHnPvZLP-GlzPZw-IxGj1OSSm7Uk1GPJGk";
        string WeatherAppID = "3e99f3f48abcb686f41634b539bbf59e";

        public string _ChatKeyID { get => ChatKeyID; set => ChatKeyID = value; }
        public string _BotToken { get => BotToken;}
        public string _TempText { get; set; }
        public string _Image { get; set; }
        public string WeatherAppID1 { get => WeatherAppID;}


        BackgroundWorker bw;
        JsonSerializer serializer = new JsonSerializer();

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

            if (bw.IsBusy != true)
            {
                this.bw.RunWorkerAsync(_BotToken); // передаем эту переменную в виде аргумента методу bw_DoWork
            }
        }

        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            /**Зачем вот это вот? Оно же нигде не используется*/
            var worker = sender as BackgroundWorker; // получаем ссылку на класс вызвавший событие
            var key = e.Argument as String; // получаем ключ из аргументов

            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(_BotToken); 

                try
                {
                    await Bot.SetWebhookAsync("");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                
                int offset = 0; //отступ по сообщениям

                while (true)
                {
                    try
                    {
                       
                        var updates = await Bot.GetUpdatesAsync(offset); 
                        foreach (var update in updates)
                            {
                                offset = update.Id + 1;

                            while (offset > update.Id + 1)
                            {
                                offset = offset - 1;
                            }
                            
                        }
                        
                        foreach (var update in updates)
                        {
                            var message = update.Message;

                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            //Вот это вот основано на моей фантазии и ничего лучше я не придумала
                            try
                            {
                                string json = System.IO.File.ReadAllText("log.txt");
                                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                jsonObj.Add(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(message.From)));

                                using (StreamWriter sw = new StreamWriter("log.txt"))
                                using (JsonWriter writer = new JsonTextWriter(sw))
                                {
                                    serializer.Serialize(writer, jsonObj);
                                }
                            }

                            catch
                            {
                                using (StreamWriter sw = new StreamWriter("log.txt"))
                                using (JsonWriter writer = new JsonTextWriter(sw))
                                {
                                    writer.WriteStartArray();
                                    serializer.Serialize(writer, message.From);
                                    writer.WriteEnd();
                                }
                            }

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
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                                    }
                                else
                                {
                                    string CityName = message.Text;
                                    CityName = CityName.Substring(3);

                                    if (CityName.IndexOf('@') != -1)
                                        CityName = CityName.Remove(CityName.IndexOf('@'));

                                    GetWeatherBot(sender, null, CityName);

                                    Thread.Sleep(1000);
                                    if (_TempText != null)
                                    {
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Погода в городе " + CityName);
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Сейчас " + _TempText);                                        
                                        
                                        using (var stream = System.IO.File.Open(_Image, FileMode.Open))
                                        {
                                            FileToSend fts = new FileToSend();
                                            fts.Content = stream;
                                            fts.Filename = _Image.Split('\\').Last();
                                            await Bot.SendPhotoAsync(message.Chat.Id, fts);
                                        }
                                    }

                                    else
                                    {
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Указанный город " + CityName + " не найден");
                                    }
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

        async private void GetWeatherBot(object sender, object e, string CityName)
        {
            try
            {
                var WeatherBotClient = new HttpClient();//Http слушатель
                var WeatherResponse = await WeatherBotClient.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?q=" + CityName + "&mode=json&units=metric&appid=" + WeatherAppID1);/*WEB-Запрос ,результатом которого является JSON*/

                dynamic x = JsonConvert.DeserializeObject(WeatherResponse);

                _TempText = $"{x.main.temp}" + "C°";
                _Image = @"weather_img\" + x.weather[0].icon + ".png";
            }
            catch 
            {
                _TempText = null;
            }
        }
    }
}
