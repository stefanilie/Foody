using System;
using UIKit;
namespace Foody
{
	public class NewsFeedSource : UITableViewSource
	{

		string[] tableItems;
		string CellIdentifier = "TableCell";

		public NewsFeedSource(string[] items)
		{
			tableItems = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Length;
		}


		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

			string item = tableItems[indexPath.Row];

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
			}

			cell.TextLabel.Text = item;

			return cell;
		}
	}
}
