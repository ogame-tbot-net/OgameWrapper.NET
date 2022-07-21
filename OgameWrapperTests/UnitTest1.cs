using NUnit.Framework;
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
            var email = "em@i.l";
            var password = "password";
            LobbyClient lobbyClient = new(email, password);
            await lobbyClient.Login();
        }
    }
}