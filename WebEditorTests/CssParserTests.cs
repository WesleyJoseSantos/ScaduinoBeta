using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebEditor.StyleModels;
using WebEditor.StyleModels.Main;
using WebEditor.StyleModels.Text;

namespace WebEditorTests
{
    [TestClass]
    public class CssParserTests
    {
        [TestMethod]
        public void TestGlobalToString()
        {
            var actualCss = new Global();
            var expectedCss =
            @"* {
                margin: 0;
                padding: 0;
                outline: 0;
                box-sizing: border-box;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            }";
            TestCssStyleParser(actualCss, expectedCss);
        }

        [TestMethod]
        public void TestBodyToString()
        {
            var actualCss = new Body();
            var expectedCss =
            @"body {
                width: 100%;
                height: 100vh;
                padding-top: 30px;
                display: flex;
                flex-direction: column;
                align-items: center;
                justify-content: center;
             }";
            TestCssStyleParser(actualCss, expectedCss);
        }

        [TestMethod]
        public void TestTitleToString()
        {
            var actualCss = new Title();
            var expectedCss =
            @".title {
                text-align: center;
                color: #f5f5f5;
            }";
            TestCssStyleParser(actualCss, expectedCss);
        }

        [TestMethod]
        public void TestHeaderTitleSpanToString()
        {
            var actualCss = new HeaderTitleSpan();
            var expectedCss =
            @"header.title span {
                font-size: 3em;
            }";
            TestCssStyleParser(actualCss, expectedCss);
        }

        public void TestCssStyleParser(ICss actualCss, string expectedCss)
        {
            var parsedCss = CssParser.CssToString(actualCss);
            
            var actualLines = parsedCss.Split('\n');
            var expectedLines = expectedCss.Split('\n');
            for (int i = 0; i < actualLines.Length; i++)
            {
                string actual = actualLines[i].Trim();
                string expected = expectedLines[i].Trim();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}