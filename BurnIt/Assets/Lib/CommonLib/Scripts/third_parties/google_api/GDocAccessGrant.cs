using System;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace MTUnity.GoogleAPI {
	class GDocAccessGrant
	{
		public static void GetRefreshToken(GoogleParams p) {
			OAuth2Parameters parameters = new OAuth2Parameters();
			parameters.ClientId = p.clientId;
			parameters.ClientSecret = p.clientSecret;
			parameters.RedirectUri = p.redirectUri;
			parameters.Scope = p.scope;
			
			string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
			Console.WriteLine(authorizationUrl);
			Console.WriteLine("Please visit the URL above to authorize your OAuth "
			                  + "request token.  Once that is complete, type in your access code to "
			                  + "continue...");
			parameters.AccessCode = Console.ReadLine();
			
			OAuthUtil.GetAccessToken(parameters);
			Console.WriteLine("OAuth Refresh Token: \n" + parameters.RefreshToken);
		}
	}
}
