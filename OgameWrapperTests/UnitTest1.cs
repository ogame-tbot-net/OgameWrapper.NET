using NUnit.Framework;
using OgameWrapper.Model;

namespace OgameWrapper.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestLogin()
        {
            Credentials credentials = new("em@i.l", "password", "en", 132);
            OgameWrapperClient ogameClient = new(credentials);
            Assert.True(ogameClient.Login());
        }
    }
}