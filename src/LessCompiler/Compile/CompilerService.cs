using System;
using System.IO;
using System.Linq;

namespace LessCompiler
{
    /// <summary>
    /// A service for working with the compilers.
    /// </summary>
    public static class CompilerService
    {
        internal const string Version = "0.2.0";
        private static readonly string _path = Path.Combine(Path.GetTempPath(), "LessCompiler" + Version);
        private static object _syncRoot = new object(); // Used to lock on the initialize step

        /// <summary>A list of allowed file extensions.</summary>
        public static readonly string[] AllowedExtensions = new[] { ".LESS" };

        /// <summary>
        /// Test if a file type is supported by the compilers.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns>True if the file type can be compiled.</returns>
        public static bool IsSupported(string inputFile)
        {
            string ext = Path.GetExtension(inputFile).ToUpperInvariant();

            return AllowedExtensions.Contains(ext);
        }

        internal static ICompiler GetCompiler(Config config)
        {
            string ext = Path.GetExtension(config.InputFile).ToUpperInvariant();
            ICompiler compiler = null;

            Initialize();

            switch (ext)
            {
                case ".LESS":
                    compiler = new LessCompiler(_path);
                    break;
            }

            return compiler;
        }

        /// <summary>
        /// Initializes the Node environment.
        /// </summary>
        public static void Initialize()
        {
            var node_modules = Path.Combine(_path, "node_modules");
            var log_file = Path.Combine(_path, "log.txt");

            lock (_syncRoot)
            {
                if (!Directory.Exists(node_modules) || !File.Exists(log_file))
                {
                    OnInitializing();

                    if (Directory.Exists(_path))
                        Directory.Delete(_path, true);

                    Directory.CreateDirectory(_path);
                    SaveResourceFile(_path, "LessCompiler.Node.node.zip", "node.zip");

                    System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(_path, "node.zip"), _path);

                    // If this file is written, then the initialization was successful.
                    File.WriteAllText(log_file, DateTime.Now.ToLongDateString());

                    OnInitialized();
                }
            }
        }

        private static void SaveResourceFile(string path, string resourceName, string fileName)
        {
            using (Stream stream = typeof(CompilerService).Assembly.GetManifestResourceStream(resourceName))
            using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                stream.CopyTo(fs);
            }
        }

        private static void OnInitializing()
        {
            if (Initializing != null)
            {
                Initializing(null, EventArgs.Empty);
            }
        }

        private static void OnInitialized()
        {
            if (Initialized != null)
            {
                Initialized(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires when the compilers are about to be initialized.
        /// </summary>
        public static event EventHandler<EventArgs> Initializing;

        /// <summary>
        /// Fires when the compilers have been initialized.
        /// </summary>
        public static event EventHandler<EventArgs> Initialized;
    }
}
