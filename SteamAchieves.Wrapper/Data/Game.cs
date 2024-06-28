namespace SteamAchieves.Wrapper.Data
{
	public class Game
	{
		public string Name { get; set; }
		public int AppId { get; set; }
		public TimeSpan PlaytimeTwoWeeks { get; set; }
		public TimeSpan PlaytimeAllTime { get; set; }
		public string IconUrl { get; set; }
		public List<Achievement> Achievements { get; set; }
		public string RequiredAge { get; set; }
		public bool IsFree { get; set; }
		public string DetailedDescription { get; set; }
		public string About { get; set; }
		public string ShortDescription { get; set; }
		public string SupportedLanguages { get; set; }
		public string HeaderImage { get; set; }
		public string Website { get; set; }
		public string Background { get; set; }
		public string ReleaseDate { get; set; }
		public bool ComingSoon { get; set; }

		public void AddData(Game other)
		{
			if (other == null)
			{
				return;
			}

			if (string.IsNullOrEmpty(Name))
			{
				Name = other.Name;
			}

			if (string.IsNullOrEmpty(IconUrl))
			{
				IconUrl = other.IconUrl;
			}

			if (string.IsNullOrEmpty(RequiredAge))
			{
				RequiredAge = other.RequiredAge;
			}

			if (string.IsNullOrEmpty(DetailedDescription))
			{
				DetailedDescription = other.DetailedDescription;
			}

			if (string.IsNullOrEmpty(About))
			{
				About = other.About;
			}

			if (string.IsNullOrEmpty(ShortDescription))
			{
				ShortDescription = other.ShortDescription;
			}

			if (string.IsNullOrEmpty(SupportedLanguages))
			{
				SupportedLanguages = other.SupportedLanguages;
			}

			if (string.IsNullOrEmpty(Website))
			{
				Website = other.Website;
			}

			if (string.IsNullOrEmpty(Background))
			{
				Background = other.Background;
			}

			if (string.IsNullOrEmpty(ReleaseDate))
			{
				ReleaseDate = other.ReleaseDate;
			}

			if (ComingSoon == false)
			{
				ComingSoon = other.ComingSoon;
			}
		}
	}
}
