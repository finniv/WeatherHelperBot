using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;

namespace Weather_Helper
{
    /// <summary>
    /// Логика взаимодействия для WindowStat.xaml
    /// </summary>
    public partial class WindowStat : Window
    {
        List<User> users = new List<User>();

        public WindowStat()
        {
            InitializeComponent();
            string text = System.IO.File.ReadAllText(@"log.txt");
            users = JsonConvert.DeserializeObject<List<User>>(text);
            users = users.GroupBy(a => a.ID).Select(g => g.First()).ToList();
            InitGrid();
        }

        void InitGrid()
        {
            dataGridUsers.ItemsSource = users;

            columnID.Binding = new Binding("ID");
            columnFirstName.Binding = new Binding("First_Name");
            columnLastName.Binding = new Binding("Last_Name");
            columnUsername.Binding = new Binding("Username");
        }

    }
    public class User
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }

        public User (int id, string firstName, string lastName, string username)
        {
            ID = id;
            First_Name = firstName;
            Last_Name = lastName;
            Username = username;
        }

    }
}

