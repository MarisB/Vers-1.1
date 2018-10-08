using System;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ControlitFactory.Helpers
{
    public static class ImageHelper
    {
        public static UIImage ToImage(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            UIImage image = null;
            try
            {

                image = new UIImage(NSData.FromArray(data));
                data = null;
            }
            catch (Exception)
            {
                return null;
            }
            return image;
        }

        public static byte[] ToArray(UIImage image)
        {

            if (image == null)
            {
                return null;
            }
            NSData data = null;

            try
            {
                data = image.AsPNG();
                return data.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
                if (data != null)
                {
                    data.Dispose();
                    data = null;
                }
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CGSize((nfloat)width, (nfloat)height));
            sourceImage.Draw(new CGRect(0, 0, (nfloat)width, (nfloat)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return resultImage;
        }


    }
}
