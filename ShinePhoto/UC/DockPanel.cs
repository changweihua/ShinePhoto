using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;

namespace ShinePhoto.UC
{
    [DefaultProperty("DockContent"), Localizability(LocalizationCategory.None, Readability = Readability.Unreadable), ContentProperty("DockContent")]
    public partial class DockPanel : AnimatedUserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(ControlTemplate), typeof(DockPanel), new PropertyMetadata(new PropertyChangedCallback(DockPanel.HeaderChanged)));
        private bool isDragging;
        private bool draggingEnabled = true;
        private double gripBarMousePos;
        private UIElement content;
        private int column;
        private int row;
        private int columnSpan = 1;
        private int rowSpan = 1;
        private string subHeader = "";
        private bool isMaximised;
        private Brush buttonForeground = new SolidColorBrush(Color.FromArgb(255, 118, 118, 118));
        //internal RectangleGeometry panelClip;
        //internal Grid lightBackground;
        //internal Rectangle shadow;
        //internal GradientStop shadowVertical1;
        //internal GradientStop shadowVertical2;
        //internal GradientStop shadowHorizontal1;
        //internal GradientStop shadowHorizontal2;
        //internal Grid darkBackground;
        //internal Thumb gripBar;
        //internal DataTemplateBase headerPresenter;
        //internal Grid contentHost;
        //internal RectangleGeometry contentHostClip;
        //internal NhsButton minimiseButton;
        //internal NhsButton maximiseButton;
        //private bool _contentLoaded;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DragEventArgs> PanelDragStarted;
        public event EventHandler<DragEventArgs> PanelDragMoved;
        public event EventHandler<DragEventArgs> PanelDropped;
        public event EventHandler PanelMaximized;
        public event EventHandler PanelMinimized;
        public new UIElement DockContent
        {
            get
            {
                return this.content;
            }
            set
            {
                this.contentHost.Children.Clear();
                this.content = value;
                this.contentHost.Children.Add(this.content);
            }
        }
        public int Column
        {
            get
            {
                return this.column;
            }
            set
            {
                this.column = value;
            }
        }
        public int Row
        {
            get
            {
                return this.row;
            }
            set
            {
                this.row = value;
            }
        }
        public int ColumnSpan
        {
            get
            {
                return this.columnSpan;
            }
            set
            {
                if (value == 0)
                {
                    this.columnSpan = 1;
                }
                else
                {
                    this.columnSpan = value;
                }
            }
        }
        public int RowSpan
        {
            get
            {
                return this.rowSpan;
            }
            set
            {
                if (value == 0)
                {
                    this.rowSpan = 1;
                }
                else
                {
                    this.rowSpan = value;
                }
            }
        }
        public DataTemplateBase HeaderPresenter
        {
            get
            {
                return this.headerPresenter;
            }
        }
        public ControlTemplate HeaderTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(DockPanel.HeaderTemplateProperty);
            }
            set
            {
                this.SetValue(DockPanel.HeaderTemplateProperty, value);
            }
        }
        public string SubHeader
        {
            get
            {
                return this.subHeader;
            }
            set
            {
                this.subHeader = value;
                this.RaisePropertyChanged("SubHeader");
            }
        }
        public bool DraggingEnabled
        {
            get
            {
                return this.draggingEnabled;
            }
            set
            {
                this.draggingEnabled = value;
                if (this.draggingEnabled)
                {
                    this.gripBar.IsHitTestVisible = true;
                }
                else
                {
                    this.gripBar.IsHitTestVisible = false;
                }
            }
        }
        public bool Maximized
        {
            get
            {
                return this.isMaximised;
            }
            set
            {
                this.isMaximised = value;
                if (this.isMaximised)
                {
                    this.maximiseButton.Visibility = System.Windows.Visibility.Hidden;
                    this.minimiseButton.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.maximiseButton.Visibility = System.Windows.Visibility.Visible;
                    this.minimiseButton.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }
        public Brush ButtonForeground
        {
            get
            {
                return this.buttonForeground;
            }
            set
            {
                this.buttonForeground = value;
                this.minimiseButton.Foreground = this.buttonForeground;
                this.maximiseButton.Foreground = this.buttonForeground;
            }
        }
        public DockPanel()
        {
            this.InitializeComponent();
            base.SizeChanged += this.DockPanel_SizeChanged;
            this.gripBar.DragDelta += GripBar_DragDelta;
            this.gripBar.DragStarted += new EventHandler<DragEventArgs>(this.GripBar_DragStarted);
            this.gripBar.DragFinished += new EventHandler<DragEventArgs>(this.GripBar_DragFinished);
            this.minimiseButton.Clicked += new EventHandler(this.MinimiseButton_Clicked);
            this.maximiseButton.Clicked += new EventHandler(this.MaximiseButton_Clicked);

        }

        //static DockPanel()
        //{
        //    ContentProperty.OverrideMetadata(typeof(DockPanel), new FrameworkPropertyMetadata(OnContentPropertyChanged));
        //}

        //private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DockPanel panel = d as DockPanel;
        //    if (panel == null|| panel.contentHost == null) return;
        //    panel.contentHost.Children.Clear();
        //    panel.content = e.NewValue as UIElement;
        //    panel.Content = null;
        //    panel.contentHost.Children.Add(panel.content);

        //}

        public static void HeaderChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            DockPanel dockPanel = (DockPanel)dependencyObject;
            dockPanel.HeaderPresenter.Template = (ControlTemplate)dependencyPropertyChangedEventArgs.NewValue;
        }
        private void GripBar_DragFinished(object sender, DragEventArgs args)
        {
            if (this.draggingEnabled)
            {
                this.isDragging = false;
                this.shadow.Opacity = 0.5;
                if (this.PanelDropped != null)
                {
                    this.PanelDropped.Invoke(this, args);
                }
            }
        }
        private void GripBar_DragStarted(object sender, DragEventArgs args)
        {
            if (this.draggingEnabled)
            {
                this.isDragging = true;
                this.gripBarMousePos = args.MouseEventArgs.GetPosition(this).X / base.Width;
                this.shadow.Opacity = 1.0;
                if (this.PanelDragStarted != null)
                {
                    this.PanelDragStarted.Invoke(this, args);
                }
            }
        }
        private void GripBar_DragDelta(object sender, DragEventArgs args)
        {
            if (this.draggingEnabled)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + args.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + args.VerticalChange);
                if (this.PanelDragMoved != null)
                {
                    this.PanelDragMoved.Invoke(this, args);
                }
            }
        }
        private void MaximiseButton_Clicked(object sender, EventArgs e)
        {
            this.Maximized = true;
            if (this.PanelMaximized != null)
            {
                this.PanelMaximized.Invoke(this, e);
            }
        }
        private void MinimiseButton_Clicked(object sender, EventArgs e)
        {
            this.Maximized = false;
            if (this.PanelMinimized != null)
            {
                this.PanelMinimized.Invoke(this, e);
            }
        }
        private void DockPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.contentHostClip.Rect = new Rect(0.0, -30.0, ActualWidth - 2.0, ActualHeight + 28.0);
            this.panelClip.Rect = new Rect(-8.0, -8.0, ActualWidth + 16.0, ActualHeight + 16.0);
            if (e.NewSize.Width > 0.0 && e.NewSize.Height > 0.0)
            {
                this.shadowHorizontal1.Offset = 300.0 / e.NewSize.Width * 0.1;
                this.shadowHorizontal2.Offset = 1.0 - 300.0 / e.NewSize.Width * 0.1;
                this.shadowVertical1.Offset = 300.0 / e.NewSize.Height * 0.1;
                this.shadowVertical2.Offset = 1.0 - 300.0 / e.NewSize.Height * 0.1;
            }
            if (this.isDragging)
            {
                this.gripBar.AdjustDownPosition(new Point((base.ActualWidth - e.PreviousSize.Width) * this.gripBarMousePos, 0.0));
            }
        }
        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
        //[DebuggerNonUserCode]
        //public void InitializeComponent()
        //{
        //    if (!this._contentLoaded)
        //    {
        //        this._contentLoaded = true;
        //        Application.LoadComponent(this, new Uri("/WpfApplication34;component/DockPanel.xaml", UriKind.RelativeOrAbsolute));
        //        this.panelClip = (RectangleGeometry)base.FindName("panelClip");
        //        this.lightBackground = (Grid)base.FindName("lightBackground");
        //        this.shadow = (Rectangle)base.FindName("shadow");
        //        this.shadowVertical1 = (GradientStop)base.FindName("shadowVertical1");
        //        this.shadowVertical2 = (GradientStop)base.FindName("shadowVertical2");
        //        this.shadowHorizontal1 = (GradientStop)base.FindName("shadowHorizontal1");
        //        this.shadowHorizontal2 = (GradientStop)base.FindName("shadowHorizontal2");
        //        this.darkBackground = (Grid)base.FindName("darkBackground");
        //        this.gripBar = (Thumb)base.FindName("gripBar");
        //        this.headerPresenter = (DataTemplateBase)base.FindName("headerPresenter");
        //        this.contentHost = (Grid)base.FindName("contentHost");
        //        this.contentHostClip = (RectangleGeometry)base.FindName("contentHostClip");
        //        this.minimiseButton = (NhsButton)base.FindName("minimiseButton");
        //        this.maximiseButton = (NhsButton)base.FindName("maximiseButton");
        //    }
        //}
    }
}
