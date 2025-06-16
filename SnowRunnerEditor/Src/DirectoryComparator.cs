using System.Diagnostics.CodeAnalysis;

using System.Security.Cryptography;

using System.Text;


namespace SnowRunnerEditor.Src
{
    internal class DirectoryComparator(DirectoryInfo firstFolder, DirectoryInfo secondFolder)
    {
        //To satisfy the compiler
        [MemberNotNullWhen(true, [
            nameof(P_DifferentFiles),
            nameof(P_NewFiles),
            nameof(P_OldFiles),
            ])]
        public bool Compared { get; private set; } = false;

        public DirectoryInfo FirstFolder { get; private set; } = firstFolder;
        public DirectoryInfo SecondFolder { get; private set; } = secondFolder;


        private List<FileInfo>? P_DifferentFiles { get; set; }
        private List<FileInfo>? P_NewFiles { get; set; }
        private List<FileInfo>? P_OldFiles { get; set; }

        public List<FileInfo> DifferentFiles
        {
            get
            {
                if (Compared) return P_DifferentFiles;
                throw new AccessViolationException();
            }
            private set { P_DifferentFiles = value; }
        }
        public List<FileInfo> NewFiles
        {
            get
            {
                if (Compared) return P_NewFiles;
                throw new AccessViolationException();
            }
            private set { P_NewFiles = value; }
        }
        public List<FileInfo> OldFiles
        {
            get
            {
                if (Compared) return P_OldFiles;
                throw new AccessViolationException();
            }
            private set { P_OldFiles = value; }
        }


        public void CompareDirectories()
        {
            if (Compared) throw new Exception("Already compared");

            List<FileInfo> filesFirst = [.. FirstFolder.GetFiles("*", SearchOption.AllDirectories)];
            List<FileInfo> filesSecond = [.. FirstFolder.GetFiles("*", SearchOption.AllDirectories)];

            List<FileInfo> both = [..filesFirst.Where(f => filesSecond.Any(s =>
            {
                string realtiveFrist = f.FullName[FirstFolder.FullName.Length..];
                string relativeSecond = s.FullName[SecondFolder.FullName.Length..];

                if (realtiveFrist == relativeSecond) return true;
                return false;
            }))];


            using MD5 md5 = MD5.Create();

            DifferentFiles = [.. both.Where(file =>
            {
                string relative = file.FullName[FirstFolder.FullName.Length..];

                FileInfo secondFile = new($"{SecondFolder.FullName}\\{relative}");
                using FileStream fs1 = new(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using FileStream fs2 = new(secondFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

                byte[] h1 = md5.ComputeHash(fs1);
                byte[] h2 = md5.ComputeHash(fs2);

                if (!h1.SequenceEqual(h2))
                    return true;

                return false;
            })];

            NewFiles = [..filesFirst.Where(f => !filesSecond.Any (s =>
            {
                string realtiveFrist = f.FullName[FirstFolder.FullName.Length..];
                string relativeSecond = s.FullName[SecondFolder.FullName.Length..];

                if (realtiveFrist == relativeSecond) return true;
                return false;
            }))];

            OldFiles = [..filesSecond.Where(s => !filesFirst.Any (f =>
            {
                string realtiveFrist = f.FullName[FirstFolder.FullName.Length..];
                string relativeSecond = s.FullName[SecondFolder.FullName.Length..];

                if (realtiveFrist == relativeSecond) return true;
                return false;
            }))];

            Compared = true;
        }

        public static void Write(FileStream fs, List<StorageFile> files)
        {
            int offset = 0;
            foreach (StorageFile file in files)
            {
                string str = $"{file.Path}\n";
                byte[] buff = Encoding.UTF8.GetBytes(str);
                fs.Write(buff, 0, buff.Length);
                offset += buff.Length;
            }
        }
    }
}
