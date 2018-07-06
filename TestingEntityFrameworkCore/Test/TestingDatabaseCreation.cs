namespace Test
{
    using NUnit.Framework;

    using Test.TestUtilities;

    using WebApplication.Data;

    [TestFixture]
    public class TestingDatabaseCreation
    {
        [Test]
        public void TestCreation()
        {
            ApplicationDbContext context = TestDatabaseContextFactory.CreateDbContext();
        }
    }
}