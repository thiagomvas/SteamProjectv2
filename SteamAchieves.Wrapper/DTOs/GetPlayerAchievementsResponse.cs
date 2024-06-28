namespace SteamAchieves.Wrapper.DTOs
{
	public class GetPlayerAchievementsResponse
	{
		public PlayerStats playerstats { get; set; }

		public class PlayerStats
		{
			public List<AchievementDTO> achievements { get; set; }
		}
	}
}
