using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShinePhoto.UC
{
    /// <summary>
    /// FlatUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class FlatUserControl : UserControl
    {
        // private static DependencyProperty ParentWindowProperty; 
 
        ////The parent window 
        //public Window ParentWindow 
        //{ 
        //    set { SetValue(ParentWindowProperty, value);} 
        //    get { return (Window)GetValue(ParentWindowProperty); } 
        //}

        //static FlatUserControl() 
        //{ 
        //    FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
        //    ParentWindowProperty = DependencyProperty.Register("ParentWindow", typeof(Window), typeof(FlatUserControl), metadata, null); 
        //}

        public FlatUserControl()
        {
            InitializeComponent();
        }

        private void TitleGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Window window = Window.GetWindow(this);
                window.DragMove();
            }
        }

        private void CloseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Close();
        }
    }
}
