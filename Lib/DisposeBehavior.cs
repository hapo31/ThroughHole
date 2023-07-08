using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CamPreview.Lib
{
    internal class DisposeBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closed += WindowClosed;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            (AssociatedObject.DataContext as IDisposable)?.Dispose();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closed -= WindowClosed;
        }
    }
}
