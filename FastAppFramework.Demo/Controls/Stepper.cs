using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FastAppFramework.Demo
{
    public class Stepper : MaterialDesignExtensions.Controls.Stepper
    {
        public Stepper()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var body = GetTemplateChild("PART_horizontalContent") as FrameworkElement;
            if (body != null)
                body.Margin = new Thickness(0);
        }
    }
}