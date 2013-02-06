using NUnit.Framework;
using RestSharp;


namespace TempoDB.Tests
{
    [TestFixture]
    public class TempoDBTests
    {
        [Test]
        public void Defaults()
        {
            var tempodb = new TempoDB("key", "secret");
            Assert.AreEqual("key", tempodb.Key);
            Assert.AreEqual("secret", tempodb.Secret);
            Assert.AreEqual("api.tempo-db.com", tempodb.Host);
            Assert.AreEqual(443, tempodb.Port);
            Assert.AreEqual(true, tempodb.Secure);
            Assert.AreEqual("v1", tempodb.Version);
            Assert.IsNotNull(tempodb.Client);
            Assert.IsInstanceOfType(typeof(RestClient), tempodb.Client);
        }
    }
}
