using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;

namespace CollectionViewPlusUiDynamics
{
	public class SpringyCollectionViewFlowLayout : UICollectionViewFlowLayout
	{
		UIDynamicAnimator dynamicAnimator;
		int margin = 10;
		float latestDelta;
		public SpringyCollectionViewFlowLayout ()
		{
			this.MinimumInteritemSpacing = margin;

			this.MinimumLineSpacing = margin;
			this.ItemSize = new SizeF(100,100);
			dynamicAnimator = new UIDynamicAnimator (this);
		}
		public override void InvalidateLayout ()
		{
			base.InvalidateLayout ();
		}
		public override void PrepareLayout ()
		{


			base.PrepareLayout ();
			if (this.dynamicAnimator.Behaviors.Count () == 0) {
				var s = this.CollectionViewContentSize;
				var rec = new RectangleF (0, 0, s.Width, s.Height);
				foreach (var item in base.LayoutAttributesForElementsInRect(rec)) {
					UIAttachmentBehavior behaviour = new UIAttachmentBehavior (item, item.Center);
					behaviour.Length = 0.0f;
					behaviour.Damping = 0.8f;
					behaviour.Frequency = 1.0f;
					dynamicAnimator.AddBehavior (behaviour);
				}
			}
		}
		public override bool ShouldInvalidateLayoutForBoundsChange (RectangleF newBounds)
		{
						var scrollView = this.CollectionView;
						var delta = newBounds.X - scrollView.Bounds.Y;
			
						var touchLocation = this.CollectionView.PanGestureRecognizer.LocationInView (this.CollectionView);
							//[self.collectionView.panGestureRecognizer locationInView:self.collectionView];
						foreach (UIAttachmentBehavior springBehaviour in this.dynamicAnimator.Behaviors) {
							var yDistanceFromTouch = Math.Abs(touchLocation.Y - springBehaviour.AnchorPoint.Y);
							var xDistanceFromTouch = Math.Abs(touchLocation.X - springBehaviour.AnchorPoint.X);
							var scrollResistance = (yDistanceFromTouch + xDistanceFromTouch) / 1500.0f;
			
							var item = springBehaviour.Items.First();
							var center = item.Center;
							if (delta < 0)
								center.X += Math.Max (delta, delta * scrollResistance);
							else
								center.X += Math.Min (delta, delta * scrollResistance);
			
							item.Center = center;
							this.dynamicAnimator.UpdateItemUsingCurrentState (item);
							latestDelta = delta;
						}
			return false;

		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForItem (NSIndexPath path)
		{
				return this.dynamicAnimator.GetLayoutAttributesForCell (path);

		}



		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect (RectangleF rect)
		{
				return dynamicAnimator.GetDynamicItems (rect).Select(c=> 
				{
					return (c as UICollectionViewLayoutAttributes);
				}
			).ToArray();
		}
	}
}

