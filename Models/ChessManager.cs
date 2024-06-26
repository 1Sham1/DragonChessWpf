﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessRules;

namespace DragonChessKurs.Models
{
   
    internal class ChessManager
    {
       public Chess chess;
        public Cell[,,] GameField;
        private MoveState moveState;
        private string figureFrom;
        private string from;
        private ListBox MoveHistory;
        private SoundPlayer moveSound;

        enum MoveState
        {
            none,
            focus,
        }

        public ChessManager(ListBox MoveHistory, string fen = "11g111i111g1/s1s1s1s1s1s1/111111111111/111111111111/111111111111/111111111111/S1S1S1S1S1S1/11G111I111G1/ouhacmkpahuo/wwwwwwwwwwww/111111111111/111111111111/111111111111/111111111111/WWWWWWWWWWWW/OUHACMKPAHUO/11b111e111b1/1d1d1d1d1d1d/111111111111/111111111111/111111111111/111111111111/1D1D1D1D1D1D/11B111E111B1 w 0 1")
        {
            this.MoveHistory = MoveHistory;
            moveSound = new SoundPlayer();
            moveSound.Stream = Properties.Resources.FigureMove;

            chess = new Chess(fen);

            GameField = new Cell[3, 12, 8];
            for (int z = 0; z < 3; z++)
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 12; x++)
                        GameField[z, x, y] = new Cell(new Button(), "0", "000");

            moveState = MoveState.none;
        }

        public void CheckMoveState(string FigPlusCoord)
        {
            if (moveState == MoveState.none) 
            {
                
                figureFrom = FigPlusCoord.Substring(0, 1);
                if (figureFrom == "." || figureFrom.ToLower() != figureFrom && chess.GetUpdateMoveColor() == false || 
                                         figureFrom.ToLower() == figureFrom && chess.GetUpdateMoveColor() == true)
                    return;
                from = FigPlusCoord.Substring(1, 3);
                foreach (string move in chess.YieldValidMoves())
                    if (from == move.Substring(1, 3))
                        MarkSquare(int.Parse((move[4]).ToString()) - 1,
                                   int.Parse((move[5] - 'a').ToString()),
                                   int.Parse(move[6].ToString()) - 1);
                    
                
                moveState = MoveState.focus;
            }
            else
            {
                UnmarkSquares();
                //ход
                foreach (string move in chess.YieldValidMoves())
                    if (from == move.Substring(1, 3))
                        if (FigPlusCoord.Substring(1, 3) == move.Substring(4, 3))
                        {
                            if (chess.GetUpdateMoveColor())
                                MoveHistory.Items.Insert(0, chess.GetRound() + ". " + move);
                            else
                                MoveHistory.Items[0] = MoveHistory.Items[0].ToString() + "  " + move;
                            chess = chess.Move(move);
                            UpdateBoard(from, move.Substring(4, 3));
                            moveSound.Play();
                        }
                            
                moveState = MoveState.none;
            }     
        }

        private void MarkSquare(int z,int x,int y )
        {
            GameField[z, x ,y].btn.Background = new SolidColorBrush(Colors.Gray);
        }

        private void UnmarkSquares()
        {
            for (int z = 0; z < 3; z++)
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 12; x++)
                        if ((y + x) % 2 != 1 && z == 2)
                            GameField[z, x, y].btn.Background = new SolidColorBrush(Colors.Aqua);
                        else if ((y + x) % 2 != 1 && z == 1)
                            GameField[z, x, y].btn.Background = new SolidColorBrush(Colors.Green);
                        else if ((y + x) % 2 != 1 && z == 0)
                            GameField[z, x, y].btn.Background = new SolidColorBrush(Colors.Red);
                        else
                            GameField[z, x, y].btn.Background = new SolidColorBrush(Colors.White);
        }

        private void UpdateBoard(string from, string to)
        {
            int zFrom, xFrom, yFrom, zTo,xTo, yTo;
            zFrom = int.Parse((from[0]).ToString()) - 1;
            xFrom = int.Parse((from[1] - 'a').ToString());
            yFrom = int.Parse((from[2]).ToString()) - 1;   
            
            zTo = int.Parse((to[0]).ToString()) - 1;
            xTo = int.Parse((to[1] - 'a').ToString());
            yTo = int.Parse((to[2]).ToString()) - 1;
            string figure = chess.GetFigureAt(zFrom, xFrom, yFrom).ToString();
            Image img = new Image();
            if (figure == ".")
            {
                GameField[zFrom, xFrom, yFrom].btn.Content = null;
                
            }
            else
            {
                if (figure == figure.ToLower())
                    img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\Black\" + chess.GetFigureAt(zFrom, xFrom, yFrom) + ".png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\White\" + chess.GetFigureAt(zFrom, xFrom, yFrom) + ".png", UriKind.Relative));
                img.Height = 40;
                img.Width = 40;
                GameField[zFrom, xFrom, yFrom].btn.Content = img;
            }
            GameField[zFrom, xFrom, yFrom].btn.CommandParameter = figure + from;
            GameField[zFrom, xFrom, yFrom].figure = figure;

            figure = chess.GetFigureAt(zTo, xTo, yTo).ToString();
            if (figure == ".")
            {
                GameField[zTo, xTo, yTo].btn.Content = null;
            }
            else
            {

                if (figure == figure.ToLower())
                    img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\Black\" + chess.GetFigureAt(zTo, xTo, yTo) + ".png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri(@"\Date\Images\Icons\Figures\White\" + chess.GetFigureAt(zTo, xTo, yTo) + ".png", UriKind.Relative));
                img.Height = 40;
                img.Width = 40;
                GameField[zTo, xTo, yTo].btn.Content = img;
            }
            GameField[zTo, xTo, yTo].btn.CommandParameter = figure + to;
            GameField[zTo, xTo, yTo].figure = figure;
            
        }

    }
}
