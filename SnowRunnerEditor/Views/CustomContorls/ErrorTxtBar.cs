using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace SnowRunnerEditor.Views.CustomContorls
{
    public partial class ErrorTextBox : TextBox
    {
        public bool Required { get; set; } = false;
        public bool Error { get; private set; } = false;

        private Brush? DefaultBorder { get; set; }
        private bool Clicked { get; set; } = false;
        
        
        public void SetVisualError(bool value, Brush? brush = null)
        {
            if (DefaultBorder == null) DefaultBorder = BorderBrush;

            if(value && brush == null) throw new ArgumentNullException(nameof(brush));
            if (value)
            {
                BorderBrush = brush;
                Error = true;
            }
            else
            {
                BorderBrush = DefaultBorder;
                Error = false;
            }
        }

        public void CheckTextError(object sender, RoutedEventArgs e)
        {
            if (!Required) return;
            
            SolidColorBrush brush = new(Colors.Red);

            if (Text == "" && Clicked) SetVisualError(true, brush);
            else if (Text != "") SetVisualError(false);

            Clicked = true;
        }

        public void CheckFocusError(object sender, RoutedEventArgs e) => CheckError();

        public void CheckError()
        {
            if (!Required) return;

            SolidColorBrush brush = new(Colors.Red);

            if (Text == "") SetVisualError(true, brush);
            else if (Text != "") SetVisualError(false);

            Clicked = true;
        }
    }
}
