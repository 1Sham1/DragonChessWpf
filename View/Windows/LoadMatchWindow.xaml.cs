using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

namespace DragonChessKurs.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoadMatchWindow.xaml
    /// </summary>
    public partial class LoadMatchWindow : Window
    {
        private List<string> saveNames;
        private List<string> saveFen;
        private List<string> saveMoveHistory;

        public string fen { get; private set; }
        public string MH { get; private set; }
        public LoadMatchWindow()
        {
            InitializeComponent();
            saveNames = new List<string>();
            saveFen = new List<string>();
            saveMoveHistory = new List<string>();
            readDateFromSaveFile();            
            SaveList.ItemsSource = saveNames;
        }

        private void readDateFromSaveFile()
        {            
            
            using (StreamReader sw = new StreamReader("date.txt"))
            {
                string s;
                while ((s = sw.ReadLine()) != null)
                {
                    var split = s.Split('|');
                    saveNames.Add(split[0]);
                    saveFen.Add(split[1]);
                    saveMoveHistory.Add(split[2]);
                }              
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (SaveList.SelectedIndex != -1)
            {
                fen = saveFen[SaveList.SelectedIndex];
                MH = saveMoveHistory[SaveList.SelectedIndex];
                this.DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
