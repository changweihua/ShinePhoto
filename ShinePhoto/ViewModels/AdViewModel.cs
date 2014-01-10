using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows;
using ShinePhoto.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Reflection;
using TransitionEffects;
using System.IO;
using System.Windows.Threading;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(AdViewModel))]
    public class AdViewModel : Screen
    {

        /// <summary>
        /// UserControl Loaded 事件
        /// </summary>
        /// <param name="source"></param>
        public void LoadAdv(object source)
        {
            var view = source as ShinePhoto.Views.AdView;

            var list = new List<AdModel> { 
                new AdModel{ PanelName = "Panel1", FilePath = AppDomain.CurrentDomain.BaseDirectory + @"Adv\140140qahhhal61h9ht8lk.jpg.thumb.jpg", Visibility = Visibility.Visible },
                new AdModel{ PanelName = "Panel2", FilePath = AppDomain.CurrentDomain.BaseDirectory + @"Adv\175352n261rlq9wcl0xplf.jpg", Visibility = Visibility.Collapsed },
                new AdModel{ PanelName = "Panel3", FilePath = AppDomain.CurrentDomain.BaseDirectory + @"Adv\QQ截图20131220152412.png", Visibility = Visibility.Collapsed },
                new AdModel{ PanelName = "Panel4", FilePath = AppDomain.CurrentDomain.BaseDirectory + @"Adv\QQ截图20131224130755.png", Visibility = Visibility.Collapsed }
            };

            adImages = list;

            StartEffectAnimation(0, 0, 1, 5);
        }

        private List<AdModel> adImages;

        private TransitionEffect[] effects = new TransitionEffect[] 
        {
            new TransitionEffects.BandedSwirlTransitionEffect(),
            new TransitionEffects.BlindsTransitionEffect(),
            new TransitionEffects.BloodTransitionEffect(),
            new TransitionEffects.CircleRevealTransitionEffect(),
            new TransitionEffects.CircleStretchTransitionEffect(),
            new TransitionEffects.CircularBlurTransitionEffect(),
            new TransitionEffects.CloudRevealTransitionEffect(),
            new TransitionEffects.CrumbleTransitionEffect(),
            new TransitionEffects.DisolveTransitionEffect(),
            new TransitionEffects.DropFadeTransitionEffect(),
            new TransitionEffects.FadeTransitionEffect(),
            new TransitionEffects.LeastBrightTransitionEffect(),
            new TransitionEffects.LineRevealTransitionEffect(),
            new TransitionEffects.MostBrightTransitionEffect(),
            new TransitionEffects.PixelateInTransitionEffect(),
            new TransitionEffects.PixelateOutTransitionEffect(),
            new TransitionEffects.PixelateTransitionEffect(),
            new TransitionEffects.RadialBlurTransitionEffect(),
            new TransitionEffects.RadialWiggleTransitionEffect(),
            new TransitionEffects.RandomCircleRevealTransitionEffect(),
            new TransitionEffects.RippleTransitionEffect(),
            new TransitionEffects.RotateCrumbleTransitionEffect(),
            new TransitionEffects.SaturateTransitionEffect(),
            new TransitionEffects.ShrinkTransitionEffect(),
            new TransitionEffects.SlideInTransitionEffect(),
            new TransitionEffects.SmoothSwirlGridTransitionEffect(),
            new TransitionEffects.SwirlGridTransitionEffect(),
            new TransitionEffects.SwirlTransitionEffect(),
            new TransitionEffects.WaterTransitionEffect(),
            new TransitionEffects.WaveTransitionEffect()
        };

        private DispatcherTimer dt;

        private string[] filefomat = { ".png", ".jpg", ".bmp" };

        private void StartEffectAnimation(int EffectIndex, int PicPindex, int PicNindex, int timespan)
        {
            try
            {
                EffectIndex = EffectIndex > effects.Length - 1 ? 0 : EffectIndex;

                if (PicNindex > adImages.Count - 1)
                {
                    PicPindex = 0;
                    PicNindex = 1;
                }

                FileInfo fi = new FileInfo(adImages[PicPindex].FilePath);

                if (!filefomat.Contains(fi.Extension.ToLower()))
                {
                    PicPindex++;
                    PicNindex++;

                    if (PicNindex > adImages.Count - 1)
                    {
                        PicPindex = 0;
                        PicNindex = 1;
                    }

                    StartEffectAnimation(EffectIndex, PicPindex, PicNindex, timespan);
                    return;
                }

                var view = GetView() as ShinePhoto.Views.AdView;

                TransitionEffect Teffect = effects[EffectIndex];
                Teffect.Progress = 0;
                Teffect.OldImage = new ImageBrush(new BitmapImage(new Uri(adImages[PicPindex].FilePath, UriKind.Relative)));
                Teffect.Input = new ImageBrush(new BitmapImage(new Uri(adImages[PicNindex].FilePath, UriKind.Relative)));
                view.AdvGrid.Effect = Teffect;
                DoubleAnimation ani = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                ani.Completed += delegate
                {
                    dt = new DispatcherTimer();
                    dt.Interval = TimeSpan.FromSeconds(timespan);
                    dt.Tick += delegate
                    {
                        dt.Stop();
                        StartEffectAnimation(++EffectIndex, ++PicPindex, ++PicNindex, timespan);
                    };
                    dt.Start();
                };
                Teffect.BeginAnimation(TransitionEffects.FadeTransitionEffect.ProgressProperty, ani);
            }
            catch
            {
                return;
            }
        }

        #region CTOR

        //[ImportingConstructor]
        public AdViewModel()
        {

        }

        #endregion

        

    }
}
