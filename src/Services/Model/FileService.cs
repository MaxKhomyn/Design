using System.IO;

namespace Services.Model
{
    public class FileService
    {
        public static bool Exist(string Path) => File.Exists(Path);
        public static string UniquePath(string Path, string Extention = "") => UniquePath(ref Path, Extention);

        public static string UniquePath(ref string Path, string Extention = "")
        {
            if (!Exist(Path + Extention)) return Path + Extention;

            Path += " - "; int index = 1;
            while (Exist(Path + index + Extention)) index++;

            return Path + index + Extention;
        }
    }
}