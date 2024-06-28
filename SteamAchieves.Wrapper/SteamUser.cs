using SteamAchieves.Wrapper.Data;
using SteamAchieves.Wrapper.DTOs;

namespace SteamAchieves.Wrapper
{

	/// <summary>
	/// Class representing an user. Any API calls for this user will be cached in this instance. Use constructor with bool parameter to change that setting./>
	/// </summary>
	public class SteamUser
	{
		public List<Game> OwnedGames { get; private set; } = new List<Game>();
		#region PlayerSummaries

		/// <summary>
		/// 64bit SteamID of the user
		/// </summary>
		public string SteamId { get; set; }

		/// <summary>
		/// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it. 
		/// Note that because this WebAPI does not use authentication, there are only two possible values returned: 
		/// <br />
		/// 1 - the profile is not visible to you (Private, Friends Only, etc) , <br />
		/// 3 - the profile is "Public", and the data is visible .<br /> 
		/// <br />
		/// Mike Blaszczak's post on Steam forums says, "The community visibility state this API returns is different than the privacy state. 
		/// It's the effective visibility state from the account making the request to the account being viewed given the requesting account's relationship to the viewed account."
		/// </summary>
		public int CommunityVisibilityState { get; set; }

		/// <summary>
		/// If set, indicates the user has a community profile configured (will be set to '1')
		/// </summary>
		public int ProfileState { get; set; }

		/// <summary>
		/// The player's persona name (display name)
		/// </summary>
		public string PersonaName { get; set; }

		/// <summary>
		/// The full URL of the player's Steam Community profile.
		/// </summary>
		public string ProfileUrl { get; set; }
		/// <summary>
		/// The full URL of the player's 32x32px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
		/// </summary>
		public string Avatar { get; set; }
		/// <summary>
		/// The full URL of the player's 64x64px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
		/// </summary>
		public string AvatarMedium { get; set; }
		/// <summary>
		/// The full URL of the player's 184x184px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
		/// </summary>
		public string AvatarFull { get; set; }
		public string AvatarHash { get; set; }

		/// <summary>
		/// The last time the user was online.
		/// </summary>
		public DateTime LastLogOff { get; set; }

		/// <summary>
		/// The user's current status. 0 - Offline, 1 - Online, 2 - Busy, 3 - Away, 4 - Snooze, 5 - looking to trade, 6 - looking to play. 
		/// If the player's profile is private, this will always be "0", except is the user has set their status to looking to trade or looking to play, 
		/// because a bug makes those status appear even if the profile is private.
		/// </summary>
		public ProfileState PersonaState { get; set; }

		/// <summary>
		/// The player's "Real Name", if they have set it.
		/// </summary>
		public string RealName { get; set; }

		/// <summary>
		/// The player's primary group, as configured in their Steam Community profile.
		/// </summary>
		public string PrimaryClanId { get; set; }

		/// <summary>
		/// The time the player's account was created.
		/// </summary>
		public DateTime TimeCreated { get; set; }
		public int PersonaStateFlags { get; set; }

		/// <summary>
		/// If set on the user's Steam Community profile, The user's country of residence, 2-character ISO country code
		/// </summary>
		public string LocCountryCode { get; set; }

		/// <summary>
		/// If the user is currently in-game, this will be the name of the game they are playing. This may be the name of a non-Steam game shortcut.
		/// </summary>
		public string GameExtraInfo { get; set; }

		/// <summary>
		/// If the user is currently in-game, this value will be returned and set to the gameid of that game.
		/// </summary>
		public string GameId { get; set; }
		/// <summary>
		/// The ip and port of the game server the user is currently playing on, if they are playing on-line in a game using Steam matchmaking. Otherwise will be set to "0.0.0.0:0".
		/// </summary>
		public string GameServerIp { get; set; }

		/// <summary>
		/// If set on the user's Steam Community profile, The user's state of residence
		/// </summary>
		public string LocStateCode { get; set; }

		/// <summary>
		/// An internal code indicating the user's city of residence. A future update will provide this data in a more useful way.
		/// </summary>
		public string LocCityId { get; set; }

		#endregion
		private readonly bool _cache;
        public SteamUser()
        {
            _cache = true;
        }

        public SteamUser(bool cache)
		{
			_cache = cache;
		}

		public async Task<List<Game>> GetShallowOwnedGamesAsync(SteamApiService service)
		{
			var result = await service.GetShallowGamesOwnedAsync(SteamId);
			if(_cache)
			{
				OwnedGames = result;
			}
			return result;
		}
		public void ConsumeDTO(PlayerSummariesDTO dto)
		{
			SteamId = dto.steamid;
			CommunityVisibilityState = dto.communityvisibilitystate;
			ProfileState = dto.profilestate;
			PersonaName = dto.personaname;
			ProfileUrl = dto.profileurl;
			Avatar = dto.avatar;
			AvatarMedium = dto.avatarmedium;
			AvatarFull = dto.avatarfull;
			AvatarHash = dto.avatarhash;
			LastLogOff = DateTimeOffset.FromUnixTimeSeconds(dto.lastlogoff).UtcDateTime; 
			PersonaState = (ProfileState)dto.personastate;
			RealName = dto.realname;
			PrimaryClanId = dto.primaryclanid;
			TimeCreated = DateTimeOffset.FromUnixTimeSeconds(dto.timecreated).UtcDateTime;
			PersonaStateFlags = dto.personastateflags;
			LocCountryCode = dto.loccountrycode;
		}
    }
}
