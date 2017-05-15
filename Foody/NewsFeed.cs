using System;
using UIKit;
namespace Foody
{
	public class NewsFeed : UIViewController
	{
		public NewsFeed()
		{
			//ToDO: crete table view, tableviewsource, tableviewcell.
			this.View.BackgroundColor = CostumColors.ORANGE;
		}

		public override void ViewDidLoad()
		{
			UITableView table = new UITableView(this.View.Bounds);
			string[] items = new string[] { "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma", "voma" };
			table.Source = new NewsFeedSource(items);
			Add(table);
			base.ViewDidLoad();
		}
	}
}
