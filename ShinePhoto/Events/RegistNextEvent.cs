using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Models;

namespace ShinePhoto.Events
{
    /// <summary>
    /// 用户信息事件
    /// </summary>
    public class UserInfoEvent
    {
        public UserInfoEvent(UserInfo user, bool flag)
        {
            CanGo = flag;
            UserInfo = user;
        }

        /// <summary>
        /// 是否可以继续
        /// </summary>
        public bool CanGo { get; private set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; private set; }
    }

    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    /// <summary>
    /// 系统设置事件
    /// </summary>
    public class SettingEvent
    {
        public SettingEvent(Setting setting, bool flag)
        {
            CanGo = flag;
            Setting = setting;
        }

        /// <summary>
        /// 是否可以继续
        /// </summary>
        public bool CanGo { get; private set; }

        /// <summary>
        /// 设置信息
        /// </summary>
        public Setting Setting { get; private set; }
    }

    public class Setting
    {
        public string MainBackground { get; set; }
        public string Folder { get; set; }
        public string WaterMarkImage { get; set; }
        public string Logo { get; set; }
    }

}
