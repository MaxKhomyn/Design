using System.Diagnostics;
using System.IO;
using System;

namespace Services
{
    public static class ExceptionService
    {
        #region Properties

        public static bool IsFileExists => File.Exists($"{DirectoryService.FullPath}Exceptions");

        #endregion Properties

        #region Methods

        public static bool WriteLine(Exception erroreption) => WriteLine(erroreption.Message);

        public static bool WriteLine(string Message)
        {
            if(!CreateFile()) return false;
            try
            {
                File.AppendAllText($"{DirectoryService.FullPath}Exceptions", Environment.NewLine + Message);
                return true;
            }
            catch(Exception error)
            {
                WriteLine(error.Message);
                return false;
            }
        }

        public static bool CreateFile()
        {
            try
            {
                if (!IsFileExists) File.Create($"{DirectoryService.FullPath}Exceptions");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return false;
            }
        }

        public static bool RemoveFile()
        {
            try
            {
                if (IsFileExists) File.Delete($"{DirectoryService.FullPath}Exceptions");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return false;
            }
        }

        #endregion Methods
    }
}