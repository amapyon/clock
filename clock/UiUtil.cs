using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace clock
{
    class UiUtil
    {

        public static void SetEnableColor(Control control)
        {
            control.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
        }

        public static void SetDisableColor(Control control)
        {
            control.Foreground = new SolidColorBrush(System.Windows.SystemColors.ControlDarkDarkColor);
        }
    }
}
