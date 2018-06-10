using System;
using IziCast.Core.Base;
namespace IziCast.Core.ViewModels
{
	public class AboutViewModel : BaseViewModel
    {
		public string FacebookId { get; } = "t.v.shevchuk";

		public string Email { get; } = @"schew4uk@gmail.com";

		public string GitHubId { get; } = "Saratsin/IziCast";

		public string TwitterId { get; } = "schew4uk";
    }
}
