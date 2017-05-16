using System;
using System.Collections.Generic;
using UIKit;
namespace Foody
{
	public class NewsFeedSource : UITableViewSource
	{

		List<string> tableItems;
		string CellIdentifier = "TableCell";

		public NewsFeedSource(List<string> items)
		{
			tableItems = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Count;
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

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			NavigatorController.instance.PushViewController(new MapView(tableItems[indexPath.Row]), true);
			tableView.DeselectRow(indexPath, true);
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
				case UITableViewCellEditingStyle.Delete:
					tableItems.RemoveAt(indexPath.Row);
					tableView.DeleteRows(new Foundation.NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					break;
				case UITableViewCellEditingStyle.None:
					Console.WriteLine("CommitEditingStyle:None called");
					break;
			}
		}

		public override bool CanEditRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return true;
		}

		public override bool CanMoveRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return true;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.Delete; // this example doesn't suppport Insert
		}

		public override string TitleForDeleteConfirmation(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return base.TitleForDeleteConfirmation(tableView, indexPath);
		}

		public override void MoveRow(UITableView tableView, Foundation.NSIndexPath sourceIndexPath, Foundation.NSIndexPath destinationIndexPath)
		{
			var item = tableItems[sourceIndexPath.Row];
			var deleteAt = sourceIndexPath.Row;
			var insertAt = destinationIndexPath.Row;

			// are we inserting 
			if (destinationIndexPath.Row < sourceIndexPath.Row)
			{
				// add one to where we delete, because we're increasing the index by inserting
				deleteAt += 1;
			}
			else
			{
				// add one to where we insert, because we haven't deleted the original yet
				insertAt += 1;
			}
			tableItems.Insert(insertAt, item);
			tableItems.RemoveAt(deleteAt);

		}
	}
}
