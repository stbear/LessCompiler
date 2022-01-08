using Newtonsoft.Json;

namespace LessCompiler
{
    public class SourceMapOptions
    {
        [JsonProperty("outputFilename")]
        public string OutputFilename { get; set; }

        [JsonProperty("sourceMapRootpath")]
        public string SourceMapRootpath { get; set; }

        [JsonProperty("sourceMapBasepath")]
        public string SourceMapBasepath { get; set; }

        [JsonProperty("outputSourceFiles")]
        public bool OutputSourceFiles { get; set; }

        [JsonProperty("sourceMapFileInline")]
        public bool SourceMapFileInline { get; set; }

        [JsonProperty("sourceMapURL")]
        public string SourceMapURL { get; set; }
    }
}
