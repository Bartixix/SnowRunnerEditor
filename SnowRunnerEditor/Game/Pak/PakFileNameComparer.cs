using SnowRunnerEditor.Game.LoadList;

namespace SnowRunnerEditor.Game.Pak
{
    internal class PakFileNameComparer : IComparer<string>
    {
        public static PakFileNameComparer Instance { get; } = new();

        public int Compare(string? x, string? y)
        {
            bool first = LoadListHelper.LoadListName.Equals(x, StringComparison.OrdinalIgnoreCase);
            bool second = LoadListHelper.LoadListName.Equals(y, StringComparison.OrdinalIgnoreCase);

            if (first)
            {
                if (second) return 0;
                return -1;
            }
            if (second) return 1;

            return StringComparer.OrdinalIgnoreCase.Compare(x, y);
        }
    }
}
