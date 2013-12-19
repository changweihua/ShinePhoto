using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Models;

namespace ShinePhoto.Events
{
    public class UserInfoEvent
    {
        public UserInfoEvent(UserInfo user, bool flag)
        {
            CanGo = flag;
            UserInfo = user;
        }

        public bool CanGo { get; private set; }
        public UserInfo UserInfo { get; private set; }
    }

    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    public class SettingEvent
    {
        public SettingEvent(Setting setting, bool flag)
        {
            CanGo = flag;
            Setting = setting;
        }

        public bool CanGo { get; private set; }
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
