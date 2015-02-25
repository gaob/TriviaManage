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
		private QuestionInfo theQuestionInfo;

		public MasterViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("TriviaManage", "TriviaManage");

			// Custom initialization
			theQuestionInfo = new QuestionInfo();
		}

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView.Source = new DataSource (this);

			//Add button to add question.
			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, CreateQuestion);
			NavigationItem.RightBarButtonItem = addButton;

			// Refresh the question list.
			await RefreshAsync(false, true);
		}

		/// <summary>
		/// Refreshs the table.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="pullDownActivated">If set to <c>true</c> pull down activated.</param>
		/// <param name="forceProgressIndicator">If set to <c>true</c> force progress indicator.</param>
		private async Task RefreshAsync(bool pullDownActivated = false, bool forceProgressIndicator = false)
		{
			Task resultTask = theQuestionInfo.RefreshQuestions();

			await UIUtilities.ShowIndeterminateProgressIfNecessary(resultTask, "Refreshing Questions...", pullDownActivated, forceProgressIndicator);

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
		}

		/// <summary>
		/// Function to be called after clicking on a question.
		/// </summary>
		/// <param name="segue">Segue.</param>
		/// <param name="sender">Sender.</param>
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

					// Set the question in the detail view to the question the user selected
					theController.SetQuestion(this, item); 
				}
			}
		}

		/// <summary>
		/// Interface to save the question.
		/// </summary>
		/// <param name="questionItem">Question item.</param>
		public async void SaveQuestion(QuestionItem questionItem)
		{
			try
			{
				Task saveTask = theQuestionInfo.Save(questionItem);

				if (theQuestionInfo.CurrentUIMode == UIModes.Adding)
				{
					await UIUtilities.ShowIndeterminateProgressIfNecessary(saveTask, string.Format("Adding task: [{0}] ...", questionItem.QuestionText));
				}
				else
				{
					await UIUtilities.ShowIndeterminateProgressIfNecessary(saveTask, string.Format("Updating task: [{0}] ...", questionItem.QuestionText));
				}

				await RefreshAsync();
			}
			finally
			{
				NavigationController.PopViewControllerAnimated(true);
			}
		}

		/// <summary>
		/// Interface to delete the question.
		/// </summary>
		/// <param name="questionItem">Question item.</param>
		public async void DeleteQuestion(QuestionItem questionItem)
		{
			try
			{
				Task deleteQuestion = theQuestionInfo.DeleteQuestion(questionItem);
				await UIUtilities.ShowIndeterminateProgressIfNecessary(deleteQuestion, string.Format("Deleting task: [{0}] ...", questionItem.QuestionText));
			}
			finally
			{
				NavigationController.PopViewControllerAnimated(true);
			}
		}

		/// <summary>
		/// Interface to create the question.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		public void CreateQuestion(object sender, EventArgs args)
		{
			// first, add the question to the underlying data
			var newQuestionItem = new QuestionItem();

			theQuestionInfo.CurrentUIMode = UIModes.Adding;

			// then open the detail view to edit it
			var detail = Storyboard.InstantiateViewController("QuestionDetailViewController") as QuestionDetailViewController;
			detail.SetQuestion(this, newQuestionItem);
			NavigationController.PushViewController(detail, true);
		}
	}
}
