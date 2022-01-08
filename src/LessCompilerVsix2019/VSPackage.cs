namespace LessCompilerVsix
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidCompilerPackageString = "e45cd35c-44e9-46c3-afe6-81d117d2ad2c";
        public const string guidCompilerCmdSetString = "92a030a3-2493-40f9-b24b-34fdfffafb7d";
        public static Guid guidCompilerPackage = new Guid(guidCompilerPackageString);
        public static Guid guidCompilerCmdSet = new Guid(guidCompilerCmdSetString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int MyMenuGroup = 0x1020;
        public const int SolExpMenuGroup = 0x1030;
        public const int ProjectMenuGroup = 0x1040;
        public const int ContextMenu = 0x1050;
        public const int ContextMenuGroup = 0x1060;
        public const int ContextMenuSyncGroup = 0x1070;
        public const int CreateConfigFile = 0x0100;
        public const int RecompileConfigFile = 0x0200;
        public const int CompileOnBuild = 0x0300;
        public const int RemoveConfig = 0x0400;
        public const int CompileSolution = 0x0500;
        public const int CleanOutputFiles = 0x0600;
    }
}
