using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace SnowRunnerEditor.Views.ProjectCreation
{
    public partial class ErrorTextBox : TextBox
    {
        private Brush? DefaultBorder;

        public void SetVisualError(bool value, Brush brush)
        {
            if (value)
            {
                DefaultBorder = BorderBrush;
                BorderBrush = brush;
            }
            else if(DefaultBorder != null)            
                BorderBrush = DefaultBorder;            
        }
    }
}
