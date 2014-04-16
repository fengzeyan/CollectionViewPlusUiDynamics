using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace CollectionViewPlusUiDynamics
{
	public partial class CollectionViewPlusUiDynamicsViewController : UIViewController
	{
		public CollectionViewPlusUiDynamicsViewController (IntPtr handle) : base (handle)
		{
		}
		static NSString gridCellId = new NSString ("GridViewCell");
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		UICollectionView collectionView;

		#region View lifecycle

		public override void ViewDidLoad ()
		{

			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			collectionView = new UICollectionView (View.Bounds,new SpringyCollectionViewFlowLayout());
		
			collectionView.BackgroundColor = UIColor.Red;
			collectionView.DataSource = new TableSource ();
			collectionView.RegisterClassForCell (typeof(GridViewItemCell),gridCellId);
				this.Add (collectionView);
		
		}

		public override void ViewWillAppear (bool animated)
		{
			collectionView.CollectionViewLayout.InvalidateLayout ();
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion


		public class TableSource : UICollectionViewDataSource
		{
			static NSString gridCellId = new NSString ("GridViewCell");

			Random rand = new Random (1);
			public List<string> items = new List<string> ();
			public TableSource ()
			{
				for (int i = 0; i < 1000; i++) {

					items.Add ( "hello " + i);
				}

			}


			#region implemented abstract members of UICollectionViewDataSource
			public override int GetItemsCount (UICollectionView collectionView, int section)
			{
				return items.Count;
			}
			public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
			{
				var cell = (GridViewItemCell)collectionView.DequeueReusableCell (gridCellId, indexPath);

				var item = items [indexPath.Row];
				cell.Text = item;

				return cell;
			}
			#endregion


		}
	}

	public class GridViewItemCell : UICollectionViewCell
	{

		public override void Draw (System.Drawing.RectangleF rect)
		{
			base.Draw (rect);
		}
		UILabel txtView;

		[Export ("initWithFrame:")]
		public GridViewItemCell (System.Drawing.RectangleF frame) : base (frame)
		{
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.ContentMode = UIViewContentMode.ScaleAspectFill;

		
			txtView = new UILabel (new RectangleF(10,10,300,30));
			txtView.TextColor = UIColor.White;
			txtView.Font = UIFont.FromName("Helvetica-Bold", 20f);


			ContentView.AddSubview (txtView);
		}

		public string Text {
			set {
				txtView.Text = value;
			}
		}

	
	}
}

