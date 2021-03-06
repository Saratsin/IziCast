using IziCast.Core;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Plugin.Overlay.Platforms.Android.Presenters;
using MvvmCross.IoC;
using System.Linq;
using Android.App;
using MvvmCross.Converters;
using MvvmCross.Localization;
using System;
using System.IO;
using System.Globalization;
using MvvmCross;
using IziCast.Core.Services.Interfaces;
using IziCast.Core.Models;

namespace IziCast.Droid
{
	public class IziCastSetup : MvxAppCompatSetup<App>
	{      
		protected override void InitializePlatformServices()
		{
			base.InitializePlatformServices();

			CreatableTypes().EndingWith("Service")
							.Where(x => !x.IsSubclassOf(typeof(Service)))
							.AsInterfaces()
							.RegisterAsLazySingleton();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			return new MvxOverlayAppCompatViewPresenter(AndroidViewAssemblies);
		}

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters(registry);
			registry.AddOrOverwrite("Language", new MvxLanguageConverter());         
		}      
	}
}
