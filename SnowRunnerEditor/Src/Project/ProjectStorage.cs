using System.Text.Json.Serialization;

namespace SnowRunnerEditor.Src.Project
{
    internal class ProjectStorage
    {
        public string Name { get; }

        public string OriginalPak { get; }
        public string InitialPak { get; }
        public string LoadListFile { get; }
        public string SharedPak { get; }
        public string SharedSoundPak { get; }

        public string ProjectDir { get; }

        public string Created { get; }
        public string LastModified { get; }

        public ProjectStorage(ProjectHelper project)
        {
            Name = project.Name;
            OriginalPak = project.OriginalPak.FullName;
            InitialPak = project.InitialPak.FullName;
            LoadListFile = project.LoadListFile.FullName;
            SharedPak = project.SharedPak.FullName;
            SharedSoundPak = project.SharedSoundPak.FullName;

            ProjectDir = project.ProjectDir.FullName;

            Created = project.Created.ToString("O");
            LastModified = project.LastModified.ToString("O");
        }


        [JsonConstructor]
        public ProjectStorage(string name, string originalPak, string initialPak, string loadListFile, string sharedPak, string sharedSoundPak, string projectDir, string created, string lastModified)
        {
            Name = name;
            OriginalPak = originalPak;
            InitialPak = initialPak;
            LoadListFile = loadListFile;
            SharedPak = sharedPak;
            SharedSoundPak = sharedSoundPak;
            ProjectDir = projectDir;
            Created = created;
            LastModified = lastModified;
        }
    }
}
