namespace SteamAchieves.Wrapper.Data
{
	public class PriceOverview
	{
		public string Currency { get; set; }
		public int Initial { get; set; }
		public int Final { get; set; }
		public int DiscountPercent { get; set; }
		public string InitialFormatted { get; set; }
		public string FinalFormatted { get; set; }
	}
}
