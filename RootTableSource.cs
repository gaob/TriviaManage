using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TriviaManage
{
	public class RootTableSource : UITableViewSource
	{

		string cellIdentifier = "QuestionCell"; // set in the Storyboard

		private QuestionInfo _questionInfo;

		public RootTableSource(QuestionInfo questionInfo)
		{
			_questionInfo = questionInfo;
		}
		public override int RowsInSection(UITableView tableview, int section)
		{
			if (_questionInfo.QuestionList != null)
			{
				return _questionInfo.QuestionList.Count;
			}
			else
			{
				return 0;
			}
		}
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// in a Storyboard, Dequeue will ALWAYS return a cell, 
			var cell = tableView.DequeueReusableCell(cellIdentifier);

			// now set the properties as normal
			cell.TextLabel.Text = (indexPath.Row + 1).ToString() + ". " + _questionInfo.QuestionList[indexPath.Row].QuestionText;

			/* to be deleted
			if (_questionInfo.QuestionList[indexPath.Row].CompletedFlag)
			{
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}
			else
			{
				cell.Accessory = UITableViewCellAccessory.None;
			}
			*/
			return cell;
		}
		public QuestionItem GetItem(int id)
		{
			return _questionInfo.QuestionList[id];
		}
		public override async void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{

			switch (editingStyle)
			{
			case UITableViewCellEditingStyle.Delete:

				// remove the item from the underlying data source
				await _questionInfo.DeleteQuestion(_questionInfo.QuestionList[indexPath.Row]);

				// delete the row from the table
				tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				break;
			case UITableViewCellEditingStyle.Insert:
				System.Diagnostics.Debug.WriteLine("CommitEditingStyle: Insert called");
				break;
			case UITableViewCellEditingStyle.None:
				System.Diagnostics.Debug.WriteLine("CommitEditingStyle: None called");
				break;
			}

		}
	}
}
