using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WavePathAlg
{
    public static class Extensions
    {
        public static void SetProperty(this object obj, string name, object value)
        {
            obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .SetValue(obj, value);
        }

        public static void Invoke(this Control control, Action action)
        {
            control.Invoke(action);
        }
    }
}
