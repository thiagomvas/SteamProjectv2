namespace SteamAchieves.Wrapper.DTOs
{
	public class GameDTO
	{
		public string name { get; set; }
		public int appid { get; set; }
		public int playtime_2weeks { get; set; }
		public int playtime_forever { get; set; }
		public string img_icon_url { get; set; }
	}
}
