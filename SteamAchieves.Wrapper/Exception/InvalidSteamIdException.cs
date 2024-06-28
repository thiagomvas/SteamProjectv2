namespace SteamAchieves.Wrapper.Exception
{

	[Serializable]
	public class InvalidSteamIdException : System.Exception
	{
		public InvalidSteamIdException() { }
		public InvalidSteamIdException(string id) : base($"Failed to sanitize ID '{id}'. The API response was not successful.") { }
		public InvalidSteamIdException(string id, System.Exception inner) : base($"Failed to sanitize ID '{id}'. The API response was not successful.", inner) { }
		protected InvalidSteamIdException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
