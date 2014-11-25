using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MessagingSample
{
	public class MainPage : ContentPage
	{
		private List<string> _eventTimes;
		private bool _isSubscribed = false;
		private ListView _eventList;

		public MainPage ()
		{
			_eventTimes = new List<string> ();

			var clearButton = new Button {
				Text = "Clear"
			};

			clearButton.Clicked += (sender, e) => {
				_eventTimes.Clear();
				UpdateList();
			};

			var publishButton = new Button {
				Text = "Publish"
			};

			publishButton.Clicked += (sender, e) => {
				MessagingCenter.Send<MainPage, DateTime>(this, "boom", DateTime.Now);
			};

			var subUnsubButton = new Button {
				Text = "Subscribe"
			};

			subUnsubButton.Clicked += (sender, e) => {
				_isSubscribed = !_isSubscribed;

				if(_isSubscribed) {
					subUnsubButton.Text = "Unsubscribe";
					MessagingCenter.Subscribe<MainPage, DateTime>(this, "boom", (page, time) => {
						_eventTimes.Add(time.ToString());
						UpdateList();
					});
				}else {
					subUnsubButton.Text = "Subscribe";
					MessagingCenter.Unsubscribe<MainPage, DateTime>(this, "boom");
				}
			};

			var buttonStack = new StackLayout {
				Spacing = 20,
				Padding = 20,
				Orientation = StackOrientation.Horizontal,
				Children = { publishButton, subUnsubButton, clearButton },
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			_eventList = new ListView {
				ItemsSource = new ObservableCollection<string>(_eventTimes)
			};

			var mainStackLayout = new StackLayout {
				Children = { buttonStack, _eventList },
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			Content = mainStackLayout;
		}

		private void UpdateList() {
			_eventList.ItemsSource = new ObservableCollection<string> (_eventTimes);
		}
	}
}