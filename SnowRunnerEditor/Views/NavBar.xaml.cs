using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SnowRunnerEditor.Game.Pak;
using SnowRunnerEditor.Src;
using SnowRunnerEditor.Src.Project;
using SnowRunnerEditor.Views.CustomContorls;
using System.Threading.Tasks;
using System.Windows.Input;

using SnowRunnerEditor.Src.Controls;
using System.Diagnostics;

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
            // element names
            string name = "Name";
            string directory = "Dir";
            string initialPak = "Initial";
            string sharedPak = "Shared";
            string SharedSoundPak = "SharedSound";
            string InitialDir = "Initial";
            string SharedCopy = "Shared";
            string SharedSoundCopy = "SharedSound";


            Dictionary<CreateProjectDialogs.DialogMapping, string> mappings = new()
            {
                { CreateProjectDialogs.DialogMapping.Name, name },
                { CreateProjectDialogs.DialogMapping.Directory, directory },
                { CreateProjectDialogs.DialogMapping.InitialPak, initialPak },
                { CreateProjectDialogs.DialogMapping.SharedPak, sharedPak },
                { CreateProjectDialogs.DialogMapping.SharedSoundPak, SharedSoundPak },
                { CreateProjectDialogs.DialogMapping.InitialDir, InitialDir },
                { CreateProjectDialogs.DialogMapping.SharedCopy, SharedCopy },
                { CreateProjectDialogs.DialogMapping.SharedSoundCopy, SharedSoundCopy }
            };

            InputControlBuilder main = new()
            {
                Width = 500
            };

            main.AddExtendedTextBox(name, "Name", true);
            main.AddItemSelector(directory, "Project Directory*", "Select", true, ItemSelector.PickTypeSelector.Folder, "");
            main.AddItemSelector(initialPak, "initial.pak file", "Select", false, ItemSelector.PickTypeSelector.File, ".pak, *");
            main.AddItemSelector(sharedPak, "shared.pak file*", "Select", true, ItemSelector.PickTypeSelector.File, ".pak, *");
            main.AddItemSelector(SharedSoundPak, "shared_sound.pak File*", "Select", true, ItemSelector.PickTypeSelector.File, ".pak, *");

            main.AddFooter("Footer", "* - Required");


            InputControlBuilder advanced = new()
            {
                Width = 400
            };

            advanced.AddExtendedCheckBox(InitialDir, "Unpackaged initial.pak already in project directory");
            advanced.AddExtendedCheckBox(SharedCopy, "Copy shared.pak (~1.6GB)");
            advanced.AddExtendedCheckBox(SharedSoundCopy, "Copy shared_sound.pak (~5.4GB)");

            advanced.AddFooter("Footer", "If selected shared/shared_sound isn't in the game directory, you may have to manually update them after an update.");


            ContentDialog mainDialog = new()
            {
                Title = "New Project",
                PrimaryButtonText = "Next",
                CloseButtonText = "Cancel",
                Content = main,
                XamlRoot = Content.XamlRoot,                
            };

            mainDialog.PrimaryButtonClick += CreateProjectDialogs.CheckMainCreateDialogState;


            ContentDialog advancedDialog = new()
            {
                Title = "Advanced Settings",
                PrimaryButtonText = "Create",
                CloseButtonText = "Cancel",
                Content = advanced,
                XamlRoot = Content.XamlRoot
            };

            ContentDialogResult mainRes = await mainDialog.ShowAsync();
            if (mainRes == ContentDialogResult.None) return;

            ContentDialogResult advancedRes = await advancedDialog.ShowAsync();
            if (advancedRes == ContentDialogResult.None) return;

            await CreateProjectDialogs.SetupProject(main, advanced, mappings);
        }




        private async void TestCmp(object sender, RoutedEventArgs e)
        {
            string rootDir = "";

            FileInfo oldPak = new($"{rootDir}\\initial-old.pak");
            FileInfo newPak = new($"{rootDir}\\initial-new.pak");

            DirectoryInfo oldUnpack = new($"{rootDir}\\old");
            DirectoryInfo newUnpack = new($"{rootDir}\\new");

            try
            {
                PakHelper oldPakHelper = new(oldPak, oldUnpack, false);
                PakHelper newPakHelper = new(newPak, newUnpack, false);

                await oldPakHelper.Unpack();
                await newPakHelper.Unpack();

                DirectoryComparator dirCmp = new(oldUnpack, newUnpack);
                dirCmp.CompareDirectories();

                dirCmp.WriteAll(rootDir);
            }
#if DEBUG
            catch
            {
                throw;
            }
#endif
            finally
            {
                await Task.Run(() => oldUnpack.Delete(true));
                await Task.Run(() => newUnpack.Delete(true));
            }
        }

        private async void TestContendDialog(object sender, RoutedEventArgs e)
        {
            InputControlBuilder controlBuilder = new();

            controlBuilder.AddExtendedCheckBox("testChx", "test chx lbl");
            controlBuilder.AddExtendedTextBox("textTxt", "Text txt lbl", true);
            controlBuilder.AddItemSelector("testItemSel", "test sel lbl", "select", false, ItemSelector.PickTypeSelector.File, ".json, *");

            ContentDialog dialog = new()
            {
                Title = "Test Dialog",
                PrimaryButtonText = "Accept",
                CloseButtonText = "Cancel",
                Content = controlBuilder,
                XamlRoot = Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        
    }
}
