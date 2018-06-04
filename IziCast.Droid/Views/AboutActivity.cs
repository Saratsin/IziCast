using System;
using Android.OS;
using IziCast.Core.ViewModels;
using IziCast.Droid.Base;
using Android.Widget;
using Mehdi.Sakout.AboutPageLib;
using Android.Views;
using Android.App;
using Android.Content.PM;
using IziCast.Droid.Extensions;

namespace IziCast.Droid.Views
{
	[Activity(ScreenOrientation = ScreenOrientation.Portrait)]
	public class AboutActivity : BaseActivity<AboutViewModel>
    {
		protected override int LayoutId { get; } = Resource.Layout.about_activity;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
		         
			var aboutContainer = FindViewById<FrameLayout>(Resource.Id.about_container);

			var versionName = PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.MetaData).VersionName;

            var versionElement = new Element().SetTitle(string.Format(ViewModel.TextSource.GetText("VersionFormat"), versionName));

			var aboutPage = new AboutPage(this).SetDescription(ViewModel.TextSource.GetText("Description"))
                                               .SetImage(Resource.Mipmap.ic_launcher)
				                               .AddItem(versionElement)
                                               .AddFacebook(ViewModel.FacebookId)
                                               .AddTwitter(ViewModel.TwitterId)
                                               .AddEmail(ViewModel.Email)
                                               .AddGitHub(ViewModel.GitHubId)
			                                   .Create();

			var padding = this.DpToPx(5);
			aboutPage.SetPadding(padding, padding, padding, padding);

			aboutContainer.AddView(aboutPage, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}
	}
}
