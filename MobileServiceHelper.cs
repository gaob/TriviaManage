using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace TriviaManage
{
	/// <summary>
	/// Dynamically instancated single instance of the Mobile Service client
	/// </summary>
	public class MobileServiceHelper
	{
		private static MobileServiceHelper _instance;

		const string applicationURL = @"https://dotnet2.azure-mobile.net/";
		const string applicationKey = @"vJvQmMAfYHXtPOXyVgtiPLKiaoyNVl83";

		private readonly MobileServiceClient _client;

		/// <summary>
		/// Prevents a default instance of the <see cref="MobileServiceHelper"/> class from being created.
		/// </summary>
		private MobileServiceHelper()
		{
			CurrentPlatform.Init();

			// Initialize the Mobile Service client with a handler to insert master key.
			_client = new MobileServiceClient (applicationURL, applicationKey, new MyHandler ());
		}

		/// <summary>
		/// The _sync root used for save multi-threaded creation/access to the singleton
		/// </summary>
		private static volatile object _syncRoot = new object();

		/// <summary>
		/// Gets the service client.
		/// </summary>
		/// <value>The service client.</value>
		public MobileServiceClient ServiceClient { get { return _client; } }

		/// <summary>
		/// Gets the default service, instaciating it if it does not exist
		/// Uses the double check pattern
		/// </summary>
		/// <value>The default service.</value>
		public static MobileServiceHelper DefaultService
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
						{
							_instance = new MobileServiceHelper();
						}
					}
				}

				return _instance;
			}
		}

		/// <summary>
		/// Customized Handler to add master key to every request.
		/// </summary>
		public class MyHandler : DelegatingHandler
		{
			protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				//Add the master key before sending.
				request.Headers.Add("X-ZUMO-MASTER", "ZvZIAXfWSLPPwvIeipxIyLfKYmPYoM27");

				var response = await base.SendAsync(request, cancellationToken);
				return response;
			}
		}
	}
}
