namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Core;
    [TestClass]
    public class SimpleTest
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