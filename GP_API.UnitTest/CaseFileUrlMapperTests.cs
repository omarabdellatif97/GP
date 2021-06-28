using GP_API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP_API.UnitTest
{
    [TestClass]
    public class CaseFileUrlMapperTests
    {



        private static TestContext context;
        private static string actionRouteString;
        private static string templateString;
        private static string template;
        private static string description;
        private static List<string> urls;
        private static CaseFileUrlMapper extractor;


        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
            context = ctx;
            actionRouteString = @"https://localhost:44371/api/file/download";
            templateString = @"FileURL-d61b8182e027-FileURL";
            template = @"<p><img src=""FileURL-d61b8182e027-FileURL/398"" alt="""" width=""604"" height=""565"" /></p>
<p><img src=""FileURL-d61b8182e027-FileURL/123"" alt="""" width=""604"" height=""565"" /></p>
<p><img src=""FileURL-d61b8182e027-FileURL/123134241"" alt="""" width=""604"" height=""565"" /></p>
<p><img src=""FileURL-d61b8182e027-FileURL/123534"" alt="""" width=""604"" height=""565"" /></p>
";
            description = @"<p><img src=""https://localhost:44371/api/file/download/398"" alt="""" width=""604"" height=""565"" /></p>
<p><img src=""https://localhost:44371/api/file/download/123"" alt="""" width=""604"" height=""565"" /></p>
<p><img src=""https://localhost:44371/api/file/download/123134241"" alt="""" width=""604"" height=""565"" /></p>
<p><img src=""https://localhost:44371/api/file/download/123534"" alt="""" width=""604"" height=""565"" /></p>
";

            urls = new List<string>()
            {
                "https://localhost:44371/api/file/download/398",
                "https://localhost:44371/api/file/download/123",
                "https://localhost:44371/api/file/download/123134241",
                "https://localhost:44371/api/file/download/123534"
            };

            extractor = new CaseFileUrlMapper(actionRouteString, templateString);
        }

        [ClassCleanup]
        public static void CleanUp()
        {


            context = null;
            actionRouteString = null;
            templateString = null;
            template = null;
            description = null;
        }

        [TestMethod]
        public void ExtractIdsTest()
        {
            //prepare
            var expectedIds = new[] { 398, 123, 123134241, 123534 };
            // act
            var ids = extractor.ExtractIds(description);
            Console.WriteLine(string.Join(", ", ids));
            // assert
            CollectionAssert.AreEquivalent(expectedIds, ids);
        }

        [TestMethod]
        public void GenerateTemplateTest()
        {
            //prepare
            var expectedTemplate = template;
            // act
            var resultTemplate = extractor.GenerateTemplate(description);

            // assert
            Assert.AreEqual(expectedTemplate, resultTemplate);
        }


        [TestMethod]
        public void GenerateDescriptionTest()
        {
            //prepare
            var expectedDecription = description;
            // act
            var resultDecription = extractor.GenerateDescription(template);

            // assert
            Assert.AreEqual(expectedDecription, resultDecription);
        }


        [TestMethod]
        public void ExtractUrlsTest()
        {
            //prepare
            var expected = urls;

            // act
            var result = extractor.ExtractFullUrls(description);

            // assert
            CollectionAssert.AreEquivalent(expected, result);
        }

    }
}
