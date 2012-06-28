namespace Client
{
	/// <summary>
	///  A builder object for a Client instance.
	///  <p/>
	///  Typical use:
	///  <p/>
	///  <pre>
	///  {@code
	///  Client client = new ClientBuilder()
	///      .Key("your-api-key")
	///      .Secret("your-api-secret")
	///      .Host("api.tempo-db.com")
	///      .Port(80)
	///		 .Version("v1")	
	///      .Secure(true)
	///      .Build();
	///  }
	///  </pre>
	/// </summary>
	public class ClientBuilder
	{

		private string _keyRenamed;
		private string _secretRenamed;
		private string _hostRenamed = "api.tempo-db.com";
		private int _portRenamed = 443;
		private bool _secureRenamed = true;
		private string _versionRenamed = "v1";

		/// <summary>
		///  Returns the built Client instance
		/// </summary>
		///  <returns> A new Client instance </returns>
		public Client Build()
		{
			return new Client(_keyRenamed, _secretRenamed, _hostRenamed, _portRenamed, _versionRenamed, _secureRenamed);
		}

		/// <summary>
		///  Sets the api key for the client instance.
		/// </summary>
		///  <param name="key"> The api key for the database being accessed </param>
		public ClientBuilder Key(string key)
		{
			_keyRenamed = key;
			return this;
		}

		/// <summary>
		///  Sets the api secret for the client instance.
		/// </summary>
		///  <param name="secret"> The api secret for the database being accessed </param>
		public ClientBuilder Secret(string secret)
		{
			_secretRenamed = secret;
			return this;
		}

		/// <summary>
		///  Sets the api host for the client instance.
		/// </summary>
		///  <param name="host"> The hostname of the server being accessed </param>
		public ClientBuilder Host(string host)
		{
			_hostRenamed = host;
			return this;
		}

		/// <summary>
		///  Sets the api port for the client instance.
		/// </summary>
		///  <param name="port"> The port of the server being accessed </param>
		public ClientBuilder Port(int port)
		{
			_portRenamed = port;
			return this;
		}


		/// <summary>
		///  Sets the api version for the client instance.
		/// </summary>
		/// <param name="version">The api version to use </param>
		public ClientBuilder Version(string version)
		{
			_versionRenamed = version;
			return this;
		}


		/// <summary>
		///  Sets the protocol being used.
		/// </summary>
		///  <param name="secure"> true = https, false = http </param>
		public ClientBuilder Secure(bool secure)
		{
			_secureRenamed = secure;
			return this;
		}
	}
}