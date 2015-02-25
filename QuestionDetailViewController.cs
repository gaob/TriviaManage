using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace TriviaManage
{
	partial class QuestionDetailViewController : UITableViewController
	{
		public MasterViewController Delegate { get; set; } // will be used to Save, Delete later
		QuestionItem currentQuestion { get; set; }

		public QuestionDetailViewController (IntPtr handle) : base (handle)
		{
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

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Ttext.Text = currentQuestion.QuestionText;
			Tone.Text = currentQuestion.AnswerOne;
			Ttwo.Text = currentQuestion.AnswerTwo;
			Tthree.Text = currentQuestion.AnswerThree;
			Tfour.Text = currentQuestion.AnswerFour;
			Tidentifier.Text = currentQuestion.Identifier;
		}

		partial void Bdelete_TouchUpInside (UIButton sender)
		{
			Delegate.DeleteQuestion(currentQuestion);
		}

		partial void Bsave_TouchUpInside (UIButton sender)
		{
			currentQuestion.QuestionText = Ttext.Text;
			currentQuestion.AnswerOne = Tone.Text;
			currentQuestion.AnswerTwo = Ttwo.Text;
			currentQuestion.AnswerThree = Tthree.Text;
			currentQuestion.AnswerFour = Tfour.Text;
			currentQuestion.Identifier = Tidentifier.Text;

			Delegate.SaveQuestion(currentQuestion);
		}

		public void SetQuestion(MasterViewController d, QuestionItem question)
		{
			Delegate = d;
			currentQuestion = question;
		}
	}
}
