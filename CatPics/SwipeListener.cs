using System;
using Xamarin.Forms;

namespace CatPics {

    public class SwipeGestureRecognizer : PanGestureRecognizer {
        private ISwipeCallBack mISwipeCallback;
        private double translatedX = 0, translatedY = 0;

        public SwipeGestureRecognizer(View view, ISwipeCallBack iSwipeCallBack) {
            mISwipeCallback = iSwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e) {

            View Content = (View)sender;

            switch (e.StatusType) {

                case GestureStatus.Running:

                    try {
                        translatedX = e.TotalX;
                        translatedY = e.TotalY;
                    }
                    catch (Exception err) {
                        System.Diagnostics.Debug.WriteLine("" + err.Message);
                    }
                    break;

                case GestureStatus.Completed:
                    //System.Diagnostics.Debug.WriteLine("translatedX : " + translatedX);
                    //System.Diagnostics.Debug.WriteLine("translatedY : " + translatedY);

                    if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY)) {
                        mISwipeCallback.onLeftSwipe(Content);
                    }
                    else if (translatedX > 0 && translatedX > Math.Abs(translatedY)) {
                        mISwipeCallback.onRightSwipe(Content);
                    }
                    else if (translatedY < 0 && Math.Abs(translatedY) > Math.Abs(translatedX)) {
                        mISwipeCallback.onTopSwipe(Content);
                    }
                    else if (translatedY > 0 && translatedY > Math.Abs(translatedX)) {
                        mISwipeCallback.onBottomSwipe(Content);
                    }
                    else {
                        mISwipeCallback.onNothingSwiped(Content);
                    }

                    break;

            }
        }

    }

    public interface ISwipeCallBack {

        void onLeftSwipe(View view);
        void onRightSwipe(View view);
        void onTopSwipe(View view);
        void onBottomSwipe(View view);
        void onNothingSwiped(View view);
    }

    public class SwipeCallBack : ISwipeCallBack{

        public Action<View> OnLeftSwipeFunc { get; set; }
        public Action<View> OnRightSwipeFunc { get; set; }
        public Action<View> OnTopSwipeFunc { get; set; }
        public Action<View> OnBottomSwipeFunc { get; set; }
        public Action<View> OnNothingSwipedFunc { get; set; }

        public void onLeftSwipe(View view){
            OnLeftSwipeFunc?.Invoke(view);
        }

        public void onRightSwipe(View view){
            OnRightSwipeFunc?.Invoke(view);
        }

        public void onTopSwipe(View view){
            OnTopSwipeFunc?.Invoke(view);
        }

        public void onBottomSwipe(View view){
            OnBottomSwipeFunc?.Invoke(view);
        }

        public void onNothingSwiped(View view){
            OnNothingSwipedFunc?.Invoke(view);
        }
    }

}
