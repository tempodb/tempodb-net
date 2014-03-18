using NUnit.Framework;
using RestSharp;


namespace TempoDB.Tests
{
    [TestFixture]
    public class TempoDBTest
    {
        [Test]
        public void Defaults()
        {
            var tempodb = new TempoDB(new Database("id"), new Credentials("key", "secret"));
            Assert.AreEqual(new Credentials("key", "secret"), tempodb.Credentials);
            Assert.AreEqual("api.tempo-db.com", tempodb.Host);
            Assert.AreEqual(443, tempodb.Port);
            Assert.AreEqual(true, tempodb.Secure);
            Assert.AreEqual("v1", tempodb.Version);
            Assert.IsNotNull(tempodb.Client);
        }
    }
}
