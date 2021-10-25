using System.Drawing;
using System.Drawing.Imaging;

namespace ComputerVision
{
    public class FastImage
    {
        private Bitmap image = null;
        private Rectangle rectangle;
        private BitmapData bitmapData = null;
        private Color color;
        private Point size;
        private int currentBitmapwidth = 0;
        public int height = 0;
        public int width = 0;

        private struct PixelData
        {
            public byte red, green, blue;
        }

        public FastImage(Bitmap bitmap)
        {
            image = bitmap;
            width = image.Width;
            height = image.Height;
            size = new Point(image.Size);
            currentBitmapwidth = size.X;
        }

        public void Lock()
        {
            // Rectangle For Locking The Bitmap In Memory
            rectangle = new Rectangle(0, 0, image.Width, image.Height);
            // Get The Bitmap's Pixel Data From The Locked Bitmap
            bitmapData = image.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        }

        public void Unlock()
        {
            image.UnlockBits(bitmapData);
        }

        public Color GetPixel(int col, int row)
        {
            unsafe
            {
                PixelData* pBase = (PixelData*)bitmapData.Scan0;
                PixelData* pPixel = pBase + row * currentBitmapwidth + col;
                color = Color.FromArgb(pPixel->red, pPixel->green, pPixel->blue);
            }
            return color;
        }

        public void SetPixel(int col, int row, Color c)
        {
            unsafe
            {
                PixelData* pBase = (PixelData*)bitmapData.Scan0;
                PixelData* pPixel = pBase + row * currentBitmapwidth + col;
                pPixel->red = c.R;
                pPixel->green = c.G;
                pPixel->blue = c.B;
            }
        }

        public Bitmap GetBitMap()
        {
            return image;
        }
    }
}