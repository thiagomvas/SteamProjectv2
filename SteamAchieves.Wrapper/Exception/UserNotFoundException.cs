namespace SteamAchieves.Wrapper.Exception
{
	/// <summary>
	/// Thrown when a user is not found in the Steam API
	/// </summary>
	[Serializable]
	public class UserNotFoundException : System.Exception
	{
		public UserNotFoundException() { }
		public UserNotFoundException(string userId) : base($"Could not find user with Id '{userId}'") { }
		public UserNotFoundException(string userId, System.Exception inner) : base($"Could not find user with Id '{userId}'", inner) { }
		protected UserNotFoundException(
				  System.Runtime.Serialization.SerializationInfo info,
							System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
