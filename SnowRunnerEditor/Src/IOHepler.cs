using SnowRunnerEditor.Game.Pak;
using SnowRunnerEditor.Src.Project;
using System.Text.Json;
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

        public static async Task<FileInfo> GetFileFromDialog(IntPtr hwnd, List<string> fileTypes)
        {
            FileOpenPicker picker = new()
            {
                ViewMode = PickerViewMode.List,
                
            };

            fileTypes.ForEach(picker.FileTypeFilter.Add);

            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile? res = await picker.PickSingleFileAsync();
            return res == null ? throw new Exception("Missing File") : new(res.Path);
        }

        public static async Task<DirectoryInfo> GetDirectoryFromDialog(IntPtr hwnd)
        {
            FolderPicker picker = new()
            {
                ViewMode = PickerViewMode.List,
                FileTypeFilter = { "*" }
            };

            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFolder ret = await picker.PickSingleFolderAsync();
            return new(ret.Path);
        }


        public static async Task LoadProjectFromJSON(FileInfo file)
        {
            using FileStream json = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            ProjectStorage storage = await JsonSerializer.DeserializeAsync<ProjectStorage>(json) ?? throw new InvalidDataException();

            ProjectHelper.LoadProject(storage);
        }
    }
}
