using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SnowRunnerEditor.Game;

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

        //Test functions
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void TestFunc(object sender, RoutedEventArgs e)
        {
            FileInfo initial = new("C:\\Users\\barti\\Desktop\\snowrnn_test\\initial.pak");
            DirectoryInfo folder = new("C:\\Users\\barti\\Desktop\\snowrnn_test\\unpack");
            FileInfo target = new("C:\\Users\\barti\\Desktop\\snowrnn_test\\initial-made.pak");

            PakHelper pk = new(initial, folder, true);
            await pk.Unpack();

            pk.AddDeafaultLoadList();

            PakHelper pk2 = new(target, folder, true);
            await pk2.Pack();
        }

        private async void TestFunc2(object sender, RoutedEventArgs e)
        {
            //StorageFolder first = await StorageFolder.GetFolderFromPathAsync("C:\\Users\\barti\\Desktop\\snwrnncmb\\1");
            //StorageFolder second = await StorageFolder.GetFolderFromPathAsync("C:\\Users\\barti\\Desktop\\snwrnncmb\\2");

            //StorageFile firstFile = await StorageFile.GetFileFromPathAsync("C:\\Users\\barti\\Desktop\\snwrnncmb\\new.pak");
            //StorageFile secondFile = await StorageFile.GetFileFromPathAsync("C:\\Users\\barti\\Desktop\\snwrnncmb\\old.pak");

            //PakHelper pk1 = new(firstFile, first);
            //PakHelper pk2 = new(secondFile, second);
            //await pk1.Unpack();
            //await pk2.Unpack();

            //await pk1.AddDeafaultLoadList();
            //await pk2.AddDeafaultLoadList();

            //DirectoryComparer drc = new(first, second);
            //await drc.CompareDirectories();

            //List<StorageFile> differnet = drc.DifferentFiles;
            //List<StorageFile> oldFiles = drc.OldFiles;
            //List<StorageFile> newFiles = drc.NewFiles;

            //using FileStream fsDifferent = new("C:\\Users\\barti\\Desktop\\snwrnncmb\\diff.txt", FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            //using FileStream fsNew = new("C:\\Users\\barti\\Desktop\\snwrnncmb\\new.txt", FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            //using FileStream fsOld = new("C:\\Users\\barti\\Desktop\\snwrnncmb\\old.txt", FileMode.CreateNew, FileAccess.Write, FileShare.Read);

            //Write(fsDifferent, differnet);
            //Write(fsOld, oldFiles);
            //Write(fsNew, newFiles);

        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
