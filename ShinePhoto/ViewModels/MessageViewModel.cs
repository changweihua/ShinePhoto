using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows.Controls.Primitives;

namespace ShinePhoto.ViewModels
{
    public class MessageViewModel : PropertyChangedBase, IViewAware
    {
        private Popup popup;

        public void AttachView(object view, object context = null)
        {
            var viewPopup = view as Popup;
            if (viewPopup != null)
            {
                popup = viewPopup;
                popup.StaysOpen = false;
            }
        }

        public object GetView(object context = null)
        {
            return popup;
        }

        public event EventHandler<ViewAttachedEventArgs> ViewAttached;
    }
}
