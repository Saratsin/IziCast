namespace IziCast.Core.Enums
{
    public class LaunchData
    {
        public LaunchData(LaunchMode mode, string contentUrl)
        {
            Mode = mode;
            ContentUrl = contentUrl;
        }

		public LaunchMode Mode { get; }
		
		public string ContentUrl { get; }
    }
}
