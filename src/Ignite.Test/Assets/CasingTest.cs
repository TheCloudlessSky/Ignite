using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ignite.Assets;

namespace Ignite.Test.Assets
{
    [TestClass]
    public class CasingTest
    {
        [TestMethod]
        public void Casing_Default()
        {
            this.TestCasing(Casing.Default, new Dictionary<string, string>
            {
                {"path/to/Foobar", "path/to/Foobar"},
                {"PATH/TO/OTHER", "PATH/TO/OTHER"}
            });
        }

        [TestMethod]
        public void Casing_LowerCase()
        {
            this.TestCasing(Casing.Lowercase, new Dictionary<string, string>
            {
                {"PATH/TO/FOOBAR", "path/to/foobar"},
                {"path/TO/foobar2", "path/to/foobar2"},
                {"path/to/something", "path/to/something"}
            });
        }

        [TestMethod]
        public void Casing_CamelCase()
        {
            this.TestCasing(Casing.CamelCase, new Dictionary<string, string>
            {
                {"Path/To/FileName", "path/to/fileName"},
                {"SomeFolder/Some/FileName", "someFolder/some/fileName"},
                {"somefolder/some/filename", "somefolder/some/filename"},
                {"sOMEFOLDER/fILENAME", "sOMEFOLDER/fILENAME"},
                {"p/T/File", "p/t/file"},
                {"/path/TO/File", "/path/tO/file"}
            });
        }

        [TestMethod]
        public void Casing_Underscore()
        {
            this.TestCasing(Casing.Underscore, new Dictionary<string, string>
            {
                {"path/to/FileName", "path/to/File_Name"},
                {"PathName/SomeThing/OtherAwesomeThing", "Path_Name/Some_Thing/Other_Awesome_Thing"},
                {"pathName/someThing/otherAwesomeThing", "path_Name/some_Thing/other_Awesome_Thing"}
            });
        }

        private void TestCasing(Func<string, string> casing, IDictionary<string, string> toTest)
        {
            foreach (var kvp in toTest)
            {
                Assert.AreEqual(kvp.Value, casing(kvp.Key));
            }
        }
    }
}
