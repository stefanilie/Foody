using System;
using UIKit;
using System.Drawing;
using CoreGraphics;
using Foundation;

namespace Foody
{
	public class SignUpView : UIViewController
	{
		UITextField txtUsername, txtPassword, txtConfirmPassword, txtFullname;
		UIButton btnSubmit;
		private float scroll_amount = 0.0f;    // amount to scroll 
		private float bottom = 0.0f;           // bottom point
		private float offset = -150.0f;          // extra offset
		private bool moveViewUp = false;           // which direction are we moving
		UIView myView;
		string pathDB;

		public SignUpView()
		{

		}

		public SignUpView(string DBpath)
		{
			this.pathDB = DBpath;
		}

		public SignUpView(bool parse)
		{
			
		}

		public void setupUI()
		{
			var x = this.View.Frame.GetMidX();
			var diff = this.View.Frame.Width - 30;
			this.txtUsername = new UITextField(new CGRect(x - 75, 165, 150, 40));
			this.txtPassword = new UITextField(new CGRect(x - 75, 210, 150, 40));
			this.txtConfirmPassword = new UITextField(new CGRect(x - 75, 255, 150, 40));
			this.btnSubmit = new UIButton(new CGRect(x - (diff / 2), 300, diff, 40));

			this.txtUsername.Placeholder = "username";
			this.txtUsername.TextColor = CostumColors.ORANGE;
			this.txtUsername.TextAlignment = UITextAlignment.Center;

			this.txtPassword.Placeholder = "password";
			this.txtPassword.TextColor = CostumColors.ORANGE;
			this.txtPassword.TextAlignment = UITextAlignment.Center;
			this.txtPassword.SecureTextEntry = true;

			this.txtConfirmPassword.Placeholder = "confirm password";
			this.txtConfirmPassword.TextColor = CostumColors.ORANGE;
			this.txtConfirmPassword.TextAlignment = UITextAlignment.Center;
			this.txtConfirmPassword.SecureTextEntry = true;

			this.btnSubmit.SetTitleColor(CostumColors.PURPLE, UIControlState.Normal);
			this.btnSubmit.BackgroundColor = CostumColors.SKY_BLUE;
			this.btnSubmit.SetTitle("Submit", UIControlState.Normal);
			this.btnSubmit.Font = UIFont.FromName("Helvetica-Bold", 20f);

			this.myView.Add(this.txtUsername);
			this.myView.Add(this.txtPassword);
			this.myView.Add(this.txtConfirmPassword);
			this.myView.Add(this.btnSubmit);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View.BackgroundColor = UIColor.LightGray;
			NavigatorController.instance.Title = "Sign Up";

			myView = new UIView(this.View.Frame);
			View.AddSubview(myView);
			this.setupUI();

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyBoardUpNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);


			txtUsername.ShouldReturn += (textField) =>
			{
				txtUsername.ResignFirstResponder();
				return true;
			};
			txtConfirmPassword.ShouldReturn += (textField) =>
			{
				txtConfirmPassword.ResignFirstResponder();
				return true;
			};
			txtPassword.ShouldReturn += (textField) =>
			{
				txtPassword.ResignFirstResponder();
				return true;
			};

			btnSubmit.TouchUpInside += (object sender, EventArgs e) =>
			{
				UserModel newUser;
				try
				{
					if (txtUsername.Text == "" ||
						txtPassword.Text == "" ||
						txtConfirmPassword.Text == "")
						throw new Exception("All fields are mandatory!", null);

					if (txtUsername.Text.Length < 4 || txtUsername.Text.Contains(";"))
					{
						throw new Exception("Username must be at least 4 characters long and should not contain ;", null);
					}

					if (this.txtPassword.Text != this.txtConfirmPassword.Text)
					{
						throw new Exception("Passwords must match!", null);
					}
					newUser = new UserModel(txtUsername.Text, txtPassword.Text);
					try
					{
						using (var connection = new SQLite.SQLiteConnection(this.pathDB))
						{
							connection.Insert(newUser);
							//newUser.printUser();
							NavigatorController.instance.PopViewController(true);
						}
					}
					catch (Exception ex)
					{
						new UIAlertView("Error!", "There seems to be a problem with the DB!",
							null, "OK", null).Show();
						Console.WriteLine(ex.Message.ToString());
					}
				}
				catch (Exception exc)
				{
					new UIAlertView("Error!", exc.Message,
						null, "OK", null).Show();
				}
			};
		}

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

			this.txtUsername.ResignFirstResponder();
			this.txtPassword.ResignFirstResponder();
			this.txtConfirmPassword.ResignFirstResponder();

		}
	}
}

