﻿using System;
using Xamarin.Forms;

namespace Roget1911
{
	public class SectionListPage : ContentPage
	{
		ListView listView;
		public SectionListPage ()
		{
			NavigationPage.SetHasNavigationBar (this, true);
            if (Device.OS != TargetPlatform.Windows)
            { // HACK: this doesn't look right on Windows
                listView = new ListView { RowHeight = 40 };
            }
            else
            {
                listView = new ListView();
            }

			listView.SetBinding (ListView.ItemsSourceProperty, ".");
			listView.ItemTemplate = new DataTemplate (typeof (TextCell)) {
				Bindings = {
					{ TextCell.TextProperty, new Binding ("Name") }
				}
			};

			listView.ItemSelected += (sender, e) => {
				if (e.SelectedItem == null) return;
				listView.SelectedItem = null; // clear out for next rendering of list

				var section = (RogetSection)e.SelectedItem;
				if (section.Sections.Count == 0)
				{ // show workds
					var categoryListPage = new CategoryListPage(section);
					categoryListPage.Title = section.Name;
					Navigation.PushAsync(categoryListPage);
				} else { // show more sections hierarchy
					var sectionListPage = new SectionListPage();
					sectionListPage.Title = section.Name;
					sectionListPage.BindingContext = section.Sections;
					Navigation.PushAsync(sectionListPage);
				}
			};

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {listView}
			};

		}
	}
}

