using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using WPFSpark;
using System.Windows.Media;
using System.Windows.Controls;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(SystemConfigViewModel))]
    public class SystemConfigViewModel : Screen
    {
        public void LoadUserControl(object source)
        {
            var view = source as ShinePhoto.Views.SystemConfigView;

            if (view != null)
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {

                        ObservableCollection<PivotItem> items = new ObservableCollection<PivotItem>();

                        string[] colors = new string[] { "first", "second", "third", "fourth" };
                        Brush[] brushes = new Brush[] { Brushes.Red, Brushes.LawnGreen, Brushes.Blue, Brushes.Orange, Brushes.Cyan };
                        List<List<TextMessage>> data = new List<List<TextMessage>>();
                        data.Add(new List<TextMessage>() { new TextMessage { MainText="design one"     ,SubText="Lorem ipsum dolor sit amet" },   
								               new TextMessage { MainText="design two"     ,SubText="consectetur adipisicing elit" },   
								               new TextMessage { MainText="design three"   ,SubText="sed do eiusmod tempor incididunt" },   
								               new TextMessage { MainText="design four"    ,SubText="ut labore et dolore magna aliqua" },   
								               new TextMessage { MainText="design five"    ,SubText="Ut enim ad minim veniam" },   
								               new TextMessage { MainText="design six"     ,SubText="quis nostrud exercitation ullamco laboris" },   
								               new TextMessage { MainText="design seven"   ,SubText="nisi ut aliquip ex ea commodo consequat" } });
                        data.Add(new List<TextMessage>() { new TextMessage { MainText="runtime one"    ,SubText="Lorem ipsum dolor sit amet" },   
								               new TextMessage { MainText="runtime two"    ,SubText="consectetur adipisicing elit" },   
								               new TextMessage { MainText="runtime three"  ,SubText="sed do eiusmod tempor incididunt" },   
								               new TextMessage { MainText="runtime four"   ,SubText="ut labore et dolore magna aliqua" },   
								               new TextMessage { MainText="runtime five"   ,SubText="Ut enim ad minim veniam" },   
								               new TextMessage { MainText="runtime six"    ,SubText="quis nostrud exercitation ullamco laboris" }, 
								               new TextMessage { MainText="runtime seven"  ,SubText="nisi ut aliquip ex ea commodo consequat" } });
                        data.Add(new List<TextMessage>() { new TextMessage { MainText="method one"     ,SubText="Lorem ipsum dolor sit amet" },   
								               new TextMessage { MainText="method two"     ,SubText="consectetur adipisicing elit" },   
								               new TextMessage { MainText="method three"   ,SubText="sed do eiusmod tempor incididunt" },   
								               new TextMessage { MainText="method four"    ,SubText="ut labore et dolore magna aliqua" },   
								               new TextMessage { MainText="method five"    ,SubText="Ut enim ad minim veniam" },   
								               new TextMessage { MainText="method six"     ,SubText="quis nostrud exercitation ullamco laboris" }, 
								               new TextMessage { MainText="method seven"   ,SubText="nisi ut aliquip ex ea commodo consequat" } });
                        data.Add(new List<TextMessage>() { new TextMessage { MainText="solution one"   ,SubText="Lorem ipsum dolor sit amet" },      
								               new TextMessage { MainText="solution two"   ,SubText="consectetur adipisicing elit" },    
								               new TextMessage { MainText="solution three" ,SubText="sed do eiusmod tempor incididunt" },    
								               new TextMessage { MainText="solution four"  ,SubText="ut labore et dolore magna aliqua" },    
								               new TextMessage { MainText="solution five"  ,SubText="Ut enim ad minim veniam" },    
								               new TextMessage { MainText="solution six"   ,SubText="quis nostrud exercitation ullamco laboris" },  
								               new TextMessage { MainText="solution seven" ,SubText="nisi ut aliquip ex ea commodo consequat" } });

                        for (int i = 0; i < colors.Count(); i++)
                        {
                            PivotHeaderControl tb = new PivotHeaderControl();
                            tb.FontFamily = new FontFamily("Segoe WP");
                            tb.FontWeight = FontWeights.Light;
                            tb.ActiveForeground = Brushes.White;
                            tb.InactiveForeground = new SolidColorBrush(Color.FromRgb(48, 48, 48));
                            tb.FontSize = 64;
                            tb.Content = colors[i];
                            tb.Margin = new Thickness(20, 0, 0, 0);

                            PivotContentControl pci = new PivotContentControl();
                            ListBox lb = new ListBox()
                            {
                                FontFamily = new FontFamily("Segoe WP"),
                                FontSize = 32,
                                FontWeight = FontWeights.Light,
                                Foreground = Brushes.Gray,
                                Background = Brushes.Black,
                                BorderThickness = new Thickness(0),
                            };
                            lb.ItemTemplate = (DataTemplate)view.Resources["ListBoxItemTemplate"];
                            lb.ItemsSource = data[i];
                            ScrollViewer.SetHorizontalScrollBarVisibility(lb, ScrollBarVisibility.Disabled);
                            lb.HorizontalAlignment = HorizontalAlignment.Stretch;
                            lb.VerticalAlignment = VerticalAlignment.Stretch;
                            lb.Margin = new Thickness(30, 10, 10, 10);
                            pci.Content = lb;

                            PivotItem pi = new PivotItem { PivotHeader = tb, PivotContent = pci };
                            //pi.SetActive(false);
                            items.Add(pi);
                        }

                        view.RootPivotPanel.ItemsSource = items;

                        view.RootPivotPanel.Background = Brushes.Black;
                    });
                }
                catch (Exception ex)
                {
                    LogManager.GetLog(typeof(SystemConfigViewModel)).Info(ex.Message);
                }

                
            }
        }

        #region 标题栏操作

        public void MoveWindow(object sender, MouseButtonEventArgs e, object view)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                (view as Window).DragMove();
            }
        }

        public void CloseWindow(object view)
        {
            if ((view as Window) != null)
            {
                (view as Window).Close();
            }
        }

        #endregion
    }

    public class TextMessage : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region MainText

        private string mainText = string.Empty;

        /// <summary>
        /// Gets or sets the MainText property. This observable property 
        /// indicates the main text.
        /// </summary>
        public string MainText
        {
            get { return mainText; }
            set
            {
                if (mainText != value)
                {
                    mainText = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("MainText"));
                }
            }
        }

        #endregion

        #region SubText

        private string subText = string.Empty;

        /// <summary>
        /// Gets or sets the SubText property. This observable property 
        /// indicates the sub text.
        /// </summary>
        public string SubText
        {
            get { return subText; }
            set
            {
                if (subText != value)
                {
                    subText = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SubText"));
                }
            }
        }

        #endregion
    }
}
