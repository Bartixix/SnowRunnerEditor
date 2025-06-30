using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.WindowsAppSDK.Runtime.Packages;
using SnowRunnerEditor.Game.Pak;
using SnowRunnerEditor.Src.Project;
using SnowRunnerEditor.Views;
using SnowRunnerEditor.Views.CustomContorls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowRunnerEditor.Src.Controls
{
    public static class CreateProjectDialogs
    {
        public enum DialogMapping
        {
            Name,
            Directory,
            InitialPak,
            SharedPak,
            SharedSoundPak,
            InitialDir,
            SharedCopy,
            SharedSoundCopy
        }


        public static void CheckMainCreateDialogState(ContentDialog dialog, ContentDialogButtonClickEventArgs e)
        {
            InputControlBuilder projectCreateControl = (InputControlBuilder)dialog.Content;

            foreach (UIElement element in projectCreateControl.Elements.Values)
            {
                if(element.GetType() == typeof(ExtendedTxtBox))
                {
                    ExtendedTxtBox box = (ExtendedTxtBox)element;
                    box.TextBox.CheckError();

                    if (box.TextBox.Error == true)
                    {
                        e.Cancel = true;
                    }
                }
                if(element.GetType() == typeof(ItemSelector))
                {
                    ItemSelector selector = (ItemSelector)element;
                    selector.TextBox.CheckError();

                    if(selector.TextBox.Error == true)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        public static async Task SetupProject(InputControlBuilder main, InputControlBuilder advanced, Dictionary<DialogMapping, string> mapping)
        {
            int dialogMappingLength = Enum.GetNames(typeof(DialogMapping)).Length;
            if (main.Elements.Count + advanced.Elements.Count != dialogMappingLength || dialogMappingLength != mapping.Count)
                throw new Exception($"Expected {dialogMappingLength} entries");

            ExtendedTxtBox nameElement = (ExtendedTxtBox)main[mapping[DialogMapping.Name]];
            ItemSelector directoryElement = (ItemSelector)main[mapping[DialogMapping.Directory]];
            ItemSelector initialPakElement = (ItemSelector)main[mapping[DialogMapping.InitialPak]];
            ItemSelector sharedPakElement = (ItemSelector)main[mapping[DialogMapping.SharedPak]];
            ItemSelector sharedSoundPakElement = (ItemSelector)main[mapping[DialogMapping.SharedSoundPak]];

            ExtendedCheckBox initialInDir = (ExtendedCheckBox)advanced[mapping[DialogMapping.InitialDir]];
            ExtendedCheckBox sharedCopy = (ExtendedCheckBox)advanced[mapping[DialogMapping.SharedCopy]];
            ExtendedCheckBox sharedSoundCopy = (ExtendedCheckBox)advanced[mapping[DialogMapping.SharedSoundCopy]];


            string projectDir = directoryElement.TextBox.Text;
            string projectName = nameElement.TextBox.Text;

            string fullDirStr = $"{projectDir}\\{projectName}";

            DirectoryInfo fullDir = Directory.CreateDirectory(fullDirStr);
            DirectoryInfo initialDir = new($"{fullDir.FullName}\\initial-unpacked");

            DirectoryInfo depsDir = fullDir.CreateSubdirectory(".deps");
            depsDir.Attributes |= FileAttributes.Hidden;

            DirectoryInfo buildDir = fullDir.CreateSubdirectory("build");

            string originalPakStr = $"{depsDir.FullName}\\initial.pak";
            FileInfo originalPak = new(originalPakStr);

            if (initialInDir.CheckBox.IsChecked == false)
            {
                initialDir.Create();

                string initPakStr = initialPakElement.TextBox.Text;
                FileInfo initPak = new(initPakStr);

                PakHelper tmpPak = new(initPak, initialDir, false);
                await tmpPak.Unpack();
                await Task.Run(() => File.Copy(initPakStr, originalPakStr));
            }
            else
            {
                PakHelper tmpPak = new(originalPak, initialDir, true);

                await tmpPak.Pack();
            }

            string sharedPakStr = sharedPakElement.TextBox.Text;
            if (sharedCopy.CheckBox.IsChecked == true)
            {
                string destFileStr = $"{depsDir.FullName}\\shared.pak";

                await Task.Run(() => File.Copy(sharedPakStr, destFileStr));
                sharedPakStr = destFileStr;
            }

            string sharedSoundPakStr = sharedSoundPakElement.TextBox.Text;
            if (sharedSoundCopy.CheckBox.IsChecked == true)
            {
                string destFileStr = $"{depsDir.FullName}\\shared_sound.pak";

                await Task.Run(() => File.Copy(sharedSoundPakStr, destFileStr));
                sharedSoundPakStr = destFileStr;
            }

            FileInfo initialPak = new($"{buildDir.FullName}\\initial.pak");
            FileInfo loadListFile = new($"{initialDir}\\pak.load_list");
            ProjectHelper.Create(projectName, originalPak, initialPak, loadListFile, new(sharedPakStr), new(sharedSoundPakStr), fullDir);

            ProjectHelper project = ProjectHelper.Current;

            await project.SaveToDefault();
        }
    }
}
