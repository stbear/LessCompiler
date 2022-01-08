namespace LessCompiler
{
    internal interface ICompiler
    {
        CompilerResult Compile(Config config);
    }
}