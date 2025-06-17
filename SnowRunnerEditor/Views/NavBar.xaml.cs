using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SnowRunnerEditor.Game.Pak;
using SnowRunnerEditor.Src;
using SnowRunnerEditor.Src.Project;
using SnowRunnerEditor.Views.ProjectCreation;
using System.Threading.Tasks;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SnowRunnerEditor.Views
{
    public sealed partial class NavBar : UserControl
    {
        public MenuBar MenuBar { get { return pMenuBar; } set { pMenuBar = value; } }

        public NavBar()
        {
            InitializeComponent();
        }

        private async void LoadProject(object sender, RoutedEventArgs e)
        {
            FileInfo projectFile = await IOHepler.GetFileFromDialog(MainWindow.WindowHandle, [".json", "*"]);
            await IOHepler.LoadProjectFromJSON(projectFile);            
        }

        private async void CreateProject(object sender, RoutedEventArgs e)
        {
            ProjectCreateControl projectCreateContorl = new();
            ProjectCreateAdvanced projectCreateAdvanced = new();

            ContentDialog dialogMain = new()
            {
                Title = "New Project",
                PrimaryButtonText = "Create",
                CloseButtonText = "Cancel",
                Content = projectCreateContorl,
                XamlRoot = Content.XamlRoot
            };

            ContentDialog dialogAdvanced = new()
            {
                Title = "Advanced Settings",
                PrimaryButtonText = "Accept",
                CloseButtonText = "Cancel",
                Content = projectCreateAdvanced,
                XamlRoot = Content.XamlRoot
            };

            dialogMain.PrimaryButtonClick += CheckMainCreateDialogState;

            ContentDialogResult mainRes = await dialogMain.ShowAsync();
            if (mainRes == ContentDialogResult.Secondary) return;

            ContentDialogResult advRes = await dialogAdvanced.ShowAsync();
            if (advRes == ContentDialogResult.Secondary) return;
        }
        
        private void CheckMainCreateDialogState(ContentDialog dialog, ContentDialogButtonClickEventArgs e)
        {            
            ProjectCreateControl projectCreateControl = (ProjectCreateControl)dialog.Content;

            bool allowContinue = true;
            SolidColorBrush brush = new(Colors.Red);

            List<ProjectCreateControl.TextBoxId> excludedBoxes = [
                ProjectCreateControl.TextBoxId.InitialBox,
                ];

            foreach (KeyValuePair<ProjectCreateControl.TextBoxId, ErrorTextBox> item in projectCreateControl.ErrorBoxes)
            {
                if (!excludedBoxes.Contains(item.Key) && item.Value.Text == "")
                {
                    allowContinue = false;
                    item.Value.SetVisualError(true, brush);
                }
            }

            if (!allowContinue)
                e.Cancel = true;            
        }        
    }
}
