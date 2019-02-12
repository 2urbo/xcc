using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Globalization;

namespace FirstFloor.Xcc.Test
{
    [TestClass]
    public class PageTests
    {
        [TestMethod]
        public void TestXamarinContentPageAndroid()
        {
            TestXaml("__ANDROID__", false, "XamarinContentPage.xaml", "XamarinContentPage.android.expected.xaml");
        }

        [TestMethod]
        public void TestXamarinContentPageAndroidRemoveIgnorableContent()
        {
            TestXaml("__ANDROID__", true, "XamarinContentPage.xaml", "XamarinContentPage.android.expected.xaml");
        }

        [TestMethod]
        public void TestXamarinContentPageiOs()
        {
            TestXaml("__IOS__", false, "XamarinContentPage.xaml", "XamarinContentPage.ios.expected.xaml");
        }

        [TestMethod]
        public void TestXamarinContentPageiOsRemoveIgnorableContent()
        {
            TestXaml("__IOS__", true, "XamarinContentPage.xaml", "XamarinContentPage.ios.expected.xaml");
        }

        [TestMethod]
        public void TestXamarinContentPageNoSymbols()
        {
            TestXaml(null, false, "XamarinContentPage.xaml", "XamarinContentPage.nosymbols.expected.xaml");
        }

        [TestMethod]
        public void TestXamarinContentPageNoSymbolsRemoveIgnorableContent()
        {
            TestXaml(null, true, "XamarinContentPage.xaml", "XamarinContentPage.nosymbols.expected.xaml");
        }

        private static void TestXaml(string symbols, bool removeIgnorableContent, string xamlName, string expectedXamlName)
        {
            var preprocessor = new XamlPreprocessor(symbols, removeIgnorableContent);
            var xaml = LoadXamlPage(xamlName);
            var expected = LoadXamlPage(expectedXamlName);
            var result = preprocessor.ProcessXaml(xaml);

            // perform char-by-char comparison, raise error with index info if mismatch
            var lineNumber = 1;
            for (var i = 0; i < expected.Length && i < result.Length; i++) {
                if (expected[i] != result[i]) {
                    Assert.Fail("Character mismatch at index {0} (line number: {1}. Expected: {2} ({3}), actual: {4} ({5})", i, lineNumber, result[i], (int)result[i], expected[i], (int)expected[i]);
                }
                if (result[i] == '\n') {
                    lineNumber++;
                }
            }

            // still fail if one string is substring of the other 
            Assert.AreEqual(expected, result);
        }

        private static string LoadXamlPage(string pageName)
        {
            var fullName = string.Format(CultureInfo.InvariantCulture, "FirstFloor.Xcc.Test.Xaml.{0}", pageName);

            using (var stream = typeof(PageTests).Assembly.GetManifestResourceStream(fullName)) {
                using (var reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
