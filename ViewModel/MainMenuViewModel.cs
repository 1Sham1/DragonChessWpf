using DragonChessKurs.Infrastructure.Commands;
using DragonChessKurs.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DragonChessKurs.ViewModel
{
    internal class MainMenuViewModel : ViewModelBase
    {

        #region Заголовок Окна

        private string _Title = "Драконьи шахматы";
       
        public string Title
        {
            get => _Title;
            set
            {
                Set(ref _Title, value);
            }
        }
        #endregion

        #region Закрытие окна
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Перейти к игре
        public ICommand StartMatchCommand { get; }
        private bool CanStartMatchCommandExecute(object p) => true;
        private void OnStartMatchCommandExecuted(object p)
        {
            GameWindow gameWindow = new GameWindow();
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Close();
            gameWindow.Show();
        }
        #endregion


        public MainMenuViewModel()
        {
            #region Команды
            CloseApplicationCommand
                = new LambdaCommand(OnCloseApplicationCommandExecuted,
                                    CanCloseApplicationCommandExecute);

            StartMatchCommand 
                = new LambdaCommand(OnStartMatchCommandExecuted,
                                    CanStartMatchCommandExecute);
            #endregion
        }
    }
}
