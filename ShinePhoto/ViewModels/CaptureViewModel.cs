using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Models;
using System.Windows.Media.Imaging;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(CaptureViewModel))]
    public class CaptureViewModel : PropertyChangedBase, IShellView
    {
        public BindableCollection<FileModel> FileModels { get; private set; }

        public CaptureViewModel()
        {
            FileModels = new BindableCollection<FileModel>();
            
            double width, height;

            var arr = System.IO.Directory.GetFiles(@"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13");
            
            for (int i = 0; i < arr.Length; i++)
            {
                GetWH(arr[i], out width, out height);
                FileModels.Add(new FileModel { FileName = arr[i], Height = height, Width = width });
            }
        }

        void GetWH(string path, out double width, out double height)
        {
            var bitmap = BitmapFrame.Create(new Uri(path, UriKind.RelativeOrAbsolute));
            double w = bitmap.Width;
            double h = bitmap.Height;

            double doa = w / h;

            width = doa * 158;
            height = 158;
        }

        private string _currentImageSource;

        public string CurrentImageSource
        {
            get { return _currentImageSource; }
            set {
                _currentImageSource = value;
                NotifyOfPropertyChange(() => CurrentImageSource);
            }
        }

    }
}
