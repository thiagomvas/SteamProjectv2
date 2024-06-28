namespace SteamAchieves.Wrapper.DTOs
{
	public class GetOwnedGamesResponse
	{
		public Response response { get; set; }

		public class Response
		{
			public List<GameDTO> games { get; set; }
		}
	}
}
