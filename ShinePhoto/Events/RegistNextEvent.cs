using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Models;

namespace ShinePhoto.Events
{
    public class RegistNextEvent
    {
        public RegistNextEvent(UserModel user, bool flag)
        {
            CanGo = flag;
            User = user;
        }

        public bool CanGo { get; private set; }
        public UserModel User { get; private set; }
    }
}
