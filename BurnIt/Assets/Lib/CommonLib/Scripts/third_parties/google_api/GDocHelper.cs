using System;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace MTUnity.GoogleAPI {
	public class GDocHelper
	{
		private static SpreadsheetsService GetService(GoogleParams p) {
			OAuth2Parameters parameters = new OAuth2Parameters();
			parameters.ClientId = p.clientId;
			parameters.ClientSecret = p.clientSecret;
			parameters.RedirectUri = p.redirectUri;
			parameters.Scope = p.scope;
			parameters.RefreshToken = p.refreshToken;

			OAuthUtil.RefreshAccessToken(parameters);

			GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "DocReader", parameters);
			SpreadsheetsService service = new SpreadsheetsService("DocReader");
			service.RequestFactory = requestFactory;

			return service;
		}

		public static MTSpreadsheet GetSpreadsheet(string title, GoogleParams p) {
			SpreadsheetsService service = GetService(p);
			SpreadsheetQuery query = new SpreadsheetQuery();
			query.Title = title;
			SpreadsheetFeed feed = service.Query(query);
			SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
			return new MTSpreadsheet(spreadsheet, service);
		}
	}
}
