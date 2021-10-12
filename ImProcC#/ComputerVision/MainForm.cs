using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerVision
{
    public partial class MainForm : Form
    {
        private string sourceFileName = "";
        private FastImage workImage;
        private Bitmap image = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(); //permite selectarea fisierului pe care dorim sa il incarcam
            sourceFileName = openFileDialog.FileName;
            panelSource.BackgroundImage = new Bitmap(sourceFileName);
            image = new Bitmap(sourceFileName);
            workImage = new FastImage(image);
        }

        private void BtnGrayscale_Click(object sender, EventArgs e)
        {
            Color color;

            workImage.Lock();
            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    byte R = color.R;
                    byte G = color.G;
                    byte B = color.B;

                    byte average = (byte)((R + G + B) / 3);

                    color = Color.FromArgb(average, average, average);

                    workImage.SetPixel(i, j, color);
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void BtnNegativare_Click(object sender, EventArgs e)
        {
            Color color;

            workImage.Lock();
            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    byte R = (byte)(255 - color.R);
                    byte G = (byte)(255 - color.G);
                    byte B = (byte)(255 - color.B);

                    color = Color.FromArgb(R, G, B);

                    workImage.SetPixel(i, j, color);
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private int SetLuminosityBasedOnTrackBarValue(int color, int trackBarValue)
        {
            if (color + trackBarValue > 255)
            {
                color = 255;
            }
            else if (color + trackBarValue < 0)
            {
                color = 0;
            }
            else
            {
                color += trackBarValue;
            }
            return color;
        }

        private int CheckNormalizationInt(int value)
        {
            if (value > 255)
            {
                value = 255;
            }
            else if (value < 0)
            {
                value = 0;
            }
            return value;
        }

        private void TrackBarNegativare_Scroll(object sender, EventArgs e)
        {
            Color color;
            Bitmap copiedImage = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(copiedImage);
            this.workImage.Lock();
            workImage.Lock();
            var trackBarValue = trackBarNegativare.Value;
            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = this.workImage.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;

                    color = Color.FromArgb(SetLuminosityBasedOnTrackBarValue(R, trackBarValue), SetLuminosityBasedOnTrackBarValue(G, trackBarValue), SetLuminosityBasedOnTrackBarValue(B, trackBarValue));
                    workImage.SetPixel(i, j, color);
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
            this.workImage.Unlock();
        }

        private void TrackBarContrast_Scroll(object sender, EventArgs e)
        {
            int minR, minG, minB, maxR, maxG, maxB, aG, bG, aR, bR, aB, bB, finalValueOfR, finalValueOfG, finalValueOfB;
            minR = minG = minB = 255;
            maxR = maxG = maxB = 0;
            int delta = trackBarContrast.Value;    //a = min-delta, b=max+delta
            byte R, G, B;
            Color color;
            Bitmap copiedImage = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(copiedImage);
            this.workImage.Lock();
            workImage.Lock();

            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    R = color.R;
                    G = color.G;
                    B = color.B;

                    if (R < minR) minR = R;
                    if (G < minG) minG = G;
                    if (B < minB) minB = B;

                    if (R > maxR) maxR = R;
                    if (G > maxG) maxG = G;
                    if (B > maxB) maxB = B;
                }
            }

            aR = minR - delta;
            aG = minG - delta;
            aB = minB - delta;

            bR = maxR + delta;
            bG = maxG + delta;
            bB = maxB + delta;

            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    R = color.R;
                    G = color.G;
                    B = color.B;
                    finalValueOfR = ((bR - aR) * (R - minR) / (maxR - minR)) + aR;
                    finalValueOfG = ((bG - aG) * (G - minG) / (maxG - minG)) + aG;
                    finalValueOfB = ((bB - aB) * (B - minB) / (maxB - minB)) + aB;

                    finalValueOfR = CheckNormalizationInt(finalValueOfR);
                    finalValueOfG = CheckNormalizationInt(finalValueOfG);
                    finalValueOfB = CheckNormalizationInt(finalValueOfB);

                    color = Color.FromArgb((byte)finalValueOfR, (byte)finalValueOfG, (byte)finalValueOfB);
                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
            this.workImage.Unlock();
        }
    }
}