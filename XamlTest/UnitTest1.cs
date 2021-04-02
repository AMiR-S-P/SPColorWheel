using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using XamlAnalyzer;
namespace XamlTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            string s = "";

            s = XamlAnalyzer.Utilities.XamlSharper.GetClassName(
                "<Window x:Class=\"XamlAnalyzer.View.SplashScreen\"" +
                "xmlns = \"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");


            Assert.AreEqual(s, "XamlAnalyzer.View.SplashScreen");
        }
    }
}
