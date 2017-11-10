using OpenCvSharp;

namespace OpenCvFindDifferent
{
    class Program
    {
        //test image from https://play.google.com/store/apps/details?id=com.vizalevgames.finddifferences200levels
        //thank for that
        static void Main()
        {
            var org1 = new Mat("1.jpg");
            var org2 = new Mat("2.jpg");

            //create background subtraction method
            var mog = BackgroundSubtractorMOG2.Create();
            var mask = new Mat();
            mog.Apply(org1, mask);
            mog.Apply(org2, mask);

            //reduce noise
            Cv2.MorphologyEx(mask,mask,MorphTypes.Open, null,null,2);

            //convert mask from gray to BGR for AddWeighted function
            var maskBgr = new Mat();
            Cv2.CvtColor(mask, maskBgr, ColorConversionCodes.GRAY2BGR);

            //apply two image as one
            Cv2.AddWeighted(org1, 1.0, maskBgr, 0.5, 2.2, org1);
            Cv2.AddWeighted(org2, 1.0, maskBgr, 0.5, 2.2, org2);

            #region draw contours
            var canny = new Mat();
            Cv2.Canny(mask, canny, 15, 120);
            Cv2.FindContours(canny, out var contours, out var _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            Cv2.DrawContours(org1, contours, -1, Scalar.Red, 2);
            Cv2.DrawContours(org2, contours, -1, Scalar.Red, 2);
            #endregion

            using (new Window("org1", org1))
            using (new Window("org2", org2))
            using (new Window("mask", mask))
            {
                Cv2.WaitKey();
            }
        }
    }
}
