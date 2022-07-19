using NUnit.Framework;
using OgameWrapper.Model;
using System.Threading.Tasks;

namespace OgameWrapper.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task TestLogin()
        {
            Credentials credentials = new("em@i.l", "password", "en", 132);
            OgameWrapperClient ogameClient = new(credentials);
            Assert.True(await ogameClient.Login());
        }
    }
}