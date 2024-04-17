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
using DragonChessKurs.View.Windows;
using System.Threading;

namespace DragonChessKurs.ViewModel
{
    internal class GameViewModel : ViewModelBase
    {
        private bool isGameTime = false;
        private string startPosition = "11g111i111g1/s1s1s1s1s1s1/111111111111/111111111111/111111111111/111111111111/S1S1S1S1S1S1/11G111I111G1/ouhacmkpahuo/wwwwwwwwwwww/111111111111/111111111111/111111111111/111111111111/WWWWWWWWWWWW/OUHACMKPAHUO/11b111e111b1/1d1d1d1d1d1d/111111111111/111111111111/111111111111/111111111111/1D1D1D1D1D1D/11B111E111B1 w 0 1";

        private ChessManager Model;
               
        #region Чей ход

        private string _WhoseTurnTitle = "";

        public string WhoseTurnTitle
        {
            get => _WhoseTurnTitle;
            set
            {
                Set(ref _WhoseTurnTitle, value);                
            }
        }
        #endregion

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

        private ListBox MoveHistory;

        #region Выполнение проверки хода
        public ICommand MoveDirectionCommand { get; }
        private bool CanMoveDirectionCommandExecute(object p)
        {
            return true;
        }
        private void OnMoveDirectionCommandExecuted(object p)
        {
            if (isGameTime)
            {
                Model.CheckMoveState(p.ToString());
                ChangeTurnTitle();
            }
                EndGame();
        }

        private void ChangeTurnTitle()
        {

            if (Model.chess.GetUpdateMoveColor())
                Set(ref _WhoseTurnTitle, "Ход золотых", "WhoseTurnTitle");
            else
                Set(ref _WhoseTurnTitle, "Ход фиолетовых", "WhoseTurnTitle");
        }

        private void EndGame()
        {
            if (Model.chess.IsCheckmate)
            {

                isGameTime = false;
                if (Model.chess.GetUpdateMoveColor() == false)
                    MessageBox.Show("Поставлен мат фиолетовым");
                else
                    MessageBox.Show("Поставлен мат золотым");
            }

            if (Model.chess.IsStalemate)
            {
                isGameTime = false;
                MessageBox.Show("Ничья!");
            }

        }
        #endregion

        #region Начать новую игру
        public ICommand StartNewGameCommand { get; }
        private bool CanStartNewGameCommandExecute(object p) => true;
        private void OnStartNewGameCommandExecuted(object p)
        {
            SetupBoard();
        }

        #region Инициализация игрового поля
        private void SetupBoard(string fen = "11g111i111g1/s1s1s1s1s1s1/111111111111/111111111111/111111111111/111111111111/S1S1S1S1S1S1/11G111I111G1/ouhacmkpahuo/wwwwwwwwwwww/111111111111/111111111111/111111111111/111111111111/WWWWWWWWWWWW/OUHACMKPAHUO/11b111e111b1/1d1d1d1d1d1d/111111111111/111111111111/111111111111/111111111111/1D1D1D1D1D1D/11B111E111B1 w 0 1")
        {
            Board.Children.Clear();
            MoveHistory.Items.Clear();
            Model = new ChessManager(MoveHistory,fen);
            isGameTime = true;
            #region Размещение доски
            for (int y = 0; y < 24; y++)
                for (int x = 0; x < 12; x++)
                {
                    Button btn1 = new Button();
                    if ((y + x) % 2 != 0 && y < 8)
                        btn1.Background = new SolidColorBrush(Colors.Aqua);
                    else if ((y + x) % 2 != 0 && y > 7 && y < 16)
                        btn1.Background = new SolidColorBrush(Colors.Green);
                    else if ((y + x) % 2 != 0 && y > 15)
                        btn1.Background = new SolidColorBrush(Colors.Red);
                    else
                        btn1.Background = new SolidColorBrush(Colors.White);
                    btn1.Width = 40;
                    btn1.Height = 40;
                    btn1.Command = MoveDirectionCommand;
                    //btn1.BorderBrush = new SolidColorBrush(Colors.Black);
                    btn1.BorderThickness = new Thickness(0);
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
            Set(ref _WhoseTurnTitle, "Ход золотых", "WhoseTurnTitle");
        }
        #endregion


        #endregion

        #region Закрытие окна
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Открытие окна сохранения текущей игры
        public ICommand SaveGameCommand { get; }
        private bool CanSaveGameCommandExecute(object p) => true;
        private void OnSaveGameCommandExecuted(object p)
        {
            if (Model != null)
            {
                SaveWindow sw = new SaveWindow(Model.chess.fen, MoveHistory);
                if (sw.ShowDialog() == true)
                {
                    MessageBox.Show("Сохранение прошло успешно");
                }
                
            }
        }
        #endregion

        #region Открытие окна загрузки игры
        public ICommand LoadGameCommand { get; }
        private bool CanLoadGameCommandExecute(object p) => true;
        private void OnLoadGameCommandExecuted(object p)
        {
            if (Model == null)            
                SetupBoard();
            
                LoadMatchWindow lmw = new LoadMatchWindow();
                if (lmw.ShowDialog() == true)
                {
                startPosition = lmw.fen;                
                
                SetupBoard(startPosition);                    
                var temp = lmw.MH.Split('!');
                for(int i = temp.Length-1;i>=0;i--)
                    MoveHistory.Items.Insert(0, temp[i]);
                MessageBox.Show("Игра успешно загружена");
            }               
           

        }
        #endregion

        #region Открытие окна о программе
        public ICommand AboutGameCommand { get; }
        private bool CanAboutGameCommandExecute(object p) => true;
        private void OnAboutGameCommandExecuted(object p)
        {
            MessageBox.Show("Данная программа представляет из себя шахматную игру с 3 полями, написал студент Орлов В.И. в рамках курсового проекта.");
        }
        #endregion


        public GameViewModel(UniformGrid Board, ListBox MoveHistory)
        {
            this._Board = Board;
            this.MoveHistory = MoveHistory;
            #region Команды
            CloseApplicationCommand
               = new LambdaCommand(OnCloseApplicationCommandExecuted,
                                   CanCloseApplicationCommandExecute);

            StartNewGameCommand
                = new LambdaCommand(OnStartNewGameCommandExecuted,
                                    CanStartNewGameCommandExecute);
            MoveDirectionCommand
                = new LambdaCommand(OnMoveDirectionCommandExecuted,
                                    CanMoveDirectionCommandExecute);
            SaveGameCommand
                = new LambdaCommand(OnSaveGameCommandExecuted,
                                    CanSaveGameCommandExecute);
            LoadGameCommand
               = new LambdaCommand(OnLoadGameCommandExecuted,
                                   CanLoadGameCommandExecute);
            AboutGameCommand
               = new LambdaCommand(OnAboutGameCommandExecuted,
                                   CanAboutGameCommandExecute);
            #endregion

        }       

    }
}
