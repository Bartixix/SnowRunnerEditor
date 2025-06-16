using SnowRunnerEditor.Src;

using System.Diagnostics.CodeAnalysis;

using System.IO.Compression;


namespace SnowRunnerEditor.Game
{
    internal sealed class PakHelper
    {
        [MemberNotNullWhen(false, nameof(P_LoadList))]
        public bool ReadOnly { get; private set; }

        public FileInfo InitialPak { get; private set; }
        public DirectoryInfo InitialUnpacked { get; private set; }

        private FileInfo? P_LoadList { get; set; }
        public FileInfo LoadList
        {
            get
            {
                if (ReadOnly) throw new AccessViolationException();
                return P_LoadList;
            }
            private set { P_LoadList = value; }
        }


        public PakHelper(FileInfo initialPak, DirectoryInfo initialUnpacked, bool defaultLoadList)
        {
            InitialPak = initialPak;
            InitialUnpacked = initialUnpacked;

            if (defaultLoadList)
            {
                LoadList = new($"{initialUnpacked.FullName}\\pak.load_list");
                ReadOnly = false;
            }
            else ReadOnly = true;
        }

        public PakHelper(FileInfo initialPak, DirectoryInfo initialUnpacked, FileInfo loadList)
        {
            InitialPak = initialPak;
            InitialUnpacked = initialUnpacked;
            LoadList = loadList;

            ReadOnly = false;
        }

        public PakHelper(FileInfo initialPak, LoadListHelper lh)
        {
            InitialPak = initialPak;
            InitialUnpacked = lh.InitialUnpacked;
            LoadList = lh.LoadList;

            ReadOnly = false;
        }

        public void AddLoadList(FileInfo loadList)
        {
            LoadList = loadList;

            ReadOnly = false;
        }

        public void AddDeafaultLoadList()
        {
            LoadList = new($"{InitialUnpacked.FullName}\\{LoadListHelper.LoadListName}");

            ReadOnly = false;
        }

        public async Task Unpack()
        {
            using FileStream fs = InitialPak.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            using ZipArchive zip = new(fs, ZipArchiveMode.Read);

            await Task.Run(() => zip.ExtractToDirectory(InitialUnpacked.FullName));
        }

        public async Task Pack()
        {
            if (ReadOnly) throw new UnauthorizedAccessException("pak.load_list file was not provided");

            List<KeyValuePair<string, string>> files = IOHepler.GetInitialFileNames(InitialUnpacked);

            KeyValuePair<string, string> load_list = files.First();
            files.RemoveAt(0);

            if (load_list.Key != LoadList.FullName) throw new DataMisalignedException("No pak.load_list file");


            using FileStream fs = InitialPak.Open(FileMode.Create, FileAccess.Write, FileShare.None);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            zip.CreateEntryFromFile(load_list.Key, load_list.Value, CompressionLevel.NoCompression);

            await Task.Run(() =>
            {
                foreach (KeyValuePair<string, string> file in files)
                    zip.CreateEntryFromFile(file.Key, file.Value, CompressionLevel.SmallestSize);
            });
        }
    }
}
