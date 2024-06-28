namespace SteamAchieves.Wrapper.DTOs
{
	public class GetGlobalAchievementPercentagesResponse
	{
		public AchievementPercentages achievementpercentages { get; set; }

		public class AchievementPercentages
		{
			public List<AchievementPercentage> achievements { get; set; }
		}

		public class AchievementPercentage
		{
			public string name { get; set; }
			public double percent { get; set; }
		}
	}
}
