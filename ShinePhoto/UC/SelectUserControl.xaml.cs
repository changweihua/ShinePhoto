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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ShinePhoto.UC
{
    /// <summary>
    /// SelectUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectUserControl : UserControl
    {
        private const string FileIcon = "pack://application:,,,/ShinePhoto.Icons;Component/light/appbar.folder.png";
        private const string FolderIcon = "pack://application:,,,/ShinePhoto.Icons;Component/light/appbar.page.add.png";

        public SelectUserControl()
        {
            InitializeComponent();
        }

        #region 依赖项属性

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(SelectUserControl), new UIPropertyMetadata("*"));

        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register("SelectedPath", typeof(string), typeof(SelectUserControl), new UIPropertyMetadata(""));

        public string LabelContent
        {
            get { return (string)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }

        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(string), typeof(SelectUserControl), new UIPropertyMetadata("LabelContent"));

        public string IconPath
        {
            get { return (string)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.Register("IconPath", typeof(string), typeof(SelectUserControl), new UIPropertyMetadata(FileIcon));

        public bool IsChooseFile
        {
            get { return (bool)GetValue(IsChooseFileProperty); }
            set { SetValue(IsChooseFileProperty, value); }
        }

        public static readonly DependencyProperty IsChooseFileProperty =
            DependencyProperty.Register("IsChooseFile", typeof(bool), typeof(SelectUserControl), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnIsChooseFileChanged)));

        private static void OnIsChooseFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = new SelectUserControl();
            if ((bool)e.NewValue)
            {
                uc.IconPath = SelectUserControl.FileIcon;
            }
            else
            {
                uc.IconPath = SelectUserControl.FolderIcon;
            }
        }


        #endregion

    
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();
            //commonOpenFileDialog.InitialDirectoryShellContainer = "";
            commonOpenFileDialog.EnsureReadOnly = true;

            if (IsChooseFile)
            {
                commonOpenFileDialog.IsFolderPicker = false;
            }
            else
            {
                commonOpenFileDialog.IsFolderPicker = true;
            }

            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SelectedPath = commonOpenFileDialog.FileName;
            }  
        }
 

    }
}
