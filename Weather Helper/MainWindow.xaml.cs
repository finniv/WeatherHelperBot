using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Telegram.Bot;

namespace Weather_Helper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 706483 Харків ІД для openweathermap
    /// https://api.telegram.org/bot398650303:AAEIAmvLtXF70UK1HE_r9X7YePn3Q-bXLl0/getUpdates запит для отримання ID користувача

    public partial class AdminWindow : Window
    {
        WeatherGet wget = new WeatherGet();
        BotCoreWH bcwh = new BotCoreWH();
        private string appID = "3e99f3f48abcb686f41634b539bbf59e";
        public string AppID { get => appID; set => appID = value; }
        //Таймер для автообновления погоды
        DispatcherTimer AutoUpd = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(10) };

        public AdminWindow()
        {
            InitializeComponent();
            AutoUpd.Tick += Refresh;
            AutoUpd.Start();
        }
        
        private async void Refresh(object sender, object e)//Асинхонный метод для работы с запросами
        {
            var WeatherClient = new HttpClient();//Http слушатель
            string WeatherResponse = null;
            try
            {
                WeatherResponse = await WeatherClient.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?q=" + wget.ChosenCity1 + "&mode=json&units=metric&appid=" + AppID);/*WEB-Запрос ,результатом которого является JSON*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            try
            {
                dynamic x = JsonConvert.DeserializeObject(WeatherResponse);

                TempText.Text = $"{x.main.temp}" + "C°";
                BitmapImage img = new BitmapImage(new Uri($"http://openweathermap.org/img/w/{x.weather[0].icon}.png"));
                Img.Source = img;
            }
            catch
            {
                MainWindow1_Loaded(sender, null);
            }
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            bcwh.MainWH();
            bcwh.LoadForm();
            Refresh(sender, null);
        }

        /*Метод,который позволяет отправлять сообщение с текущей температурой за окном на указанный ID*/
        private async void TelegramSend_Click(object sender, RoutedEventArgs e)
        {
            var bot = new TelegramBotClient(bcwh._BotToken);/*Токен для доступа к боту*/
            
            var s = await bot.SendTextMessageAsync(bcwh._ChatKeyID, "Сейчас на улице в городе  "+CityName.Text +" " + TempText.Text);
        }

        private void CityButton_Click(object sender, RoutedEventArgs e)
        {
            switch (CityName.Text)
            {
                case "Харьков":
                    wget.ChosenCity1 = "Kharkiv";
                    Refresh(sender, null);
                    break;
                case "Киев":
                    wget.ChosenCity1 = "Kiev";
                    Refresh(sender, null);
                    break;
                case "Львов":
                    wget.ChosenCity1 = "Lviv";
                    Refresh(sender, null);
                    break;
                case "Донецк":
                    wget.ChosenCity1 = "Donetsk";
                    Refresh(sender, null);
                    break;
                case "Житомир":
                    wget.ChosenCity1 = "Zhytomyr";
                    Refresh(sender, null);
                    break;
                case "Полтава":
                    wget.ChosenCity1 = "Poltava";
                    Refresh(sender, null);
                    break;
                default:
                    wget.ChosenCity1 = "Kharkiv";
                    break;
            }
        }

        async private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string ChatId = IDChatBox.Text;
            string ChatMesage = SendMessageBox.Text;
            try
            {
                var bot = new TelegramBotClient(bcwh._BotToken);
                var send = await bot.SendTextMessageAsync(ChatId, ChatMesage);
                MessageBox.Show("Отправлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
          
        }

        private void StatButton_Click(object sender, RoutedEventArgs e)
        {
            WindowStat winst = new WindowStat();
            winst.Show();
        }
    }
}
