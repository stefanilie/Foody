using System;
using UIKit;
using CoreGraphics;
namespace Foody
{
	public class LoginView : UIViewController
	{
		public LoginView()
		{
		}

		public override void ViewDidLoad()
		{
			this.View.BackgroundColor = CostumColors.GRAY;
			UIButton btnLogin = UIButton.FromType(UIButtonType.Custom);
			btnLogin = new UIButton(new CGRect(this.View.Frame.GetMidX() - 100, this.View.Frame.GetMidY() - 25, 200, 50));
			btnLogin.SetTitle("Login", UIControlState.Normal);
			btnLogin.BackgroundColor = CostumColors.SKY_BLUE;
			btnLogin.SetTitleColor(CostumColors.ORANGE, UIControlState.Normal);
			btnLogin.TitleLabel.AdjustsFontSizeToFitWidth = true;

			this.View.AddSubview(btnLogin);
		}

	}
}
