
namespace MTUnity.GoogleAPI {
	public class GoogleParams {
		
		public string clientId;
		
		// This is the OAuth 2.0 Client Secret retrieved
		// above.  Be sure to store this value securely.  Leaking this
		// value would enable others to act on behalf of your application!
		public string clientSecret;
		
		// Run GDocAccessGrant program to get Refresh Token
		public string refreshToken;
		
		// Space separated list of scopes for which to request access.
		public string scope;
		
		// This is the Redirect URI for installed applications.
		// If you are building a web application, you have to set your
		// Redirect URI at https://code.google.com/apis/console.
		public string redirectUri;
	}
}
