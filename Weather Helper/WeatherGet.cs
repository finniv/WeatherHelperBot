using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Weather_Helper
{
    public class WeatherGet
    {
       
        private string appID = "3e99f3f48abcb686f41634b539bbf59e";//ID доступа к API
        string ChosenCity = "Kharkiv";//Выбранный город

        public string AppID { get => appID; set => appID = value; }
        public string ChosenCity1 { get => ChosenCity; set => ChosenCity = value; }
        public async Task GetWeatherAsync()
        {
            var WeatherClient = new HttpClient();//Http слушатель


            var WeatherResponse = await WeatherClient.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?q=" + ChosenCity1 + "&mode=json&units=metric&appid=" + AppID);/*WEB-Запрос ,результатом которого является JSON*/

            dynamic x = JsonConvert.DeserializeObject(WeatherResponse);
            /*Динамичесґкая переменная,которая позволяет работать с данными ,
            которые отсутствуют на момент компиляции
            NewtonSoft библиотека для десериализации Json*/

            string WGText = $"{x.main.temp}" + "C°";
        }
    }
}
