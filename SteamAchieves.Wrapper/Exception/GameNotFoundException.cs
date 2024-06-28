namespace SteamAchieves.Wrapper.Exception
{

	/// <summary>
	/// Thrown when a game is not found in the Steam API
	/// </summary>
	[Serializable]
	public class GameNotFoundException : System.Exception
	{
		public GameNotFoundException() { }
		public GameNotFoundException(string appId) : base($"Could not find game with App Id '{appId}'") { }
		public GameNotFoundException(string appId, System.Exception inner) : base($"Could not find game with App Id '{appId}'", inner) { }
		protected GameNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
