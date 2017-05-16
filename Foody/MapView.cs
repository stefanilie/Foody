using System;
using CoreLocation;
using MapKit;
using UIKit;
namespace Foody
{
	public class MapView : UIViewController
	{
		private string title;
		public MapView(string title)
		{
			this.title = title;
		}

		public override void ViewDidLoad()
		{
			var map = new MKMapView(UIScreen.MainScreen.Bounds);
			map.MapType = MKMapType.Standard;
			map.ZoomEnabled = true;
			map.ScrollEnabled = true;
			CLLocationManager locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();
			map.ShowsUserLocation = true;
			map.AddAnnotations(new MKPointAnnotation()
			{
				Title = this.title,
				Coordinate = new CLLocationCoordinate2D(45.280770, 27.963129)
			});
			View = map;
		}
	}
}
