using System.Collections.Generic;
using Newtonsoft.Json;

namespace LessCompiler
{
    /// <summary>
    /// Give all options for the LESS compiler
    /// </summary>
    public class LessOptions : BaseOptions<LessOptions>
    {
        private const string trueStr = "true";

        /// <summary> Creates a new instance of the class.</summary>
        public LessOptions()
        { }

        /// <summary>
        /// Load the settings from the config object
        /// </summary>
        protected override void LoadSettings(Config config)
        {
            base.LoadSettings(config);

            var autoPrefix = GetValue(config, "autoPrefix");
            if (autoPrefix != null)
                AutoPrefix = autoPrefix;

            var cssComb = GetValue(config, "cssComb");
            if (cssComb != null)
                CssComb = cssComb;

            var math = GetValue(config, "math");
            if (math != null)
                Math = math;

            var strictUnits = GetValue(config, "strictUnits");
            if (strictUnits != null)
                StrictUnits = strictUnits.ToLowerInvariant() == trueStr;

            var rootPath = GetValue(config, "rootPath");
            if (rootPath != null)
                RootPath = rootPath;

            var relativeUrls = GetValue(config, "relativeUrls");
            if (relativeUrls != null)
                RelativeUrls = relativeUrls.ToLowerInvariant() == trueStr;

            var sourceMap = GetValue(config, "sourceMap");
            if (sourceMap != null)
            {
                var dic = config.Options["sourceMap"] as Dictionary<string, object>;
                if (dic != null)
                {
                    SourceMap = true;
                    SourceMapOptions = new SourceMapOptions
                    {
                        OutputFilename = GetValue(dic, "outputFilename"),
                        SourceMapRootpath = GetValue(dic, "sourceMapRootpath"),
                        SourceMapBasepath = GetValue(dic, "sourceMapBasepath"),
                        OutputSourceFiles = GetValue(dic, "outputSourceFiles") == trueStr,
                        SourceMapFileInline = GetValue(dic, "sourceMapFileInline") == trueStr,
                        SourceMapURL = GetValue(dic, "sourceMapURL")
                    };
                }
                else
                {
                    SourceMap = sourceMap.ToLowerInvariant() == trueStr;
                }
            }

        }

        /// <summary>
        /// The file name should match the compiler name
        /// </summary>
        protected override string CompilerFileName
        {
            get { return "less"; }
        }

        /// <summary>
        /// Autoprefixer will use the data based on current browser popularity and
        /// property support to apply prefixes for you.
        /// </summary>
        [JsonProperty("autoPrefix")]
        public string AutoPrefix { get; set; } = "";

        /// <summary>
        /// CssComb will order the properties in the compiled CSS file.
        /// </summary>
        [JsonProperty("cssComb")]
        public string CssComb { get; set; } = "none";

        /// <summary>
        /// New option for math that replaces 'strictMath' option.
        /// </summary>
        [JsonProperty("math")]
        public string Math { get; set; } = null;

        /// <summary>
        /// Without this option, less attempts to guess at the output unit when it does maths.
        /// </summary>
        [JsonProperty("strictUnits")]
        public bool StrictUnits { get; set; } = false;

        /// <summary>
        /// This option allows you to re-write URL's in imported files so that the URL is always
        /// relative to the base imported file.
        /// </summary>
        [JsonProperty("relativeUrls")]
        public bool RelativeUrls { get; set; } = true;

        /// <summary>
        /// Allows you to add a path to every generated import and url in your css.
        /// This does not affect less import statements that are processed,
        /// just ones that are left in the output css.
        /// </summary>
        [JsonProperty("rootPath")]
        public string RootPath { get; set; } = "";

        /// <summary>
        /// Generate Source Map v3
        /// </summary>
        [JsonProperty("sourceMap")]
        public bool SourceMap { get; set; }

        /// <summary>
        /// Extended Source Map options
        /// </summary>
        public SourceMapOptions SourceMapOptions { get; set; }
    }
}
