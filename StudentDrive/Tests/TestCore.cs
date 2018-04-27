namespace Tests
{
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestCore
    {
        [TestMethod]
        public void IsNotNullMethod()
        {
            using (var core = new Core())
            {
                Assert.IsNotNull(core.GetUserAuthorize("Sergey", "123"));
            }
        }
    }
}