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
using System.ComponentModel;
using System.Diagnostics;

namespace ShinePhoto.UC
{
    /// <summary>
    /// HoloDatePickerUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class HoloDatePickerUserControl : UserControl
    {
        public HoloDatePickerUserControl()
        {
            InitializeComponent();
            InitialDate(DateTime.Parse(CurrentDate??DateTime.Now.ToString("yyyy-MM-dd")));
        }

        #region 事件

        //private static object 

        #endregion

        #region 公共方法

        private void InitialDate( DateTime dt)
        {
            var now = dt;
            CurrentMonth = now.Month;
            CurrentYear = now.Year;
            CurrentDay = now.Day;
            SetPrevMonth(CurrentMonth);
            SetNextMonth(CurrentMonth);
            _maxDays = DateTime.DaysInMonth(CurrentYear, CurrentMonth);
            SetPrevDay(CurrentDay);
            SetNextDay(CurrentDay);
        }

        private void SetPrevMonth(int value)
        {
            int result = 0;

            if (value == 1)
            {
                result = 12;
            }
            else
            {
                result = value - 1;
            }

            PrevMonth = result;
        }

        private void SetNextMonth(int value)
        {
            int result = 0;

            if (value == 12)
            {
                result = 1;
            }
            else
            {
                result = value + 1;
            }

            NextMonth = result;
        }

        private void SetPrevDay(int value)
        {
            int result = 0;

            if (value == 1)
            {
                result = _maxDays;
            }
            else
            {
                result = value - 1;
            }

            PrevDay = result;
        }

        private void SetNextDay(int value)
        {
            int result = 0;

            if (value == _maxDays)
            {
                result = 1;
            }
            else
            {
                result = value + 1;
            }

            NextDay = result;
        }

        #endregion

        #region 依赖项属性

        /// <summary>
        /// 当前日期
        /// </summary>
        public string CurrentDate
        {
            get { return (string)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }
        
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(string), typeof(HoloDatePickerUserControl), new FrameworkPropertyMetadata(DateTime.Now.ToString("yyyy-MM-dd"), CurrentDatePropertyChanged, CurrentDateCoerceValue), CurrentDateValidateValue);

        static void CurrentDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(String.Format("PropertyChanged - 属性：{0} 新值：{1} 旧值：{2}", e.Property.Name, e.NewValue, e.OldValue));
        }

        //返回强制转换后的值
        static object CurrentDateCoerceValue(DependencyObject dobj, object newValue)
        {
            Debug.WriteLine(String.Format("CoerceValue - {0}", newValue));
            return newValue;
        }

        static bool CurrentDateValidateValue(object obj)
        {
            Debug.WriteLine(String.Format("ValidateValue - {0}", obj));
            return true;
        }

        #region Year Property

        /// <summary>
        /// 当前年份
        /// </summary>
        public int CurrentYear
        {
            get { return (int)GetValue(CurrentYearProperty); }
            set { SetValue(CurrentYearProperty, value); }
        }

        public static readonly DependencyProperty CurrentYearProperty =
            DependencyProperty.Register("CurrentYear", typeof(int), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(0, CurrentYearPropertyChanged, CurrentYearCoerceValue), CurrentYearValidateValue);

        static void CurrentYearPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(String.Format("PropertyChanged - 属性：{0} 新值：{1} 旧值：{2}", e.Property.Name, e.NewValue, e.OldValue));
        }

        //返回强制转换后的值
        static object CurrentYearCoerceValue(DependencyObject dobj, object newValue)
        {
            Debug.WriteLine(String.Format("CoerceValue - {0}", newValue));
            return newValue;
        }

        static bool CurrentYearValidateValue(object obj)
        {
            Debug.WriteLine(String.Format("ValidateValue - {0}", obj));
            return true;
        }

        public int PrevYear
        {
            get { return (int)GetValue(PrevYearProperty); }
            set { SetValue(PrevYearProperty, value); }
        }

        public static readonly DependencyProperty PrevYearProperty =
            DependencyProperty.Register("PrevYear", typeof(int), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(0));

        public int NextYear
        {
            get { return (int)GetValue(NextYearProperty); }
            set { SetValue(NextYearProperty, value); }
        }

        public static readonly DependencyProperty NextYearProperty =
            DependencyProperty.Register("NextYear", typeof(int), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(0));

        #endregion


        #region Month Property

        /// <summary>
        /// 当前月份
        /// </summary>
        public int CurrentMonth
        {
            get { return (int)GetValue(CurrentMonthProperty); }
            set { SetValue(CurrentMonthProperty, value); }
        }

        public static readonly DependencyProperty CurrentMonthProperty =
            DependencyProperty.Register("CurrentMonth", typeof(int), typeof(HoloDatePickerUserControl), new FrameworkPropertyMetadata(0, CurrentMonthPropertyChanged, CurrentMonthCoerceValue), CurrentMonthValidateValue);

        static void CurrentMonthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(String.Format("PropertyChanged - 属性：{0} 新值：{1} 旧值：{2}", e.Property.Name, e.NewValue, e.OldValue));
        }

        //返回强制转换后的值
        static object CurrentMonthCoerceValue(DependencyObject dobj, object newValue)
        {
            Debug.WriteLine(String.Format("CoerceValue - {0}", newValue));
            return newValue;
        }

        static bool CurrentMonthValidateValue(object obj)
        {
            Debug.WriteLine(String.Format("ValidateValue - {0}", obj));
            return true;
        }

        public int NextMonth
        {
            get { return (int)GetValue(NextMonthProperty); }
            set { SetValue(NextMonthProperty, value); }
        }

        public static readonly DependencyProperty NextMonthProperty =
            DependencyProperty.Register("NextMonth", typeof(int), typeof(HoloDatePickerUserControl), new FrameworkPropertyMetadata(0));

        public int PrevMonth
        {
            get { return (int)GetValue(PrevMonthProperty); }
            set { SetValue(PrevMonthProperty, value); }
        }

        public static readonly DependencyProperty PrevMonthProperty =
            DependencyProperty.Register("PrevMonth", typeof(int), typeof(HoloDatePickerUserControl), new FrameworkPropertyMetadata(0));


        #endregion


        #region Day Property

        /// <summary>
        /// 当前是多少号
        /// </summary>
        public int CurrentDay
        {
            get { return (int)GetValue(CurrentDayProperty); }
            set { SetValue(CurrentDayProperty, value); }
        }

        public static readonly DependencyProperty CurrentDayProperty =
            DependencyProperty.Register("CurrentDay", typeof(int), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(0, CurrentDayPropertyChanged, CurrentDayCoerceValue), CurrentDayValidateValue);

        public int NextDay
        {
            get { return (int)GetValue(NextDayProperty); }
            set { SetValue(NextDayProperty, value); }
        }

        public static readonly DependencyProperty NextDayProperty =
            DependencyProperty.Register("NextDay", typeof(int), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(0));

        public int PrevDay
        {
            get { return (int)GetValue(PrevDayProperty); }
            set { SetValue(PrevDayProperty, value); }
        }

        public static readonly DependencyProperty PrevDayProperty =
            DependencyProperty.Register("PrevDay", typeof(int), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(0));

        static void CurrentDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(String.Format("PropertyChanged - 属性：{0} 新值：{1} 旧值：{2}", e.Property.Name, e.NewValue, e.OldValue));
        }

        //返回强制转换后的值
        static object CurrentDayCoerceValue(DependencyObject dobj, object newValue)
        {
            Debug.WriteLine(String.Format("CoerceValue - {0}", newValue));
            return newValue;
        }

        static bool CurrentDayValidateValue(object obj)
        {
            Debug.WriteLine(String.Format("ValidateValue - {0}", obj));
            return true;
        }

        #endregion


        /// <summary>
        /// 起始日期
        /// </summary>
        public string MinDate
        {
            get { return (string)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(string), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(""));


        /// <summary>
        /// 结束日期
        /// </summary>
        public string MaxDate
        {
            get { return (string)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(string), typeof(HoloDatePickerUserControl), new UIPropertyMetadata(""));



        #endregion

        #region 日期增减按钮事件


        private void DecreaseMonth_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentMonth--;
            if (CurrentMonth == 0)
            {
                CurrentMonth = 12;
            }
            SetPrevMonth(CurrentMonth);
            SetNextMonth(CurrentMonth);
            _maxDays = DateTime.DaysInMonth(CurrentYear, CurrentMonth);
        }

        private void IncreaseMonth_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int temp = CurrentMonth + 1;
            if (temp == 12)
            {
                CurrentMonth = 1;
            }
            else
            {
                CurrentMonth = temp;
            }
            SetPrevMonth(CurrentMonth);
            SetNextMonth(CurrentMonth);
            _maxDays = DateTime.DaysInMonth(CurrentYear, CurrentMonth);
        }

        private void DecreaseDay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentDay--;
            if (CurrentDay == 0)
            {
                CurrentDay = _maxDays;
            }
            SetNextDay(CurrentDay);
            SetPrevDay(CurrentDay);
        }

        private void IncreaseDay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentDay++;
            if (CurrentDay == _maxDays)
            {
                CurrentDay = 1;
            }
            SetNextDay(CurrentDay);
            SetPrevDay(CurrentDay);
        }


        private void IncreaseYear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentYear++;
        }

        private void DecreaseYear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentYear--;
        }

        #endregion

        #region CLR 属性

        private int _maxDays = 31;

        public string SelectedDate
        {
            get {
                return (new DateTime(CurrentYear, CurrentMonth, CurrentDay)).ToString("yyyy-MM-dd");
            }
        }

        #endregion

    }
}
