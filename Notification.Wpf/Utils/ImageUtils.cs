using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Notification.Wpf.Utils
{
    /// <summary>
    /// Helper class to provide common Wpf image operations.
    /// </summary>
    public class ImageUtils
    {
        /// <summary>
        /// Converts a <see cref="BitmapSource"/> to encoded raw image bytes.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <returns>A png encoded array of raw bytes.</returns>
        public static byte[] BitmapSourceToBytes(BitmapSource image)
        {
            var encoder = new PngBitmapEncoder();
            var frame = BitmapFrame.Create(image);
            encoder.Frames.Add(frame);
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Converts raw image data into an <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="image">The raw image data.</param>
        /// <returns>A Wpf <see cref="ImageSource"/>.</returns>
        public static ImageSource BytesToImageSource(byte[] image)
        {
            try
            {
                using (var ms = new MemoryStream(image))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts raw image data into an <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="image">The raw image data.</param>
        /// <param name="maxWidth">The maximum image width.</param>
        /// <param name="maxHeight">The maximum image height.</param>
        /// <returns>A Wpf <see cref="ImageSource"/>.</returns>
        public static ImageSource BytesToImageSource(byte[] image, int maxWidth, int maxHeight)
        {
            try
            {
                using (var ms = new MemoryStream(image))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;

                    if (maxWidth > maxHeight)
                    {
                        bitmapImage.DecodePixelWidth = maxWidth;
                    }
                    else
                    {
                        bitmapImage.DecodePixelHeight = maxHeight;
                    }

                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a DeviceIndependentBitmap <see cref="MemoryStream"/> from the Clipboard to an <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="ms">The DIB <see cref="MemoryStream"/> to convert.</param>
        /// <returns>A <see cref="ImageSource"/> containing the DIB image.</returns>
        /// <remarks>
        /// Adapted from https://thomaslevesque.com/2009/02/05/wpf-paste-an-image-from-the-clipboard/.
        /// </remarks>
        public static ImageSource ImageFromClipboardDib(MemoryStream ms)
        {
            if (ms != null)
            {
                byte[] dibBuffer = new byte[ms.Length];
                ms.Read(dibBuffer, 0, dibBuffer.Length);

                BITMAPINFOHEADER infoHeader =
                    BinaryStructConverter.FromByteArray<BITMAPINFOHEADER>(dibBuffer);

                int fileHeaderSize = Marshal.SizeOf(typeof(BITMAPFILEHEADER));
                int infoHeaderSize = infoHeader.biSize;
                int fileSize = fileHeaderSize + infoHeader.biSize + infoHeader.biSizeImage;

                BITMAPFILEHEADER fileHeader = new BITMAPFILEHEADER
                {
                    bfType = BITMAPFILEHEADER.BM,
                    bfSize = fileSize,
                    bfReserved1 = 0,
                    bfReserved2 = 0,
                    bfOffBits = fileHeaderSize + infoHeaderSize + (infoHeader.biClrUsed * 4),
                };

                byte[] fileHeaderBytes =
                    BinaryStructConverter.ToByteArray<BITMAPFILEHEADER>(fileHeader);

                MemoryStream msBitmap = new MemoryStream();
                msBitmap.Write(fileHeaderBytes, 0, fileHeaderSize);
                msBitmap.Write(dibBuffer, 0, dibBuffer.Length);
                msBitmap.Seek(0, SeekOrigin.Begin);

                return BitmapFrame.Create(msBitmap);
            }

            return null;
        }

#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter. Justification: Field name need to match Win32 API names.

        /// <remarks>
        /// Adapted from https://thomaslevesque.com/2009/02/05/wpf-paste-an-image-from-the-clipboard/.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct BITMAPFILEHEADER
        {
            public static readonly short BM = 0x4d42; // BM

            public short bfType;
            public int bfSize;
            public short bfReserved1;
            public short bfReserved2;
            public int bfOffBits;
        }

        /// <remarks>
        /// Adapted from https://thomaslevesque.com/2009/02/05/wpf-paste-an-image-from-the-clipboard/.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }
#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter

        /// <remarks>
        /// Adapted from https://thomaslevesque.com/2009/02/05/wpf-paste-an-image-from-the-clipboard/.
        /// </remarks>
        private static class BinaryStructConverter
        {
            public static T FromByteArray<T>(byte[] bytes)
                where T : struct
            {
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    int size = Marshal.SizeOf(typeof(T));
                    ptr = Marshal.AllocHGlobal(size);
                    Marshal.Copy(bytes, 0, ptr, size);
                    object obj = Marshal.PtrToStructure(ptr, typeof(T));
                    return (T)obj;
                }
                finally
                {
                    if (ptr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptr);
                    }
                }
            }

            public static byte[] ToByteArray<T>(T obj)
                where T : struct
            {
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    int size = Marshal.SizeOf(typeof(T));
                    ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(obj, ptr, true);
                    byte[] bytes = new byte[size];
                    Marshal.Copy(ptr, bytes, 0, size);
                    return bytes;
                }
                finally
                {
                    if (ptr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptr);
                    }
                }
            }
        }
    }
}
