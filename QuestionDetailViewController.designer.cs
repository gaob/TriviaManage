// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace TriviaManage
{
	[Register ("QuestionDetailViewController")]
	partial class QuestionDetailViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Bdelete { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Bsave { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Tfour { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Tidentifier { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Tone { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Ttext { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Tthree { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Ttwo { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Bdelete != null) {
				Bdelete.Dispose ();
				Bdelete = null;
			}
			if (Bsave != null) {
				Bsave.Dispose ();
				Bsave = null;
			}
			if (Tfour != null) {
				Tfour.Dispose ();
				Tfour = null;
			}
			if (Tidentifier != null) {
				Tidentifier.Dispose ();
				Tidentifier = null;
			}
			if (Tone != null) {
				Tone.Dispose ();
				Tone = null;
			}
			if (Ttext != null) {
				Ttext.Dispose ();
				Ttext = null;
			}
			if (Tthree != null) {
				Tthree.Dispose ();
				Tthree = null;
			}
			if (Ttwo != null) {
				Ttwo.Dispose ();
				Ttwo = null;
			}
		}
	}
}
