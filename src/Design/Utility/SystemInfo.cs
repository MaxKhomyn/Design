using System.Linq;
using System;

namespace SourceChord.FluentWPF.Utility
{
    struct VersionInfo
    {
        public int Major;
        public int Minor;
        public int Build;
    }

    class SystemInfo
    {
        #region Fields

        public static Lazy<VersionInfo> Version { get; private set; } = new Lazy<VersionInfo>(() => GetVersionInfo());

        #endregion Fields

        #region Methods

        internal static VersionInfo GetVersionInfo()
        {
            using (var mc = new System.Management.ManagementClass("Win32_OperatingSystem"))
            using (var moc = mc.GetInstances())
            {
                foreach (System.Management.ManagementObject mo in moc)
                {
                    var version = mo["Version"] as string;
                    var versionNumbers = version.Split('.').Select(o => int.Parse(o)).ToList();

                    var info = new VersionInfo()
                    {
                        Major = versionNumbers[0],
                        Minor = versionNumbers[1],
                        Build = versionNumbers[2],
                    };
                    return info;
                }
            }
            return default(VersionInfo);
        }

        internal static bool IsWin10() => Version.Value.Major == 10;
        internal static bool IsWin7() => Version.Value.Major == 6 && Version.Value.Minor == 1;
        internal static bool IsWin8x() => Version.Value.Major == 6 && (Version.Value.Minor == 2 || Version.Value.Minor == 3);

        #endregion Methods
    }
}