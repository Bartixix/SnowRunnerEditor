global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using Windows.Storage;


namespace SnowRunnerEditor.Src
{
    enum Manifest
    {
        Shared,
        SharedSound
    }

    internal class GlobalVars
    {
        public static DirectoryInfo AppDataFolder { get; } = new($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SnowRunnerPatcher");
    }
}
