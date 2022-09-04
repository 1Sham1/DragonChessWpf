using ChessRules;
using DragonChessKurs.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DragonChessKurs.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            DataContext = new GameViewModel(Board);
            /*
            chess = new Chess();
            GameField = new Button[3, 12, 8];

            for (int y = 0; y < 24; y++)
                for (int x = 0; x < 12; x++)
                {
                    Button btn1 = new Button();
                    if ((y + x) % 2 == 0 && y < 8)
                        btn1.Background = new SolidColorBrush(Colors.Aqua);
                    else if ((y + x) % 2 == 0 && y > 7 && y < 16)
                        btn1.Background = new SolidColorBrush(Colors.Green);
                    else if ((y + x) % 2 == 0 && y > 15)
                        btn1.Background = new SolidColorBrush(Colors.Red);
                    else
                        btn1.Background = new SolidColorBrush(Colors.White);
                    btn1.Width = 40;
                    btn1.Height = 40;
                    Board.Children.Add(btn1);
                    if (y < 8)
                        GameField[2, x, 7 - y] = btn1;
                    if (y > 7 && y < 16)
                        GameField[1, x, 23 - y - 8 ] = btn1;
                    if (y > 15)
                        GameField[0, x, 39 - y - 16] = btn1;
                }

            for (int z = 0; z < 3; z++)
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 12; x++)
                    {
                        if (chess.GetFigureAt(z, x, y) == 0)
                            continue;
                        Image img = new Image();
                        if(chess.GetFigureAt(z, x, y).ToString() == chess.GetFigureAt(z, x, y).ToString().ToLower())
                            img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\Black\" + chess.GetFigureAt(z, x, y) + ".png", UriKind.Relative));
                        else
                            img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\White\" + chess.GetFigureAt(z, x, y) + ".png", UriKind.Relative));
                        img.Height = 40;
                        img.Width = 40;
                        GameField[z, x, y].Content = img;
                    }
                           
            */
            
        }
    }
}
