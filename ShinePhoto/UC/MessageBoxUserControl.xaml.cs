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
    /// MessageBoxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxUserControl : UserControl
    {
        public MessageBoxUserControl()
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
