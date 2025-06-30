using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SnowRunnerEditor.Views.CustomContorls
{
    public sealed partial class ExtendedTxtBox : UserControl
    {
        public ErrorTextBox TextBox => TxtBox;

        public bool Required { get { return TxtBox.Required; } set { TxtBox.Required = value; } }

        public string Label { get; set; } = string.Empty;

        public ExtendedTxtBox()
        {
            InitializeComponent();

            TxtBox.TextChanged += TxtBox.CheckTextError;
            TxtBox.LostFocus += TxtBox.CheckFocusError;
        }
    }
}
