using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Drawing;
using System.Data;

namespace Foody
{
	public class LoginView : UIViewController
	{
		UITextField txtUserName, txtPassword;
		UIView myView;
		private string _pathToDB { get; set; }
		UIButton btnLogin, btnSignUp;
		private float scroll_amount = 0.0f;    // amount to scroll 
		private float bottom = 0.0f;           // bottom point
		private float offset = -150.0f;          // extra offset
		private bool moveViewUp = false;           // which direction are we moving

		public LoginView()
		{
		}

		public void SetupTextfields()
		{
			txtUserName = new UITextField(new CGRect(this.View.Frame.GetMidX() - 75, 120, 150, 40));
			txtUserName.Placeholder = "username";
			txtUserName.TextColor = CostumColors.PURPLE;
			txtUserName.TextAlignment = UITextAlignment.Center;
			txtUserName.AdjustsFontSizeToFitWidth = true;
			txtUserName.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			txtPassword = new UITextField(new CGRect(this.View.Frame.GetMidX() - 75, 170, 150, 40));
			txtPassword.Placeholder = "password";
			txtPassword.TextColor = CostumColors.PURPLE;
			txtPassword.SecureTextEntry = true;
			txtPassword.TextAlignment = UITextAlignment.Center;
			txtPassword.AdjustsFontSizeToFitWidth = false;
			txtPassword.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

		}

		public void SetupButtons()
		{
			btnLogin = UIButton.FromType(UIButtonType.Custom);
			btnLogin = new UIButton(new CGRect(15, 240, View.Frame.GetMidX() - 20, 40));
			btnLogin.SetTitle("Login", UIControlState.Normal);
			btnLogin.BackgroundColor = CostumColors.SKY_BLUE;
			btnLogin.SetTitleColor(CostumColors.ORANGE, UIControlState.Normal);
			btnLogin.SetTitleColor(CostumColors.PURPLE, UIControlState.Selected);
			btnLogin.TitleLabel.AdjustsFontSizeToFitWidth = true;
			btnLogin.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			btnSignUp = UIButton.FromType(UIButtonType.Custom);
			btnSignUp = new UIButton(new CGRect(View.Frame.GetMidX() + 10, 240, View.Frame.GetMidX() - 20, 40));
			btnSignUp.SetTitle("SignUp", UIControlState.Normal);
			btnSignUp.BackgroundColor = CostumColors.SKY_BLUE;
			btnSignUp.SetTitleColor(CostumColors.ORANGE, UIControlState.Normal);
			btnSignUp.TitleLabel.AdjustsFontSizeToFitWidth = true;
			btnSignUp.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

		}

		public override void ViewDidLoad()
		{
			this.myView = new UIView(this.View.Frame);
			View.AddSubview(myView);
			SetupButtons();
			SetupTextfields();
			this.myView.BackgroundColor = CostumColors.GRAY;
			this.myView.Add(txtUserName);
			this.myView.Add(txtPassword);
			this.myView.Add(btnLogin);
			this.myView.Add(btnSignUp);

			btnLogin.TouchUpInside += (sender, e) =>
			{
				
				NavigatorController.instance.PushViewController(new NewsFeed(), true);
			};


			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyBoardUpNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);
		}

		#region scroll for keyboard
		private void KeyBoardUpNotification(NSNotification notification)
		{
			if (moveViewUp)
				return;
			// get the keyboard size
			RectangleF r = (RectangleF)UIKeyboard.BoundsFromNotification(notification);

			// Find what opened the keyboard
			foreach (UIView view in this.View.Subviews)
			{
				if (view.IsFirstResponder)
					myView = view;
			}

			// Bottom of the controller = initial position + height + offset      
			bottom = (float)(myView.Frame.Y + myView.Frame.Height + offset);

			// Calculate how far we need to scroll
			scroll_amount = (float)(r.Height - (View.Frame.Size.Height - bottom));

			// Perform the scrolling
			if (scroll_amount > 0)
			{
				moveViewUp = true;
				ScrollTheView(moveViewUp);
			}
			else
			{
				moveViewUp = false;
			}

		}

		private void KeyBoardDownNotification(NSNotification notification)
		{
			if (moveViewUp)
			{
				moveViewUp = false;
				ScrollTheView(false);
			}
		}

		private void ScrollTheView(bool move)
		{

			// scroll the view up or down
			UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
			UIView.SetAnimationDuration(0.3);

			RectangleF frame = (RectangleF)View.Frame;

			if (move)
			{
				frame.Y -= scroll_amount;
			}
			else
			{
				frame.Y += scroll_amount;
				scroll_amount = 0;
			}

			View.Frame = frame;
			UIView.CommitAnimations();
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			this.txtPassword.ResignFirstResponder();
			this.txtUserName.ResignFirstResponder();
		}

	#endregion
	}
}
