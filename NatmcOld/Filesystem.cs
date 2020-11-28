using Natmc.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natmc
{
    public static class Filesystem
    {
        private static readonly LogScope Log = new LogScope("Filesystem");

        public static string GameRoot { get; private set; }

        public static void Init()
        {
            GameRoot = ".";
            Log.Info($"Game root: {Path.GetFullPath(GameRoot)}");
        }

        public static bool FileExists(string path) => File.Exists(Path.Combine(GameRoot, path));
        public static bool DirectoryExists(string path) => Directory.Exists(Path.Combine(GameRoot, path));
        public static string ReadTextFile(string path) => File.ReadAllText(Path.Combine(GameRoot, path));
        public static byte[] ReadBinaryFile(string path) => File.ReadAllBytes(Path.Combine(GameRoot, path));
        public static Stream OpenFile(string path, FileMode mode, FileAccess access)
            => new FileStream(Path.Combine(GameRoot, path), mode, access);
    }
}
