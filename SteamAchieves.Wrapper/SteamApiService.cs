using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamAchieves.Wrapper.Data;
using SteamAchieves.Wrapper.DTOs;
using SteamAchieves.Wrapper.Exception;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;

namespace SteamAchieves.Wrapper
{
	public class SteamApiService
	{
		private readonly string _apiKey;
		private readonly HttpClient _http;
		public SteamApiService(string apiKey)
		{
			_apiKey = apiKey;
			_http = new HttpClient();
		}

		/// <summary>
		/// Calls the ISteamUser/ResolveVanityURL endpoint to resolve a vanity URL to a Steam ID
		/// </summary>
		/// <param name="steamId">The steam user Id</param>
		/// <returns>The resolved steamId. If an already resolved Steam Id is passed, the original id will be returned with no calls.</returns>
		/// <exception cref="InvalidSteamIdException">Thrown when the API response was not successful. </exception>
		/// <exception cref="InvalidOperationException">Thrown when the response returns an unexpected error value</exception>
		/// <remarks>Uses the API Key once.</remarks>
		public async Task<string> ResolveVanityURL(string steamId)
		{
			if (long.TryParse(steamId, out _))
				return steamId;
			else
			{
				string apiUrl = $"http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={_apiKey}&vanityurl={steamId}";
				var result = await _http.GetStringAsync(apiUrl);
				JObject obj = JObject.Parse(result);
				int successValue = obj["response"]["success"].Value<int>();
				switch ((SteamApiResponse)successValue)
				{
					case SteamApiResponse.Success:
						return obj["response"]!["steamid"]!.ToString();
					case SteamApiResponse.Failure:
						throw new InvalidSteamIdException("Failed to sanitize ID. The API response was not successful.");
					default:
						throw new InvalidOperationException("Unexpected Steam API response value.");
				}
			}
		}

		/// <summary>
		/// Calls the ISteamUser/GetPlayerSummaries endpoint to get player summaries
		/// </summary>
		/// <param name="userId">The id of the user whose data is being fetched</param>
		/// <returns>A DTO containing data for a user.</returns>
		/// <remarks>Uses the API Key once.</remarks>
		public async Task<PlayerSummariesDTO> GetPlayerSummariesAsync(string userId)
		{
			string playerSummaries = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_apiKey}&steamids={userId}";

			try
			{
				var response = await _http.GetAsync(playerSummaries);
				if (response == null)
				{
					throw new UserNotFoundException("Failed to get player summaries");
				}
				var str = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<GetPlayerSummariesResponse>(str);
				return result.response.players.First();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Calls the store to get data about a game
		/// </summary>
		/// <param name="appId">The game's Id</param>
		/// <returns>The game data from the store.</returns>
		public async Task<Game> GetGameDetailsFromStoreAsync(int appId)
		{
			var language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			var apiUrl = $"https://store.steampowered.com/api/appdetails?appids={appId}&l={language}";

			try
			{
				var response = await _http.GetStringAsync(apiUrl);
				JObject obj = JObject.Parse(response);
				if (obj[appId.ToString()]["success"].Value<bool>() == false)
				{
					throw new GameNotFoundException(appId.ToString());
				}

				var result = obj[appId.ToString()].ToObject<StoreAppDetailsResponse>();

				var game = new Game
				{
					Name = result.data.name,
					AppId = appId,
					IconUrl = result.data.header_image,
					Achievements = new List<Achievement>(),
					RequiredAge = result.data.required_age,
					IsFree = result.data.is_free,
					DetailedDescription = result.data.detailed_description,
					About = result.data.about_the_game,
					ShortDescription = result.data.short_description,
					SupportedLanguages = result.data.supported_languages,
					Website = result.data.website,
					Background = result.data.background,
					ReleaseDate = result.data.release_date.date,
					ComingSoon = result.data.release_date.coming_soon
				};

				return game;

			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Gets a game's data from IPlayerService/GetOwnedGames and the store. Prioritizes data from IPlayerService/GetOwnedGames
		/// </summary>
		/// <param name="appId">The game's Id</param>
		/// <param name="steamId">The user's Id</param>
		/// <returns>A user's data for a game, as well as store data from said game</returns>
		/// <exception cref="GameNotFoundException">Thrown when an invalid game id is passed</exception>
		public async Task<Game> GetGameDetailsForUserAsync(int appId, string steamId)
		{
			var httpClient = new HttpClient();

			// Get game details
			var gameDetailsResponse = await httpClient.GetStringAsync(
				$"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_apiKey}&steamid={steamId}&include_appinfo=true&format=json");
			var gameDetailsData = JsonConvert.DeserializeObject<GetOwnedGamesResponse>(gameDetailsResponse);


			var gameData = gameDetailsData.response.games.Find(game => game.appid == appId);

			if (gameData == null)
			{
				throw new GameNotFoundException(appId.ToString());
			}

			var game = new Game
			{
				Name = gameData.name,
				AppId = gameData.appid,
				PlaytimeTwoWeeks = TimeSpan.FromMinutes(gameData.playtime_2weeks),
				PlaytimeAllTime = TimeSpan.FromMinutes(gameData.playtime_forever),
				IconUrl = $"http://media.steampowered.com/steamcommunity/public/images/apps/{gameData.appid}/{gameData.img_icon_url}.jpg",
				Achievements = new List<Achievement>()
			};


			// Get achievements
			var achievementsResponse = await httpClient.GetStringAsync(
				$"http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v0001/?appid={appId}&key={_apiKey}&steamid={steamId}&l={CultureInfo.CurrentCulture.TwoLetterISOLanguageName}");
			var achievementsData = JsonConvert.DeserializeObject<GetPlayerAchievementsResponse>(achievementsResponse);

			var globalAchievementsResponse = await httpClient.GetStringAsync(
				$"http://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid={appId}");
			var globalAchievementsData = JsonConvert.DeserializeObject<GetGlobalAchievementPercentagesResponse>(globalAchievementsResponse);

			var achievementPercentages = new Dictionary<string, double>();

			foreach (var achievement in globalAchievementsData.achievementpercentages.achievements)
			{
				achievementPercentages[achievement.name] = achievement.percent;
			}

			foreach (var achievement in achievementsData.playerstats.achievements)
			{
				game.Achievements.Add(new Achievement
				{
					Name = achievement.name,
					ApiName = achievement.apiname,
					Description = achievement.description,
					GlobalUnlockPercentage = achievementPercentages.ContainsKey(achievement.apiname) ? achievementPercentages[achievement.apiname] : 0,
					IsUnlocked = achievement.achieved == 1,
					UnlockTime = achievement.achieved == 1 ? DateTimeOffset.FromUnixTimeSeconds(achievement.unlocktime).UtcDateTime : (DateTime?)null
				});
			}

			var storeResult = await GetGameDetailsFromStoreAsync(appId);
			if (storeResult != null)
			{
				game.AddData(storeResult);
			}

			return game;
		}

		public async Task<List<Game>> GetShallowGamesOwnedAsync(string steamId)
		{
			string gameDetails = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_apiKey}&steamid={steamId}&include_appinfo=true&format=json";

			try
			{
				var response = await _http.GetAsync(gameDetails);
				if (response != null)
				{
					var str = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<GetOwnedGamesResponse>(str);
					var games = new List<Game>();

					foreach (var gameData in result.response.games)
					{
						var game = new Game
						{
							Name = gameData.name,
							AppId = gameData.appid,
							PlaytimeTwoWeeks = TimeSpan.FromMinutes(gameData.playtime_2weeks),
							PlaytimeAllTime = TimeSpan.FromMinutes(gameData.playtime_forever),
							IconUrl = $"http://media.steampowered.com/steamcommunity/public/images/apps/{gameData.appid}/{gameData.img_icon_url}.jpg",
							Achievements = new List<Achievement>()
						};

						games.Add(game);
					}

					return games;
				}
				return new();
			}
			catch
			{
				throw;
			}
		}
	}
}
