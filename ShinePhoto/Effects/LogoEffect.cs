using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;

namespace ShinePhoto.Effects
{
    #region 作者和版权
    /*************************************************************************************
     * CLR 版本:       4.0.30319.18052
     * 类 名 称:       LogoEffect
     * 机器名称:       LUMIA800
     * 命名空间:       ShinePhoto.Effects
     * 文 件 名:       LogoEffect
     * 创建时间:       2014/1/11 13:12:42
     * 作    者:       常伟华 Changweihua
	 * 版    权:	   LogoEffect说明：本代码版权归常伟华所有，使用时必须带上常伟华网站地址 All Rights Reserved (C) 2014 - 2013
     * 签    名:       To be or not, it is not a problem !
     * 网    站:       http://www.cmono.net
     * 邮    箱:       changweihua@outlook.com  
     * 唯一标识：	   d58469af-eece-4c91-91e8-77ecbdb17873  
	 *
	 * 登录用户:       Changweihua
	 * 所 属 域:       Lumia800

	 * 创建年份:       2014
     * 修改时间:
     * 修 改 人:
     * 
     ************************************************************************************/
    #endregion

    /// <summary>
    /// 摘要
    /// </summary>
    public class LogoEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(LogoEffect), 0);
        public static readonly DependencyProperty RedThresholdProperty = DependencyProperty.Register("RedThreshold", typeof(double), typeof(LogoEffect), new UIPropertyMetadata(0.0, ShaderEffect.PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty SunAngleProperty = DependencyProperty.Register("SunAngle", typeof(double), typeof(LogoEffect), new UIPropertyMetadata(0.0, ShaderEffect.PixelShaderConstantCallback(1)));
        public Brush Input
        {
            get
            {
                return (Brush)base.GetValue(LogoEffect.InputProperty);
            }
            set
            {
                base.SetValue(LogoEffect.InputProperty, value);
            }
        }
        public double RedThreshold
        {
            get
            {
                return (double)base.GetValue(LogoEffect.RedThresholdProperty);
            }
            set
            {
                base.SetValue(LogoEffect.RedThresholdProperty, value);
            }
        }
        public double SunAngle
        {
            get
            {
                return (double)base.GetValue(LogoEffect.SunAngleProperty);
            }
            set
            {
                base.SetValue(LogoEffect.SunAngleProperty, value);
            }
        }
        public LogoEffect()
        {
            base.PixelShader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/ShinePhoto;Component/Res/LogoEffect.ps", UriKind.RelativeOrAbsolute)
            };
            base.UpdateShaderValue(LogoEffect.InputProperty);
            base.UpdateShaderValue(LogoEffect.RedThresholdProperty);
            base.UpdateShaderValue(LogoEffect.SunAngleProperty);
        }
    }
}
