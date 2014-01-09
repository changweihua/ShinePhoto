using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ShinePhoto.UC
{
    public class ImageButton : Button
    {
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(ImageButton));
    }

    public class TriImageButton : Button
    {
        public string TriImage
        {
            get { return (string)GetValue(TriImageProperty); }
            set { SetValue(TriImageProperty, value); }
        }

        public CroppedBitmap NormalImage
        {
            get { return (CroppedBitmap)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public CroppedBitmap HoverImage
        {
            get { return (CroppedBitmap)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public CroppedBitmap PressImage
        {
            get { return (CroppedBitmap)GetValue(PressImageProperty); }
            set { SetValue(PressImageProperty, value); }
        }

        public static void OnTriImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TriImageButton button = d as TriImageButton;
            var triimage = new BitmapImage(new Uri(e.NewValue as string, UriKind.RelativeOrAbsolute)) { BaseUri = BaseUriHelper.GetBaseUri(button) };
            var frame = BitmapFrame.Create(triimage);
            button.NormalImage = new CroppedBitmap(triimage, new Int32Rect(0, 0, (int)frame.PixelWidth / 3, (int)frame.PixelHeight));
            button.HoverImage = new CroppedBitmap(triimage, new Int32Rect((int)frame.PixelWidth / 3, 0, (int)frame.PixelWidth / 3, (int)frame.PixelHeight));
            button.PressImage = new CroppedBitmap(triimage, new Int32Rect((int)frame.PixelWidth / 3 * 2, 0, (int)frame.PixelWidth / 3, (int)frame.PixelHeight));
        }

        public static readonly DependencyProperty TriImageProperty =
            DependencyProperty.Register("TriImage", typeof(string), typeof(TriImageButton), new PropertyMetadata(new PropertyChangedCallback(OnTriImageChanged)));

        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(CroppedBitmap), typeof(TriImageButton));

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register("HoverImage", typeof(CroppedBitmap), typeof(TriImageButton));

        public static readonly DependencyProperty PressImageProperty =
            DependencyProperty.Register("PressImage", typeof(CroppedBitmap), typeof(TriImageButton));
    }

}
