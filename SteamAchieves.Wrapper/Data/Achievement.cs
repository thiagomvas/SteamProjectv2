namespace SteamAchieves.Wrapper.Data
{
	public class Achievement
	{
		public string Name { get; set; }
		public string ApiName { get; set; }
		public string Description { get; set; }
		public double GlobalUnlockPercentage { get; set; }
		public bool IsUnlocked { get; set; }
		public DateTime? UnlockTime { get; set; }
	}
}
