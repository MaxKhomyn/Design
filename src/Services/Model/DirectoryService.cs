using System.IO;
using System;

namespace Services
{
    public class DirectoryService
    {
        #region Properties

        public static string Document => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string FullPath => Create($@"{Document}\AppName");

        #endregion Properties

        #region Methods

        public static void Remove(string DirectoryPath)
        {
            //Remove Files
            foreach (string _File in Directory.GetFiles(DirectoryPath))
            {
                File.SetAttributes(_File, FileAttributes.Normal);
                File.Delete(_File);
            }

            //Remove Directories
            foreach (string _Directory in Directory.GetDirectories(DirectoryPath))
                Remove(_Directory);

            //Remove Current Directory
            Directory.Delete(DirectoryPath, false);
        }

        public static string Create(string Path)
        {
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            return Path;
        }

        #endregion Methods
    }
}