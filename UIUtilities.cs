using System.Threading.Tasks;
using BigTed;

namespace TriviaManage
{
	/// <summary>
	/// User interface utilities to use BTProgressHUD component.
	/// </summary>
	public static class UIUtilities
	{
		/// <summary>
		/// Shows the indeterminate progress if necessary
		/// Completes the execution of the provided executingTask by awaiting on it.
		/// </summary>
		/// <param name="executingTask">The executing task.</param>
		/// <param name="message">The message.</param>
		/// <param name="pullDownActivated">if set to <c>true</c> [pull down activated].</param>
		/// <param name="forceProgressIndicator">if set to <c>true</c> [force progress indicator].</param>
		/// <returns>Task.</returns>
		/// <remarks>Does not return any results from the executing task</remarks>
		public static async Task ShowIndeterminateProgressIfNecessary(Task executingTask, string message, bool pullDownActivated = false, bool forceProgressIndicator = false)
		{
			bool showingIndeterminateProgress = forceProgressIndicator;

			try
			{
				if (!pullDownActivated || showingIndeterminateProgress)
				{
					if (!showingIndeterminateProgress)
					{
						// Only display the indeterminate progress indicator if it takes more than 1.5 seconds
						// to get the results
						Task feedbackTask = Task.Delay(1500);

						Task finishedTask = await Task.WhenAny(executingTask, feedbackTask);

						if (finishedTask == feedbackTask)
						{
							showingIndeterminateProgress = true;
						}
					}

					if (showingIndeterminateProgress)
					{
						// Display an indeterminate progress bar indicating that the tasks are being refreshed.
						BTProgressHUD.Show(message);
					}
				}

				await executingTask;
			}
			finally
			{
				if (showingIndeterminateProgress)
				{
					BTProgressHUD.Dismiss();
				}
			}

		}
	}
}
