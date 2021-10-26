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

        private void egalizareBtn_Click(object sender, EventArgs e)
        {
            byte R, G, B;
            Color color;
            int[] hist = new int[256];
            int[] histc = new int[256];
            int[] transf = new int[256];
            int intensitate;

            workImage.Lock();

            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    R = color.R;
                    G = color.G;
                    B = color.B;

                    intensitate = (R + G + B) / 3;
                    hist[intensitate] = hist[intensitate] + 1;
                }
            }

            histc[0] = hist[0];

            for (int i = 1; i < 256; i++)
            {
                histc[i] = histc[i - 1] + hist[i];
            }

            for (int i = 0; i < 256; i++)
            {
                transf[i] = (histc[i] * 255) / (workImage.width * workImage.height);
            }

            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    R = color.R;
                    G = color.G;
                    B = color.B;
                    intensitate = (R + G + B) / 3;
                    color = Color.FromArgb(transf[intensitate], transf[intensitate], transf[intensitate]);

                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void rotatieBtn_Click(object sender, EventArgs e)
        {
            double angle = (Convert.ToDouble(rotatieLbl.Text) * Math.PI) / 180;

            Color color;
            Bitmap image = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(image);
            workImage.Lock();
            Bitmap newImage = new Bitmap(workImage.width, workImage.height);
            FastImage newFastImage = new FastImage(newImage);
            newFastImage.Lock();

            int x0 = workImage.width / 2;
            int y0 = workImage.height / 2;
            int x2, y2;
            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    x2 = (int)(Math.Cos(angle) * (i - x0) - Math.Sin(angle) * (j - y0) + x0);
                    y2 = (int)(Math.Sin(angle) * (i - x0) + Math.Cos(angle) * (j - y0) + y0);
                    if ((x2 >= 0) && (x2 < newFastImage.width))
                    {
                        if ((y2 >= 0) && (y2 < newFastImage.height))
                        {
                            newFastImage.SetPixel(x2, y2, color);
                        }
                    }
                }
            }
            for (int i = 0; i < newFastImage.width; i++)
            {
                for (int j = 0; j < newFastImage.height; j++)
                {
                    x2 = (int)(Math.Cos(-angle) * (i - x0) - Math.Sin(-angle) * (j - y0) + x0);
                    y2 = (int)(Math.Sin(-angle) * (i - x0) + Math.Cos(-angle) * (j - y0) + y0);
                    if ((x2 >= 0) && (x2 < newFastImage.width))
                    {
                        if ((y2 >= 0) && (y2 < newFastImage.height))
                        {
                            color = workImage.GetPixel(x2, y2);
                            newFastImage.SetPixel(i, j, color);
                        }
                    }
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = newFastImage.GetBitMap();
            newFastImage.Unlock();
            workImage.Unlock();
        }

        private void InitializeMatrix(int[,] matrix, int index)
        {
            matrix[0, 0] = 1;
            matrix[0, 1] = index;
            matrix[0, 2] = 1;
            matrix[1, 0] = index;
            matrix[1, 1] = index * index;
            matrix[1, 2] = index;
            matrix[2, 0] = 1;
            matrix[2, 1] = index;
            matrix[2, 2] = 1;
        }

        private void ftjBtn_Click(object sender, EventArgs e) //Filtrul Trece-Jos
        {
            workImage.Lock();
            Color color;
            byte R, G, B;
            int GSum, RSum, BSum;
            int[,] H = new int[3, 3];
            int index = Convert.ToInt32(ftjTxtBox.Text);
            InitializeMatrix(H, index);

            //Convoluția imaginii cu matricea H
            for (int i = 1; i < workImage.height - 2; i++)
            {
                for (int j = 1; j < workImage.width - 2; j++)
                {
                    GSum = RSum = BSum = 0;

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(col, row);
                            G = color.G;
                            R = color.R;
                            B = color.B;

                            GSum = GSum + G * H[row - i + 1, col - j + 1];
                            RSum = RSum + R * H[row - i + 1, col - j + 1];
                            BSum = BSum + B * H[row - i + 1, col - j + 1];
                        }
                    }
                    GSum = GSum / ((index + 2) * (index + 2));
                    RSum = RSum / ((index + 2) * (index + 2));
                    BSum = BSum / ((index + 2) * (index + 2));

                    color = Color.FromArgb(RSum, GSum, BSum);
                    workImage.SetPixel(j, i, color);
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void outlierBtn_Click(object sender, EventArgs e)
        {
            workImage.Lock();
            Color color;
            byte R, G, B;
            int GSum, RSum, BSum;
            int[,] H = new int[3, 3];
            int index = Convert.ToInt32(ftjTxtBox.Text);
            InitializeMatrix(H, index);

            //Convoluția imaginii cu matricea H
            for (int i = 1; i < workImage.height - 2; i++)
            {
                for (int j = 1; j < workImage.width - 2; j++)
                {
                    GSum = RSum = BSum = 0;

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(col, row);
                            G = color.G;
                            R = color.R;
                            B = color.B;

                            GSum = GSum + G * H[row - i + 1, col - j + 1];
                            RSum = RSum + R * H[row - i + 1, col - j + 1];
                            BSum = BSum + B * H[row - i + 1, col - j + 1];
                        }
                    }
                    GSum = GSum / ((index + 2) * (index + 2));
                    RSum = RSum / ((index + 2) * (index + 2));
                    BSum = BSum / ((index + 2) * (index + 2));

                    color = Color.FromArgb(RSum, GSum, BSum);
                    workImage.SetPixel(j, i, color);
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }
    }
}