using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace ShinePhoto.UC
{
    public class DockPanelContainer : Canvas
    {
        private List<RowDefinition> rows = new List<RowDefinition>();
        private List<ColumnDefinition> columns = new List<ColumnDefinition>();
        private DockPanel draggingPanel;
        private bool maximisedMode;
        private double minimisedPanelColumnWidth = 275.0;
        private DockPanel maximisedPanel;
        private int currentZIndex = 1;
        private int maxColumns;
        private int maxRows;
        private bool wiredUp;
        public int MaxColumns
        {
            get
            {
                return this.maxColumns;
            }
            set
            {
                this.maxColumns = value;
                this.columns.Clear();
                for (int i = 0; i < this.maxColumns; i++)
                {
                    List<ColumnDefinition> arg_43_0 = this.columns;
                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = new GridLength(1.0 / (double)this.maxColumns);
                    arg_43_0.Add(columnDefinition);
                }
            }
        }
        public int MaxRows
        {
            get
            {
                return this.maxRows;
            }
            set
            {
                this.maxRows = value;
                this.rows.Clear();
                for (int i = 0; i < this.maxRows; i++)
                {
                    List<RowDefinition> arg_43_0 = this.rows;
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(1.0 / (double)this.maxRows);
                    arg_43_0.Add(rowDefinition);
                }
            }
        }
        private int CurrentZIndex
        {
            get
            {
                this.currentZIndex++;
                return this.currentZIndex;
            }
        }
        public DockPanelContainer()
        {
            this.MaxColumns = 4;
            this.MaxRows = 2;
            Loaded += this.DockPanelContainer_Loaded;
            SizeChanged += this.DockPanelContainer_SizeChanged;
        }
        public void UpdatePanelLayout()
        {
            IEnumerator enumerator = base.Children.GetEnumerator();

            while (enumerator.MoveNext())
            {
                UIElement current = enumerator.Current as UIElement;
                DockPanel dockPanel = (DockPanel)current;
                if (dockPanel != this.draggingPanel)
                {
                    double num = 0.0;
                    for (int i = 0; i < dockPanel.Column; i++)
                    {
                        num += this.columns[i].Width.Value * base.ActualWidth;
                    }
                    double num2 = 0.0;
                    for (int j = 0; j < dockPanel.Row; j++)
                    {
                        num2 += this.rows[j].Height.Value * ActualHeight;
                    }
                    Canvas.SetLeft(dockPanel, num);
                    Canvas.SetTop(dockPanel, num2);
                }
            }

        }
        public void UniformLayout()
        {
            this.draggingPanel = null;
            this.maximisedMode = false;
            using (List<ColumnDefinition>.Enumerator enumerator = this.columns.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ColumnDefinition current = enumerator.Current;
                    current.Width = new GridLength(1.0 / (double)this.maxColumns);
                }
            }
            using (List<RowDefinition>.Enumerator enumerator2 = this.rows.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    RowDefinition current2 = enumerator2.Current;
                    current2.Height = new GridLength(1.0 / (double)this.maxRows);
                }
            }
            using (IEnumerator<UIElement> enumerator3 = base.Children.GetEnumerator() as IEnumerator<UIElement>)
            {
                while (enumerator3.MoveNext())
                {
                    UIElement current3 = enumerator3.Current;
                    DockPanel dockPanel = (DockPanel)current3;
                    dockPanel.DraggingEnabled = true;
                    dockPanel.Maximized = false;
                    dockPanel.Width = this.columns[dockPanel.Column].Width.Value * ActualWidth - dockPanel.Margin.Left - dockPanel.Margin.Right;
                    dockPanel.Height = this.rows[dockPanel.Row].Height.Value * ActualHeight - dockPanel.Margin.Top - dockPanel.Margin.Bottom;
                }
            }
            this.AnimateUpdateLayout();
        }
        public void AnimateUpdateLayout()
        {
            IEnumerator enumerator = Children.GetEnumerator();

            while (enumerator.MoveNext())
            {
                UIElement current = enumerator.Current as UIElement;
                DockPanel dockPanel = (DockPanel)current;
                if (dockPanel != this.draggingPanel)
                {
                    double num = 0.0;
                    for (int i = 0; i < dockPanel.Column; i++)
                    {
                        num += this.columns[i].Width.Value * ActualWidth;
                    }
                    double num2 = 0.0;
                    for (int j = 0; j < dockPanel.Row; j++)
                    {
                        num2 += this.rows[j].Height.Value * ActualHeight;
                    }
                    dockPanel.AnimatePosition(num, num2);
                }
            }

        }
        public void ResetLayout()
        {
            this.maximisedPanel = null;
            this.draggingPanel = null;
            this.maximisedMode = false;
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, 700);
            IEnumerator enumerator = Children.GetEnumerator();

            while (enumerator.MoveNext())
            {
                UIElement current = enumerator.Current as UIElement;
                DockPanel dockPanel = (DockPanel)current;
                dockPanel.DraggingEnabled = true;
                dockPanel.Maximized = false;
                double num = 0.0;
                for (int i = 0; i < dockPanel.Column; i++)
                {
                    num += this.columns[i].Width.Value * ActualWidth;
                }
                double num2 = 0.0;
                for (int j = 0; j < dockPanel.Row; j++)
                {
                    num2 += this.rows[j].Height.Value * ActualHeight;
                }
                dockPanel.PositionAnimationDuration = timeSpan;
                dockPanel.SizeAnimationDuration = timeSpan;
                dockPanel.AnimateSize(this.columns[dockPanel.Column].Width.Value * ActualWidth * (double)dockPanel.ColumnSpan - dockPanel.Margin.Left - dockPanel.Margin.Right, this.rows[dockPanel.Row].Height.Value * ActualHeight * (double)dockPanel.RowSpan - dockPanel.Margin.Top - dockPanel.Margin.Bottom);
                dockPanel.AnimatePosition(num, num2);
            }

        }
        private void DockPanelContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.maximisedPanel == null)
            {
                if (this.rows.Count > 0 && this.columns.Count > 0)
                {
                    IEnumerator enumerator = base.Children.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        UIElement current = enumerator.Current as UIElement;
                        DockPanel dockPanel = (DockPanel)current;
                        dockPanel.Maximized = false;
                        double width = this.columns[dockPanel.Column].Width.Value * ActualWidth * (double)dockPanel.ColumnSpan - dockPanel.Margin.Left - dockPanel.Margin.Right;
                        double height = this.rows[dockPanel.Row].Height.Value * ActualHeight * (double)dockPanel.RowSpan - dockPanel.Margin.Top - dockPanel.Margin.Bottom;
                        dockPanel.Width = width;
                        dockPanel.Height = height;
                    }

                    this.UpdatePanelLayout();
                }
            }
            else
            {
                this.maximisedPanel.Width = ActualWidth - this.minimisedPanelColumnWidth - this.maximisedPanel.Margin.Left - this.maximisedPanel.Margin.Right;
                this.maximisedPanel.Height = ActualHeight - this.maximisedPanel.Margin.Top - this.maximisedPanel.Margin.Bottom;
                double num = 0.0;
                IEnumerator enumerator = Children.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    UIElement current2 = enumerator.Current as UIElement;
                    DockPanel dockPanel2 = (DockPanel)current2;
                    if (dockPanel2 != this.maximisedPanel)
                    {
                        dockPanel2.Maximized = false;
                        dockPanel2.DraggingEnabled = false;
                        Canvas.SetLeft(dockPanel2, ActualWidth - this.minimisedPanelColumnWidth + dockPanel2.Margin.Left);
                        Canvas.SetTop(dockPanel2, num + dockPanel2.Margin.Top);
                        num += ActualHeight / (double)(Children.Count - 1);
                        dockPanel2.Width = this.minimisedPanelColumnWidth - dockPanel2.Margin.Left - dockPanel2.Margin.Right;
                        dockPanel2.Height = ActualHeight / (double)(Children.Count - 1) - dockPanel2.Margin.Top - dockPanel2.Margin.Bottom;
                    }
                }

            }
        }
        private void DockPanelContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.wiredUp)
            {
                this.WireUpChildren();
                this.wiredUp = true;
            }
            this.UpdatePanelLayout();
        }
        private void WireUpChildren()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.rows.Count * this.columns.Count; i++)
            {
                list.Add(i);
            }
            IEnumerator enumerator = Children.GetEnumerator();

            while (enumerator.MoveNext())
            {
                UIElement current = enumerator.Current as UIElement;
                if (current.GetType() != typeof(DockPanel))
                {
                    break;
                }
                DockPanel dockPanel = (DockPanel)current;
                if (list.Count == 0)
                {
                    dockPanel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    dockPanel.Row = list[0] / this.columns.Count;
                    dockPanel.Column = list[0] - dockPanel.Row * this.columns.Count;
                    for (int j = dockPanel.RowSpan; j > 0; j--)
                    {
                        for (int k = 0; k < dockPanel.ColumnSpan; k++)
                        {
                            list.Remove(list[(j - 1) * this.columns.Count]);
                        }
                    }
                    dockPanel.PanelDragStarted += new EventHandler<DragEventArgs>(this.DockPanelContainer_PanelDragStarted);
                    dockPanel.PanelDragMoved += new EventHandler<DragEventArgs>(this.DockPanelContainer_PanelDragMoved);
                    dockPanel.PanelDropped += new EventHandler<DragEventArgs>(this.DockPanelContainer_PanelDropped);
                    dockPanel.PanelMaximized += new EventHandler(this.DockPanelContainer_PanelMaximised);
                    dockPanel.PanelMinimized += new EventHandler(this.DockPanelContainer_PanelMinimised);
                }
            }

        }
        private void DockPanelContainer_PanelMinimised(object sender, EventArgs e)
        {
            this.ResetLayout();
        }
        private void DockPanelContainer_PanelMaximised(object sender, EventArgs e)
        {
            DockPanel dockPanel = (DockPanel)sender;
            this.maximisedPanel = dockPanel;
            Canvas.SetZIndex(dockPanel, this.CurrentZIndex);
            dockPanel.PositionAnimationDuration = new TimeSpan(0, 0, 0, 0, 700);
            dockPanel.AnimatePosition(dockPanel.Margin.Left, dockPanel.Margin.Top);
            dockPanel.SizeAnimationDuration = new TimeSpan(0, 0, 0, 0, 700);
            dockPanel.AnimateSize(ActualWidth - this.minimisedPanelColumnWidth - dockPanel.Margin.Left - dockPanel.Margin.Right, ActualHeight - dockPanel.Margin.Top - dockPanel.Margin.Bottom);
            dockPanel.DraggingEnabled = false;
            double num = 0.0;
            IEnumerator enumerator = Children.GetEnumerator();

            while (enumerator.MoveNext())
            {
                UIElement current = enumerator.Current as UIElement;
                DockPanel dockPanel2 = (DockPanel)current;
                if (dockPanel2 != dockPanel)
                {
                    dockPanel2.Maximized = false;
                    dockPanel2.DraggingEnabled = false;
                    dockPanel2.PositionAnimationDuration = new TimeSpan(0, 0, 0, 0, 700);
                    dockPanel2.AnimatePosition(ActualWidth - this.minimisedPanelColumnWidth + dockPanel2.Margin.Left, num + dockPanel2.Margin.Top);
                    num += ActualHeight / (double)(Children.Count - 1);
                    dockPanel2.SizeAnimationDuration = new TimeSpan(0, 0, 0, 0, 700);
                    dockPanel2.AnimateSize(this.minimisedPanelColumnWidth - dockPanel2.Margin.Left - dockPanel2.Margin.Right, ActualHeight / (double)(Children.Count - 1) - dockPanel2.Margin.Top - dockPanel2.Margin.Bottom);
                }
            }

        }
        private void DockPanelContainer_PanelDropped(object sender, DragEventArgs args)
        {
            this.draggingPanel = null;
            this.UpdatePanelLayout();
        }
        private void DockPanelContainer_PanelDragMoved(object sender, DragEventArgs args)
        {
            Point position = args.MouseEventArgs.GetPosition(this);
            DockPanel dockPanel = (DockPanel)sender;
            if (!this.maximisedMode)
            {
                DockPanel dockPanel2 = null;
                int num = -1;
                int num2 = -1;
                for (int i = 0; i < Children.Count; i++)
                {
                    DockPanel dockPanel3 = (DockPanel)Children[i];
                    double num3 = 0.0;
                    for (int j = 0; j < dockPanel3.Column; j++)
                    {
                        num3 += this.columns[j].Width.Value * ActualWidth;
                    }
                    double num4 = 0.0;
                    for (int k = 0; k < dockPanel3.Row; k++)
                    {
                        num4 += this.rows[k].Height.Value * ActualHeight;
                    }
                    if (position.X >= num3 && position.X < num3 + this.columns[dockPanel3.Column].Width.Value * ActualWidth && position.Y >= num4 && position.Y < num4 + this.rows[dockPanel3.Row].Height.Value * ActualHeight)
                    {
                        num = dockPanel3.Column;
                        num2 = dockPanel3.Row;
                        if (dockPanel3 != dockPanel)
                        {
                            dockPanel2 = dockPanel3;
                        }
                    }
                }
                if (dockPanel2 != null)
                {
                    List<int> list = new List<int>();
                    if (num + dockPanel.ColumnSpan >= this.columns.Count)
                    {
                        num = this.columns.Count - dockPanel.ColumnSpan;
                    }
                    for (int j = num; j < num + dockPanel.ColumnSpan; j++)
                    {
                        list.Add(j);
                    }
                    List<int> list2 = new List<int>();
                    if (num2 + dockPanel.RowSpan >= this.rows.Count)
                    {
                        num2 = this.rows.Count - dockPanel.RowSpan;
                    }
                    for (int k = num2; k < num2 + dockPanel.RowSpan; k++)
                    {
                        list2.Add(k);
                    }
                    List<GridCell> list3 = new List<GridCell>();
                    for (int k = dockPanel.Row; k < dockPanel.Row + dockPanel.RowSpan; k++)
                    {
                        for (int j = dockPanel.Column; j < dockPanel.Column + dockPanel.ColumnSpan; j++)
                        {
                            list3.Add(new GridCell(j, k));
                        }
                    }
                    bool flag = false;
                    for (int i = 0; i < Children.Count; i++)
                    {
                        DockPanel dockPanel3 = (DockPanel)Children[i];
                        if (list2.Contains(dockPanel3.Row) && list.Contains(dockPanel3.Column) && dockPanel3.ColumnSpan <= dockPanel.ColumnSpan && dockPanel3.RowSpan <= dockPanel.RowSpan)
                        {
                            GridCell gridCell = list3[0];
                            dockPanel3.Column = gridCell.Column;
                            dockPanel3.Row = gridCell.Row;
                            list3.Remove(gridCell);
                            flag = true;
                            dockPanel3.PositionAnimationDuration = new TimeSpan(0, 0, 0, 0, 200);
                        }
                    }
                    if (flag)
                    {
                        dockPanel.Column = num;
                        dockPanel.Row = num2;
                    }
                    this.AnimateUpdateLayout();
                }
            }
        }
        private void DockPanelContainer_PanelDragStarted(object sender, DragEventArgs args)
        {
            DockPanel dockPanel = (DockPanel)sender;
            this.draggingPanel = dockPanel;
            Canvas.SetZIndex(dockPanel, this.CurrentZIndex);
        }
    }
}
