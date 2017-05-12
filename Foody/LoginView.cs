using System;
using UIKit;
using CoreGraphics;
using System.Drawing;
using System.IO;
using SQLite;
using Foundation;

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
		public SQLiteConnection connection;

		public LoginView()
		{
		}

		public void setupDB()
		{
			var documents = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
			this._pathToDB = Path.Combine(documents, "movie_db_sqlite-net.db");
			Console.WriteLine("path to db: " + this._pathToDB.ToString());
			using (this.connection = new SQLiteConnection(_pathToDB))
			{
				//this.connection.DropTable<UserModel>();
				var res = connection.Query<UserModel>("SELECT name FROM sqlite_master WHERE type='table' AND name='Users'");
				if (res.Count == 0)
					this.connection.CreateTable<UserModel>();
				Console.WriteLine("Database created successfully!");
			}
		}

		public void SetupTextfields()
		{
			txtUserName = new UITextField(new CGRect(this.View.Frame.GetMidX() - 75, 170, 150, 40));
			txtUserName.Placeholder = "username";
			txtUserName.TextColor = CostumColors.ORANGE;
			txtUserName.TextAlignment = UITextAlignment.Center;
			txtUserName.AdjustsFontSizeToFitWidth = true;
			txtUserName.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			txtPassword = new UITextField(new CGRect(this.View.Frame.GetMidX() - 75, 220, 150, 40));
			txtPassword.Placeholder = "password";
			txtPassword.TextColor = CostumColors.ORANGE;
			txtPassword.SecureTextEntry = true;
			txtPassword.TextAlignment = UITextAlignment.Center;
			txtPassword.AdjustsFontSizeToFitWidth = false;
			txtPassword.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

		}

		public void SetupButtons()
		{
			btnLogin = UIButton.FromType(UIButtonType.Custom);
			btnLogin = new UIButton(new CGRect(15, 280, View.Frame.GetMidX() - 20, 40));
			btnLogin.SetTitle("Login", UIControlState.Normal);
			btnLogin.BackgroundColor = CostumColors.SKY_BLUE;
			btnLogin.SetTitleColor(CostumColors.PURPLE, UIControlState.Normal);
			btnLogin.TitleLabel.AdjustsFontSizeToFitWidth = true;
			btnLogin.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			btnLogin.Font = UIFont.FromName("Helvetica-Bold", 20f);

			btnSignUp = UIButton.FromType(UIButtonType.Custom);
			btnSignUp = new UIButton(new CGRect(View.Frame.GetMidX() + 10, 280, View.Frame.GetMidX() - 20, 40));
			btnSignUp.SetTitle("SignUp", UIControlState.Normal);
			btnSignUp.BackgroundColor = CostumColors.SKY_BLUE;
			btnSignUp.SetTitleColor(CostumColors.PURPLE, UIControlState.Normal);
			btnSignUp.TitleLabel.AdjustsFontSizeToFitWidth = true;
			btnSignUp.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			btnSignUp.Font = UIFont.FromName("Helvetica-Bold", 20f);

		}

		public override void ViewDidLoad()
		{
			this.myView = new UIView(this.View.Frame);
			setupDB();
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
				bool isUser = false;
				try
				{
					using (this.connection = new SQLiteConnection(_pathToDB))
					{
						var users = connection.Query<UserModel>("SELECT * FROM Users");
						foreach (var user in users)
						{
							if (user.username == txtUserName.Text)
							{
								if (user.password == this.txtPassword.Text)
									isUser = true;
							}
						}
						//Console.WriteLine("User retreived:"+users.ToString());
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("EROARE DE SELECT!:" + ex.Message);
				}
				if (isUser)
				{
					NavigatorController.instance.PushViewController(new NewsFeed(), true);
				}
				else
				{
					new UIAlertView("Error!", "Username or password incorrect!",
						null, "OK", null).Show();
				}

			};

			btnSignUp.TouchUpInside += (sender, e) =>
			{
				Console.WriteLine(this._pathToDB);
				NavigatorController.instance.PushViewController(new SignUpView(this._pathToDB), true);
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
