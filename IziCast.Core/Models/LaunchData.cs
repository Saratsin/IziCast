using IziCast.Core.Enums;

namespace IziCast.Core.Models
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
