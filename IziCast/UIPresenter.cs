using System;
using Xamarin.Forms;
using IziCast.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace IziCast
{
	public class UIPresenter
	{
		static UIPresenter _singleton;
		public static UIPresenter Singleton => _singleton ?? (_singleton = new UIPresenter());

		SemaphoreSlim _locker = new SemaphoreSlim(1);
		NavigationPage _rootPage;
		Dictionary<Type, Type> _viewModelPageDictionary = new Dictionary<Type, Type>();

		public void Initialize(NavigationPage rootPage)
		{
			_rootPage = rootPage;

			var assembly = GetType().GetTypeInfo().Assembly;

			var pageTypes = assembly.DefinedTypes.Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Page", StringComparison.Ordinal)).ToDictionary(k => k.Name.RemoveEnd("Page"), v => v.AsType());
			var viewModelTypes = assembly.DefinedTypes.Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("ViewModel", StringComparison.Ordinal)).ToDictionary(k => k.Name.RemoveEnd("ViewModel"), v => v.AsType());

			foreach (var kvp in viewModelTypes)
			{
				if(!pageTypes.ContainsKey(kvp.Key))
					throw new ArgumentException($"{kvp.Value.Name} has no implemented page");

				_viewModelPageDictionary.Add(kvp.Value, pageTypes[kvp.Key]);
			}
		}

		async Task Lock(Func<Task> action)
		{
			await _locker.WaitAsync();
			try
			{
				await action.Invoke();
			}
			finally
			{
				_locker.Release();
			}
		}

		Task InvokeOnMainThread(Func<Task> action)
		{
			var tcs = new TaskCompletionSource<bool>();
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					await action.Invoke();
				}
				catch (Exception ex)
				{
					tcs.TrySetException(ex);
				}
				finally
				{
					tcs.TrySetResult(true);
				}
			});
			return tcs.Task;
		}

		public Task PushAsync<TViewModel>(TViewModel vm, bool animated = true) where TViewModel : BaseViewModel
		{
			return Lock(() =>
			{
				var pageType = default(Type);

				if (!_viewModelPageDictionary.TryGetValue(vm.GetType(), out pageType))
					throw new InvalidOperationException($"{vm.GetType().Name} has no implemented page");

				var page = (Page)Activator.CreateInstance(pageType);
				page.BindingContext = vm;

				return InvokeOnMainThread(() => _rootPage.PushAsync(page));
			});
		}

		public Task PopAsync(bool animated = true, int levels = 1)
		{
			return Lock(() =>
			{
				return InvokeOnMainThread(() =>
				{
					if(_rootPage.Navigation.NavigationStack.Count > levels)
						for (var i = 1; i < levels; ++i)
							_rootPage.Navigation.RemovePage(_rootPage.Navigation.NavigationStack[_rootPage.Navigation.NavigationStack.Count - 2]);
					
					return _rootPage.PopAsync(animated);
				});
			});
		}
	}
}