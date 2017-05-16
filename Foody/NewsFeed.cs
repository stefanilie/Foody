using System;
using System.Collections.Generic;
using UIKit;
namespace Foody
{
	public class NewsFeed : UIViewController
	{
		UIBarButtonItem done, edit;
		public NewsFeed()
		{
			//ToDO: crete table view, tableviewsource, tableviewcell.
			this.View.BackgroundColor = CostumColors.ORANGE;
		}

		public override void ViewDidLoad()
		{
			UITableView table = new UITableView(this.View.Bounds);
			List<string> items = new List<string> { "voma", "ketamina", "MDMA", "dava", "antigel", "lichid de frane", "voma", "ketamina", "MDMA", "dava", "antigel", "lichid de frane" };
			table.Source = new NewsFeedSource(items);
			var edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (sender, e) =>
			{
				if (table.Editing)
					table.SetEditing(false, true);
				table.SetEditing(true, true);
				NavigationItem.LeftBarButtonItem = null;
				NavigationItem.RightBarButtonItem = this.done;
				this.NavigationItem.SetRightBarButtonItem(this.done, true);
			});
			var done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, e) =>
			{
				table.SetEditing(false, true);
				this.NavigationItem.SetRightBarButtonItem(edit, true);
			});
			this.NavigationItem.SetRightBarButtonItem(edit, true);
			Add(table);

		}
	}
}
