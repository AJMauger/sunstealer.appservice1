using Moq;

namespace TestProject
{
    public class UnitTest2
    {
        [Fact]
        public void Test1()
        {
            var app = new Mock<AppService1.Services.IApplication>();
            var config = new AppService1.Models.Configuration();

            var result = app.Object.Reconfigure(config);

            Assert.True(result);
        }
    }
}
