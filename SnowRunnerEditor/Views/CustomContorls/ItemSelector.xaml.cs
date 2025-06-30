using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SnowRunnerEditor.Src;

using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SnowRunnerEditor.Views.CustomContorls
{
    public sealed partial class ItemSelector : UserControl
    {
        public string Label { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Required {get { return TxtBox.Required; } set {  TxtBox.Required = value; } }

        public List<string> ExtensionList { get; private set; } = [];
        public string FileExtensions { set { ExtensionList = [.. value.Replace(" ", "").Split(",")]; } }
        
        public PickTypeSelector PickType { get; set; } = PickTypeSelector.None;

        public ErrorTextBox TextBox => TxtBox;

        public Brush DefaultBorder { get; }

        public enum PickTypeSelector
        {
            None,
            File,
            Folder
        }

        public ItemSelector()
        {            
            InitializeComponent();

            DefaultBorder = TxtBox.BorderBrush;

            TxtBox.TextChanged += TxtBox.CheckTextError;
            TxtBox.LostFocus += TxtBox.CheckFocusError;
        }

        private async void Select(object sender, RoutedEventArgs e)
        {
            if (PickType == PickTypeSelector.None) throw new InvalidOperationException();

            if(PickType == PickTypeSelector.File)
                TxtBox.Text = (await IOHepler.GetFileFromDialog(MainWindow.WindowHandle, ExtensionList)).FullName;

            if(PickType == PickTypeSelector.Folder) 
                TxtBox.Text = (await IOHepler.GetDirectoryFromDialog(MainWindow.WindowHandle)).FullName;

            TxtBox.CheckTextError(TxtBox, e);
        }        
    }
}
