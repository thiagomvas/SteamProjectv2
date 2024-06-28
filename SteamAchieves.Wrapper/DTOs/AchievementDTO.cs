namespace SteamAchieves.Wrapper.DTOs
{
	public class AchievementDTO
	{
		public string apiname { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int achieved { get; set; }
		public long unlocktime { get; set; }
	}
}
