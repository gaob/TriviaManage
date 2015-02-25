using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace TriviaManage
{
	partial class QuestionDetailViewController : UITableViewController
	{
		public QuestionDetailViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			//Initialize Textfields data.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Add support for the UI to dismiss the keyboard as it can cover up
			// save and delete buttons.
			Ttext.ShouldReturn += textField =>
			{
				textField.ResignFirstResponder();
				return true;
			};

			Tone.ShouldReturn += textField =>
			{
				textField.ResignFirstResponder();
				return true;
			};

			Ttwo.ShouldReturn += textField =>
			{
				textField.ResignFirstResponder();
				return true;
			};

			Tthree.ShouldReturn += textField =>
			{
				textField.ResignFirstResponder();
				return true;
			};

			Tfour.ShouldReturn += textField =>
			{
				textField.ResignFirstResponder();
				return true;
			};

			Tidentifier.ShouldReturn += textField =>
			{
				textField.ResignFirstResponder();
				return true;
			};
		}
	}
}
