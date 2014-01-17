using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinePhoto.UC
{
    public class GridCell
    {
        public int Column
        {
            get;
            set;
        }
        public int Row
        {
            get;
            set;
        }
        public GridCell(int column, int row)
        {
            this.Column = column;
            this.Row = row;
        }
    }
}
