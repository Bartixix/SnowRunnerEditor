namespace SnowRunnerEditor.Game
{
    internal class LoadListHelper
    {
        public static string LoadListName { get; } = "pak.load_list";


        public DirectoryInfo InitialUnpacked { get; protected set; }

        public FileInfo LoadList { get; protected set; }
        public FileInfo Shared { get; protected set; }
        public FileInfo SharedSound { get; protected set; }

        public LoadListHelper(DirectoryInfo initialUnpacked, FileInfo loadList, FileInfo shared, FileInfo sharedSound)
        {
            InitialUnpacked = initialUnpacked;
            LoadList = loadList;
            Shared = shared;
            SharedSound = sharedSound;
        }

        public LoadListHelper(PakHelper pk, FileInfo shared, FileInfo sharedSound)
        {
            if (pk.ReadOnly) throw new Exception();

            InitialUnpacked = pk.InitialUnpacked;
            LoadList = pk.LoadList;
            Shared = shared;
            SharedSound = sharedSound;
        }

        public void Create()
        {

        }

        public List<LoadListEntry> CreateEntries()
        {
            throw new NotImplementedException();
        }
    }
}
