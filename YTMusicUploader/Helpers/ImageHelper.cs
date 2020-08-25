using System.Drawing;
using System.Linq;
using System.Security.Cryptography;

namespace JBToolkit.Imaging
{
    /// <summary>
    /// Image helper methods
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Get a SHA hash of an image (useful for comparing)
        /// </summary>
        public static byte[] ShaHash(this Image image)
        {
            var bytes = new byte[1];
            bytes = (byte[])(new ImageConverter()).ConvertTo(image, bytes.GetType());

            return (new SHA256Managed()).ComputeHash(bytes);
        }

        /// <summary>
        /// Checks if 1 image is the same as another by comparing its SHA hash. Also performs
        /// a quick check on image dimentions to avoid performs cost of unnecessarily generating
        /// a hash
        /// </summary>
        /// <returns>True if the same, false otherwise</returns>
        public static bool IsSameImage(Image imageA, Image imageB)
        {
            if (imageA.Width != imageB.Width) return false;
            if (imageA.Height != imageB.Height) return false;

            var hashA = imageA.ShaHash();
            var hashB = imageB.ShaHash();

            return !hashA
                .Where((nextByte, index) => nextByte != hashB[index])
                .Any();
        }
    }
}
