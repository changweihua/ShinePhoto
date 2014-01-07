using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Interfaces;
using ShinePhoto.Interface;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(IntroductionViewModel))]
    public class IntroductionViewModel : Screen, IShellView
    {

        public void InitView(object source)
        {
            var view = source as ShinePhoto.Views.IntroductionView;

            TextBlock tbName = null;
            if (view != null)
            {
                tbName = ShinePhoto.Helpers.TreeHelper.FindVisualChildByName<TextBlock>(view, "tbName");
                SwipeWords(tbName, view);
            }
           
        }

        private void SwipeWords(TextBlock tb, FrameworkElement container)
        {
            Storyboard perChar = new Storyboard();
            tb.TextEffects = new TextEffectCollection();
            for (int i = 0; i < tb.Text.Length; i++)
            {
                TextEffect effect = new TextEffect();
                effect.Transform = new TranslateTransform();
                effect.PositionStart = i;
                effect.PositionCount = 1;
                //effect.Foreground = Brushes.Red;
                tb.TextEffects.Add(effect);
                DoubleAnimation anim = new DoubleAnimation();
                anim.To = 5;
                anim.AccelerationRatio = 0.3;
                anim.DecelerationRatio = 0.3;
                anim.AutoReverse = true;
                anim.Duration = TimeSpan.FromSeconds(1);
                anim.BeginTime = TimeSpan.FromMilliseconds(250 * i);
                anim.RepeatBehavior = RepeatBehavior.Forever;
                Storyboard.SetTargetProperty(anim, new PropertyPath("TextEffects[" + i + "].Transform.Y"));
                Storyboard.SetTargetName(anim, tb.Name);
                perChar.Children.Add(anim);
            }
            perChar.Begin(container);
        }

    }
}
