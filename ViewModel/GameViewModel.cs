using DragonChessKurs.Models;
using DragonChessKurs.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DragonChessKurs.ViewModel
{
    internal class GameViewModel : ViewModelBase
    {
        private ChessModel Model;

        #region Доски
        private UniformGrid _Board;
        public UniformGrid Board
        {
            get => _Board;
            set
            {
                Set(ref _Board, value);
            }
        }
        #endregion

        #region Выполнение проверки хода
        public ICommand MoveDirectionCommand { get; }
        private bool CanMoveDirectionCommandExecute(object p)
        {
            return true;
        }
        private void OnMoveDirectionCommandExecuted(object p)
        {
          Model.CheckMoveState(p.ToString());
        }
        #endregion

        #region Начать новую игру
        public ICommand StartNewGameCommand { get; }
        private bool CanStartNewGameCommandExecute(object p) => true;
        private void OnStartNewGameCommandExecuted(object p)
        {
            MessageBox.Show("New Game");
            SetupBoard();
        }
        #endregion

        #region Инициализация игрового поля
        private void SetupBoard()
        {
            Board.Children.Clear();
            Model = new ChessModel();
            #region Размещение доски
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
                    btn1.Command = MoveDirectionCommand;
                    Board.Children.Add(btn1);
                        if (y < 8)
                            Model.GameField[2, x, 7 - y].btn = btn1;
                        if (y > 7 && y < 16)
                            Model.GameField[1, x, 23 - y - 8].btn = btn1;
                        if (y > 15)
                            Model.GameField[0, x, 39 - y - 16].btn = btn1;
                    
                }
            #endregion

            #region Размещение фигур
            string figure;
            for (int z = 0; z < 3; z++)
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 12; x++)
                    {
                        Model.GameField[z, x, y].coordinate = (z+1).ToString() + ((Char)(x+'a')).ToString() + (y+1).ToString();
                        figure = Model.chess.GetFigureAt(z, x, y).ToString();
                       
                        Image img = new Image();
                        if (figure == figure.ToLower())
                            img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\Black\" + Model.chess.GetFigureAt(z, x, y) + ".png", UriKind.Relative));
                        else
                            img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\White\" + Model.chess.GetFigureAt(z, x, y) + ".png", UriKind.Relative));
                        Model.GameField[z, x, y].figure = figure;
                        img.Height = 40;
                        img.Width = 40;
                        Model.GameField[z, x, y].btn.Content = img;
                        Model.GameField[z, x, y].btn.CommandParameter = Model.GameField[z, x, y].figure + Model.GameField[z, x, y].coordinate;
                    }
            
            #endregion
        }
        #endregion

        public GameViewModel(UniformGrid Board)
        {
            this._Board = Board;
            #region Команды
            StartNewGameCommand
                = new LambdaCommand(OnStartNewGameCommandExecuted,
                                    CanStartNewGameCommandExecute);
            MoveDirectionCommand
                = new LambdaCommand(OnMoveDirectionCommandExecuted,
                                    CanMoveDirectionCommandExecute);
            #endregion
        }
        
        public GameViewModel()
        {
            #region Команды
            StartNewGameCommand
                = new LambdaCommand(OnStartNewGameCommandExecuted,
                                    CanStartNewGameCommandExecute);
            #endregion
        }

    }
}
