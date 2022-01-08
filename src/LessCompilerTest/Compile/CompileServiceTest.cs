using Microsoft.VisualStudio.TestTools.UnitTesting;
using LessCompiler;

namespace LessCompilerTest
{
    [TestClass]
    public class CompileServiceTest
    {
        [TestMethod, TestCategory("CompileService")]
        public void LessIsSupported()
        {
            var result = CompilerService.IsSupported(".LESS");
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("CompileService")]
        public void LowerCaseSupportedExtensionAlsoWorks()
        {
            var result = CompilerService.IsSupported(".less");
            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("CompileService")]
        public void OtherExtensionDoesntWorks()
        {
            var result = CompilerService.IsSupported(".cs");
            Assert.IsFalse(result);
        }

    }
}
