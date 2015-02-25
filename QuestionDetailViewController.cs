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

			Ttext.Text = currentQuestion.questionText;
			Tone.Text = currentQuestion.answerOne;
			Ttwo.Text = currentQuestion.answerTwo;
			Tthree.Text = currentQuestion.answerThree;
			Tfour.Text = currentQuestion.answerFour;
			Tidentifier.Text = currentQuestion.identifier;
		}

		partial void Bdelete_TouchUpInside (UIButton sender)
		{
			Delegate.DeleteQuestion(currentQuestion);
		}

		partial void Bsave_TouchUpInside (UIButton sender)
		{
			currentQuestion.questionText = Ttext.Text;
			currentQuestion.answerOne = Tone.Text;
			currentQuestion.answerTwo = Ttwo.Text;
			currentQuestion.answerThree = Tthree.Text;
			currentQuestion.answerFour = Tfour.Text;
			currentQuestion.identifier = Tidentifier.Text;

			Delegate.SaveQuestion(currentQuestion);
		}

		public void SetQuestion(MasterViewController d, QuestionItem question)
		{
			Delegate = d;
			currentQuestion = question;
		}
	}
}
