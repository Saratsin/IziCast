using System;
using Xamarin.Forms;

namespace IziCast.Controls
{
	public class CustomEntry : Entry
	{
		public static new readonly BindableProperty IsFocusedProperty = BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(CustomEntry), false, BindingMode.TwoWay, null, OnIsFocusedPropertyChanged);

		static void OnIsFocusedPropertyChanged(object bindable, object oldValue, object newValue)
		{
			var entry = (CustomEntry)bindable;
			var oldV = entry.GetBaseFocusedValue();
			var newV = (bool)newValue;

			if (oldV != newV)
			{
				if (newV)
					entry.Focus();
				else
					entry.Unfocus();
			}
		}

		bool GetBaseFocusedValue() => base.IsFocused;

		public new bool IsFocused
		{
			get { return (bool)GetValue(IsFocusedProperty); }
			set { SetValue(IsFocusedProperty, value); }
		}

		public CustomEntry()
		{
			Focused += OnFocusChanged;
			Unfocused += OnFocusChanged;
		}

		void OnFocusChanged(object sender, FocusEventArgs e)
		{
			if (e.IsFocused)
			{
				
			}
			else
			{
				
			}
		}
	}
}
