using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DragonChessKurs.Models
{
    internal class Cell
    {
        public Button btn;
        public string figure;
        public string coordinate;

        public Cell(Button btn, string figure, string coordinate)
        {
            this.btn = btn;
            this.figure = figure;
            this.coordinate = coordinate;
        }
    }
}
