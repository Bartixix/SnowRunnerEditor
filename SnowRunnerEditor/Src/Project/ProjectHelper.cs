using SnowRunnerEditor.Game.LoadList;
using SnowRunnerEditor.Game.Pak;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;


namespace SnowRunnerEditor.Src.Project
{
    internal class ProjectHelper
    {
        [MemberNotNullWhen(true, nameof(P_Current))]
        public static bool ProjectLoaded { get; private set; } = false;

        private static ProjectHelper? P_Current { get; set; }
        public static ProjectHelper Current
        {
            get
            {
                if (!ProjectLoaded) throw new AccessViolationException();
                return P_Current;
            }
            set { P_Current = value; }
        }

        public string Name { get; }

        public FileInfo OriginalPak { get; }
        public FileInfo InitialPak { get; }
        public FileInfo LoadListFile { get; }
        public FileInfo SharedPak { get; }
        public FileInfo SharedSoundPak { get; }

        public DirectoryInfo ProjectDir { get; }
        public DirectoryInfo InitialUnpacked { get; }

        public DateTime Created { get; }
        public DateTime LastModified { get; }


        public PakHelper PakHelper { get; }
        public LoadListHelper LoadListHelper { get; }

        public ProjectHelper(string name, FileInfo originalPak, FileInfo initialPak, FileInfo loadListFile, FileInfo sharedPak, FileInfo sharedSoundPak, DirectoryInfo projectDir, DateTime created, DateTime lastModified)
        {
            Name = name;
            OriginalPak = originalPak;
            InitialPak = initialPak;
            LoadListFile = loadListFile;
            SharedPak = sharedPak;
            SharedSoundPak = sharedSoundPak;

            ProjectDir = projectDir;
            InitialUnpacked = new($"{ProjectDir.FullName}\\initial");

            Created = created;
            LastModified = lastModified;            


            PakHelper = new(InitialPak, InitialUnpacked, LoadListFile);
            LoadListHelper = new(InitialUnpacked, LoadListFile, SharedPak, SharedSoundPak);
        }

        private ProjectHelper (ProjectStorage storage)
        {
            Name = storage.Name;
            OriginalPak = new(storage.OriginalPak);
            InitialPak = new(storage.InitialPak);
            LoadListFile = new(storage.LoadListFile);
            SharedPak = new(storage.SharedPak);
            SharedSoundPak = new(storage.SharedSoundPak);

            ProjectDir = new(storage.ProjectDir);
            InitialUnpacked = new($"{ProjectDir.FullName}\\initial");

            Created = DateTime.Parse(storage.Created, null, DateTimeStyles.RoundtripKind);
            LastModified = DateTime.Parse(storage.LastModified, null, DateTimeStyles.RoundtripKind);

            PakHelper = new(InitialPak, InitialUnpacked, LoadListFile);
            LoadListHelper = new(InitialUnpacked, LoadListFile, SharedPak, SharedSoundPak);
        }

        public static async Task Create(string name, FileInfo originalPak, FileInfo initialPak, FileInfo loadListFile, FileInfo sharedPak, FileInfo sharedSoundPak, DirectoryInfo initialUnpacked)
        {
            ProjectHelper project = new(name, originalPak, initialPak, loadListFile, sharedPak, sharedSoundPak, initialUnpacked, DateTime.UtcNow, DateTime.UtcNow);
            await project.SetupProject();

            Current = project;
            ProjectLoaded = true;
        }

        public static void LoadProject(string name, FileInfo originalPak, FileInfo initialPak, FileInfo loadListFile, FileInfo sharedPak, FileInfo sharedSoundPak, DirectoryInfo initialUnpacked, DateTime created, DateTime lastModified)
        {
            ProjectHelper project = new(name, originalPak, initialPak, loadListFile, sharedPak, sharedSoundPak, initialUnpacked, created, lastModified);

            Current = project;
            ProjectLoaded = true;
        }

        public static void LoadProject(ProjectStorage storage)
        {
            ProjectHelper project = new(storage);

            Current = project;
            ProjectLoaded = true;
        }

        private async Task SetupProject()
        {
            PakHelper pk = new(OriginalPak, InitialUnpacked, LoadListFile);
            await pk.Unpack();
        }
    }
}
