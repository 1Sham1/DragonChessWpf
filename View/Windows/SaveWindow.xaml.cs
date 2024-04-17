using System;
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
    public partial class SaveWindow : Window
    {
        private string fen;
        private ListBox MoveHistory;
        public SaveWindow(string fen, ListBox MoveHistory)
        {
            InitializeComponent();
            this.fen = fen;
            this.MoveHistory = MoveHistory;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (SaveNametextBox.Text != "")
            {
                List<string> temp = new List<string>();
                using (StreamReader sw = new StreamReader("date.txt"))
                {
                    
                    string s;
                    while ((s = sw.ReadLine()) != null)
                    {
                        string save = s;
                        if (save != null)
                        {
                            if (SaveNametextBox.Text != (save.Split('|'))[0])
                                temp.Add(save);
                        }
                    }

                }
                using (StreamWriter  sw = new StreamWriter("date.txt", false))
                {
                    var tempHistory = string.Join("!", MoveHistory.Items.Cast<String>().ToList().ToArray());
                    temp.Add(SaveNametextBox.Text + "|" + fen +"|"+tempHistory);
                    foreach(string save in temp)
                    {
                        sw.WriteLine(save);
                    }
                     
                }

                    this.DialogResult = true;
            }
            else
                MessageBox.Show("Введите название сохранения");
        }
    }
}
