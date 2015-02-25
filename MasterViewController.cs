using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace TriviaManage
{
	public partial class MasterViewController : UITableViewController
	{
		DataSource dataSource;

		private QuestionInfo theQuestionInfo;

		public MasterViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Master", "Master");

			// Custom initialization
			theQuestionInfo = new QuestionInfo();
		}

		void AddNewItem (object sender, EventArgs args)
		{
			dataSource.Objects.Insert (0, DateTime.Now);

			using (var indexPath = NSIndexPath.FromRowSection (0, 0))
				TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView.Source = dataSource = new DataSource (this);

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, AddNewItem);
			NavigationItem.RightBarButtonItem = addButton;

			/*
			RefreshControl.ValueChanged += async (sender, e) =>
			{
				await RefreshAsync(true);
			};
			*/


			// Refresh the task list.
			await RefreshAsync(false, true);
		}

		private async Task RefreshAsync(bool pullDownActivated = false, bool forceProgressIndicator = false)
		{
			//RefreshControl.BeginRefreshing();

			Task resultTask = theQuestionInfo.RefreshQuestions();

			await UIUtilities.ShowIndeterminateProgressIfNecessary(resultTask, "Refreshing Questions...", pullDownActivated, forceProgressIndicator);

			//RefreshControl.EndRefreshing();

			TableView.ReloadData();

		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// Passing the collection to the source and assigning it to the table view
			TableView.Source = new RootTableSource(theQuestionInfo);
		}

		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("Cell");
			readonly List<object> objects = new List<object> ();
			readonly MasterViewController controller;

			public DataSource (MasterViewController controller)
			{
				this.controller = controller;
			}

			public IList<object> Objects {
				get { return objects; }
			}

			// Customize the number of sections in the table view.
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return objects.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (UITableViewCell)tableView.DequeueReusableCell (CellIdentifier, indexPath);

				cell.TextLabel.Text = objects [indexPath.Row].ToString ();

				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					// Delete the row from the data source.
					objects.RemoveAt (indexPath.Row);
					controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}

			/*
			// Override to support rearranging the table view.
			public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
			}
			*/

			/*
			// Override to support conditional rearranging of the table view.
			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the item to be re-orderable.
				return true;
			}
			*/
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "TaskSegue") {
				var theController = segue.DestinationViewController as QuestionDetailViewController;

				if (theController != null) {
					// This is the current table
					var source = TableView.Source as RootTableSource;

					// This is the row selected by the user
					var rowPath = TableView.IndexPathForSelectedRow;
					var item = source.GetItem(rowPath.Row);

					// Tell the view model the user is editing
					theQuestionInfo.CurrentUIMode = UIModes.Editing;

					// Set the task in the detail view to the task the user selected
					// Is defined on the TaskDetailViewController
					//theController.SetTask(this, item); 
				}
			}
		}

		public async void SaveTask(QuestionItem questionItem)
		{
			try
			{

				Task saveTask = theQuestionInfo.Save(questionItem);

				if (theQuestionInfo.CurrentUIMode == UIModes.Adding)
				{

					await UIUtilities.ShowIndeterminateProgressIfNecessary(saveTask, string.Format("Adding task: [{0}] ...", questionItem.questionText));
				}
				else
				{
					await UIUtilities.ShowIndeterminateProgressIfNecessary(saveTask, string.Format("Updating task: [{0}] ...", questionItem.questionText));
				}


				await RefreshAsync();


			}
			finally
			{
				NavigationController.PopViewControllerAnimated(true);
			}
		}

		public async void DeleteTask(QuestionItem questionItem)
		{
			try
			{
				Task deleteQuestion = theQuestionInfo.DeleteQuestion(questionItem);
				await UIUtilities.ShowIndeterminateProgressIfNecessary(deleteQuestion, string.Format("Deleting task: [{0}] ...", questionItem.questionText));

			}
			finally
			{
				NavigationController.PopViewControllerAnimated(true);
			}
		}

		public void CreateTask(object sender, EventArgs args)
		{
			// first, add the task to the underlying data
			var newQuestionItem = new QuestionItem();

			theQuestionInfo.CurrentUIMode = UIModes.Adding;

			// then open the detail view to edit it
			var detail = Storyboard.InstantiateViewController("QuestionDetailViewController") as QuestionDetailViewController;
			//detail.SetTask(this, newQuestionItem);
			NavigationController.PushViewController(detail, true);
		}
	}
}
