using CodeFactory.ContentManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using Moq;

namespace CodeFactory.ContentManager.Test
{
    
    
    /// <summary>
    ///This is a test class for ContentManagementServiceTest and is intended
    ///to contain all ContentManagementServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContentManagementServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetChildPages
        ///</summary>
        [TestMethod()]
        public void GetChildPagesTest()
        {
            Nullable<Guid> parentId = new Nullable<Guid>();
            string slug = string.Empty;
            int pageSize = int.MaxValue;
            int pageIndex = 0;
            int totalCount = 0;

            TestContext.WriteLine("Recuperando todas las páginas hijo sin especificar parentId");
            List<IPage> actual = ContentManagementService.GetChildPages(parentId, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count > 0);
            foreach (IPage item in actual)
                Assert.IsInstanceOfType(item, typeof(Page));

            TestContext.WriteLine("Recuperando solo las hijas especificando el parentId");
            parentId = new Guid("90983831-e1a6-4425-8f79-71c342dfa0df");
            actual = ContentManagementService.GetChildPages(parentId, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count > 0);
            foreach (IPage item in actual)
                Assert.IsInstanceOfType(item, typeof(Page));

            TestContext.WriteLine("Recuperando solo las hijas especificando el parentId y con la paginación errónea");
            parentId = new Guid("90983831-e1a6-4425-8f79-71c342dfa0df");
            pageSize = 10;
            pageIndex = 50;
            actual = ContentManagementService.GetChildPages(parentId, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count == 0);
        }

        /// <summary>
        ///A test for GetPageBySlug
        ///</summary>
        [TestMethod()]
        public void GetPageBySlugTest()
        {
            TestContext.WriteLine("Recuperando la primera página mediante el slug");
            string slug = "FirstPage";
            IPage actual = ContentManagementService.GetPageBySlug(slug);
            Assert.IsInstanceOfType(actual, typeof(Page));
        }

        /// <summary>
        ///A test for GetPage
        ///</summary>
        [TestMethod()]
        public void GetPageTest()
        {
            var mock = new Mock<IPageRepository>();

            mock.Setup(foo => foo.GetPage(It.IsAny<Guid>())).Returns(new Page());

            mock.VerifyAll();

            TestContext.WriteLine("Recuperando la primera página mediante el id");
            Guid id = new Guid("90983831-e1a6-4425-8f79-71c342dfa0df");
            IPage actual = ContentManagementService.GetPage(id);
            Assert.IsInstanceOfType(actual, typeof(Page));
        }

        /// <summary>
        ///A test for InsertPage
        ///</summary>
        [TestMethod()]
        public void CRUDPageTest()
        {
            IPage page = new Page();

            page.Title = page.Slug = Guid.NewGuid().ToString();
            string slug = page.Title;
            page.Description = "Description";
            page.Layout = "UnknownLayout";
            page.Keywords = "Keywords";
            page.IsVisible = true;
            page.Author = "kohku";

            ContentManagementService.InsertPage(page);
            page = ContentManagementService.GetPageBySlug(slug);
            Assert.IsInstanceOfType(page, typeof(Page));

            page.Layout = "ThisLayout";
            ContentManagementService.UpdatePage(page);
            page = ContentManagementService.GetPageBySlug(slug);
            Assert.IsInstanceOfType(page, typeof(Page));
            Assert.AreEqual("ThisLayout", page.Layout);

            page.Roles.Add("Administrator");
            ContentManagementService.UpdateRoles(page);
            page = ContentManagementService.GetPageBySlug(slug);
            Assert.IsInstanceOfType(page, typeof(Page));
            Assert.AreEqual(1, page.Roles.Count);

            ContentManagementService.DeletePage(page);
            page = ContentManagementService.GetPageBySlug(slug);
            Assert.IsNotInstanceOfType(page, typeof(Page));
        }

        /// <summary>
        ///A test for GetPages
        ///</summary>
        [TestMethod()]
        public void GetPagesTest()
        {
            Nullable<Guid> id = new Nullable<Guid>();
            Nullable<Guid> parentId = new Nullable<Guid>();
            Nullable<Guid> sectionId = new Nullable<Guid>();
            string slug = string.Empty;
            Nullable<bool> isVisible = new Nullable<bool>();
            int pageSize = int.MaxValue; 
            int pageIndex = 0;
            int totalCount = 0;

            TestContext.WriteLine("Recuperando todas las páginas");
            List<IPage> actual = ContentManagementService.GetPages(id, parentId, sectionId, slug, isVisible, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count > 0);
            foreach (IPage item in actual)
                Assert.IsInstanceOfType(item, typeof(Page));

            TestContext.WriteLine("Recuperando las páginas invisibles");
            isVisible = false;
            actual = ContentManagementService.GetPages(id, parentId, sectionId, slug, isVisible, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count > 0);
            foreach (IPage item in actual)
                Assert.IsInstanceOfType(item, typeof(Page));

            TestContext.WriteLine("Recuperando la primera página");
            isVisible = default(Nullable<bool>);
            slug = "FirstPage";
            actual = ContentManagementService.GetPages(id, parentId, sectionId, slug, isVisible, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count > 0);
            foreach (IPage item in actual)
                Assert.IsInstanceOfType(item, typeof(Page));

            TestContext.WriteLine("Recuperando la primera página");
            slug = string.Empty;
            parentId = new Guid("90983831-e1a6-4425-8f79-71c342dfa0df");
            actual = ContentManagementService.GetPages(id, parentId, sectionId, slug, isVisible, pageSize, pageIndex, out totalCount);
            Assert.IsTrue(actual.Count > 0);
            foreach (IPage item in actual)
                Assert.IsInstanceOfType(item, typeof(Page));
        }
    }
}
