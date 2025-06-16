using Microsoft.UI.Xaml;
using SnowRunnerEditor.Game;

using Windows.Storage.Pickers;
using WinRT.Interop;


namespace SnowRunnerEditor.Src
{
    internal class IOHepler
    {

        public static List<KeyValuePair<string, string>> GetInitialFileNames(DirectoryInfo searchDir)
        {

            IEnumerable<FileInfo> absolute = searchDir.EnumerateFiles("*", SearchOption.AllDirectories);

            string dir = searchDir.FullName;
            IEnumerable<KeyValuePair<string, string>> files = absolute.Select(f =>
            {
                string fileName = f.FullName;
                return new KeyValuePair<string, string>(
                    fileName,
                    fileName[(dir.Length + 1)..]
                    );
            });

            return [.. files.OrderBy(f => f.Value, PakFileNameComparer.Instance)];
        }

        public static async Task<FileInfo> GetFileFromDialog(Window wnd, List<string> fileTypes)
        {
            FileOpenPicker picker = new()
            {
                ViewMode = PickerViewMode.List,
            };

            var hwnd = WindowNative.GetWindowHandle(wnd);
            fileTypes.ForEach(picker.FileTypeFilter.Add);

            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile res = await picker.PickSingleFileAsync();
            return new(res.Path);
        }
    }
}
