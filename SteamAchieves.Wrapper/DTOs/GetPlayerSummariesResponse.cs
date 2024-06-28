namespace SteamAchieves.Wrapper.DTOs
{
	public class GetPlayerSummariesResponse
	{
		public Response response { get; set; }
		public class Rootobject
		{
			public Response response { get; set; }
		}

		public class Response
		{
			public PlayerSummariesDTO[] players { get; set; }
		}

	}

}
