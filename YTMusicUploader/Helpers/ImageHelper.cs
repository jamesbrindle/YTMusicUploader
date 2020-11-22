using System.Data.HashFunction.xxHash;
using System.Drawing;
using System.Linq;

namespace JBToolkit.Imaging
{
    /// <summary>
    /// Image helper methods
    /// </summary>
    public static class ImageHelper
    {
        private static xxHashConfig XxHashConfig = new xxHashConfig() { HashSizeInBits = 32 };
        private static IxxHash XxHashFactory = xxHashFactory.Instance.Create(XxHashConfig);

        /// <summary>
        /// Get a 32bit xxHash of an image (useful for comparing)
        /// </summary>
        public static byte[] XxHash(this Image image)
        {
            var bytes = new byte[1];
            bytes = (byte[])new ImageConverter().ConvertTo(image, bytes.GetType());
            return XxHashFactory.ComputeHash(bytes).Hash;
        }

        /// <summary>
        /// Checks if 1 image is the same as another by comparing its SHA hash. Also performs
        /// a quick check on image dimentions to avoid performs cost of unnecessarily generating
        /// a hash
        /// </summary>
        /// <returns>True if the same, false otherwise</returns>
        public static bool IsSameImage(Image imageA, Image imageB)
        {
            try
            {
                if (imageA.Width != imageB.Width) return false;
                if (imageA.Height != imageB.Height) return false;

                var hashA = imageA.XxHash();
                var hashB = imageB.XxHash();

                return !hashA
                    .Where((nextByte, index) => nextByte != hashB[index])
                    .Any();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  Resizes an image
        /// </summary>
        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            var result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(bmp, 0, 0, width, height);

            return result;
        }
    }
}
