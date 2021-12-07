using System;
using System.Collections.Generic;
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

            Finish();
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
            Finish();
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

        private void EgalizareBtn_Click(object sender, EventArgs e)
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

            Finish();
        }

        private void RotateBtn_Click(object sender, EventArgs e)
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

        private void InitializeMatrix_ForFTJ(int[,] matrix, int index)
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

        private void InitializeMatrix_Outlier(int[,] matrix)
        {
            matrix[0, 0] = 1;
            matrix[0, 1] = 1;
            matrix[0, 2] = 1;
            matrix[1, 0] = 1;
            matrix[1, 1] = 0;
            matrix[1, 2] = 1;
            matrix[2, 0] = 1;
            matrix[2, 1] = 1;
            matrix[2, 2] = 1;
        }

        private void FTJBtn_Click(object sender, EventArgs e) //Filtrul Trece-Jos
        {
            workImage.Lock();
            Color color;
            byte R, G, B;
            int GSum, RSum, BSum;
            int[,] H = new int[3, 3];
            int index = Convert.ToInt32(ftjTxtBox.Text);
            InitializeMatrix_ForFTJ(H, index);

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
            Finish();
        }

        private void OutlierBtn_Click(object sender, EventArgs e)
        {
            int[,] H = new int[3, 3];
            Color color;
            byte R, G, B;
            int RSum, GSum, BSum;
            int pragIndex = Convert.ToInt32(outlierTxtBox.Text);
            InitializeMatrix_Outlier(H);

            workImage.Lock();

            for (int i = 1; i < workImage.width - 2; i++)
            {
                for (int j = 1; j < workImage.height - 2; j++)
                {
                    RSum = GSum = BSum = 0;

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(row, col);
                            R = color.R;
                            G = color.G;
                            B = color.B;

                            RSum = RSum + R * H[row - i + 1, col - j + 1];
                            GSum = GSum + G * H[row - i + 1, col - j + 1];
                            BSum = BSum + B * H[row - i + 1, col - j + 1];
                        }
                    }

                    color = workImage.GetPixel(i, j);
                    R = color.R;
                    G = color.G;
                    B = color.B;
                    RSum = RSum / 8;
                    GSum = GSum / 8;
                    BSum = BSum / 8;

                    if (Math.Abs((R + G + B) / 3 - (RSum + GSum + BSum) / 3) > pragIndex)
                    {
                        color = Color.FromArgb(RSum, GSum, BSum);
                    }

                    workImage.SetPixel(i, j, color);
                }
            }

            Finish();
        }

        private void Finish()
        {
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void MedianBtn_Click(object sender, EventArgs e)
        {
            Color color;
            workImage.Lock();

            for (int i = 1; i < workImage.width - 1; i++)
            {
                for (int j = 1; j < workImage.height - 1; j++)
                {
                    int[] R = new int[9], G = new int[9], B = new int[9];

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(row, col);
                            R[3 * (row - i + 1) + col - j + 1] = color.R;
                            G[3 * (row - i + 1) + col - j + 1] = color.G;
                            B[3 * (row - i + 1) + col - j + 1] = color.B;
                        }
                    }

                    Array.Sort(R);
                    Array.Sort(G);
                    Array.Sort(B);

                    color = Color.FromArgb((byte)R[4], (byte)G[4], (byte)B[4]);
                    workImage.SetPixel(i, j, color);
                }
            }

            Finish();
        }

        //maximul din dictionar - vector de frecventa
        private Color CBP(int x, int y, int CS, int SR, int T)
        {
            Dictionary<Color, int> Q = new Dictionary<Color, int>();

            for (int i = x - SR; i <= x + SR; i++)
            {
                for (int j = y - SR; j <= y + SR; j++)
                {
                    if (i == x && i == y) continue;
                    if (SAD(x, y, x, j, CS) < T && !SaltAndPepper(x, j)) //in afara de cel detectat ca sare si piper
                    {
                        Color color = workImage.GetPixel(x, j);
                        if (!Q.ContainsKey(color))
                        {
                            Q.Add(color, 1);
                        }
                        else
                        {
                            Q[color]++;
                        }
                    }
                }
            }

            KeyValuePair<Color, int> max = new KeyValuePair<Color, int>();
            foreach (var element in Q)
            {
                if (element.Value > max.Value)
                    max = element;
            }

            return max.Key;
        }

        // se uita in vecinatatea pixelului curent
        private int SAD(int x1, int y1, int x2, int y2, int CS)
        {
            int S = 0;
            for (int i = (CS / 2) * (-1); i <= CS / 2; i++)
            {
                for (int j = (CS / 2) * (-1); j <= CS / 2; j++)
                {
                    if (i + x1 > 0 && i + x1 < Width && i + x2 > 0 && i + x2 < Width)
                    {
                        if (j + y1 > 0 && j + y1 < Height && j + y2 > 0 && j + y2 < Height)
                        {
                            if (!(i == 0 && j == 0))
                            {
                                Color color1 = workImage.GetPixel(i + x1, j + y1);
                                Color color2 = workImage.GetPixel(i + x2, j + y2);
                                int greyScale1 = (color1.R + color1.G + color1.B) / 3;
                                int greyScale2 = (color1.R + color1.G + color1.B) / 3;
                                S = S + Math.Abs(greyScale1 - greyScale2);
                            }
                        }
                    }
                }
            }
            return S;
        }

        private bool SaltAndPepper(int i, int j)
        {
            Color color;
            color = workImage.GetPixel(i, j);
            int grayScale = (color.R + color.G + color.B) / 3;
            return (grayScale == 0) || (grayScale == 255);
        }

        private void MarkovBtn_Click(object sender, EventArgs e)
        {
            //T = 500
            //SR 4
            //CS 3
            workImage.Lock();
            for (int i = 0; i < workImage.width; i++)
            {
                for (int j = 0; j < workImage.height; j++)
                {
                    if (SaltAndPepper(i, j))
                    {
                        workImage.SetPixel(i, j, CBP(i, j, 3, 4, 500));
                    }
                }
            }
            workImage.Unlock();
        }

        private void UnsharpBtn_Click(object sender, EventArgs e)
        {
            workImage.Lock();
            int[,] H = new int[3, 3];
            int index = Convert.ToInt32(unsharpMaskingText.Text);
            Color color;

            byte R, G, B;
            R = G = B = 0;
            double c = 0.6; //c[0.6,0.8]

            int RSum, GSum, BSum;
            H[0, 0] = 1;
            H[0, 1] = index;
            H[0, 2] = 1;
            H[1, 0] = index;
            H[1, 1] = index * index;
            H[1, 2] = index;
            H[2, 0] = 1;
            H[2, 1] = index;
            H[2, 2] = 1;

            for (int i = 1; i < workImage.height - 1; i++)
            {
                for (int j = 1; j < workImage.width - 1; j++)
                {
                    RSum = GSum = BSum = 0;

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(col, row);
                            R = color.R;
                            G = color.G;
                            B = color.B;

                            RSum += R * H[row - i + 1, col - j + 1];
                            GSum += G * H[row - i + 1, col - j + 1];
                            BSum += B * H[row - i + 1, col - j + 1];
                        }
                    }

                    RSum /= ((index + 2) * (index + 2));
                    GSum /= ((index + 2) * (index + 2));
                    BSum /= ((index + 2) * (index + 2));

                    RSum = Convert.ToInt32((int)(c * R / ((2 * c) - 1)) - (((1 - c) * RSum) / ((2 * c) - 1)));
                    GSum = Convert.ToInt32((int)(c * G / ((2 * c) - 1)) - (((1 - c) * GSum) / ((2 * c) - 1)));
                    BSum = Convert.ToInt32((int)(c * B / ((2 * c) - 1)) - (((1 - c) * BSum) / ((2 * c) - 1)));

                    if (RSum > 255) RSum = 255;
                    if (GSum > 255) GSum = 255;
                    if (BSum > 255) BSum = 255;

                    if (RSum < 0) RSum = 0;
                    if (GSum < 0) GSum = 0;
                    if (BSum < 0) BSum = 0;

                    color = Color.FromArgb(RSum, GSum, BSum);
                    workImage.SetPixel(j, i, color);
                }
            }
            Finish();
        }

        private void FTSBtn_Click(object sender, EventArgs e)
        {
            Bitmap copiedImage = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(copiedImage);
            this.workImage.Lock();
            workImage.Lock();

            int[,] H = new int[3, 3];
            Color color;

            byte R, G, B;

            int RSum, GSum, BSum;
            ////Matricea 1
            //H[0, 0] = 0;
            //H[0, 1] = -1;
            //H[0, 2] = 0;
            //H[1, 0] = -1;
            //H[1, 1] = 5;
            //H[1, 2] = -1;
            //H[2, 0] = 0;
            //H[2, 1] = -1;
            //H[2, 2] = 0;

            //Matricea 2
            H[0, 0] = -1;
            H[0, 1] = -1;
            H[0, 2] = -1;
            H[1, 0] = -1;
            H[1, 1] = 9;
            H[1, 2] = -1;
            H[2, 0] = -1;
            H[2, 1] = -1;
            H[2, 2] = -1;

            ////Matricea 3
            //H[0, 0] = 1;
            //H[0, 1] = -2;
            //H[0, 2] = 1;
            //H[1, 0] = -2;
            //H[1, 1] = 5;
            //H[1, 2] = -2;
            //H[2, 0] = 1;
            //H[2, 1] = -2;
            //H[2, 2] = 1;

            for (int i = 1; i < workImage.height - 1; i++)
            {
                for (int j = 1; j < workImage.width - 1; j++)
                {
                    RSum = GSum = BSum = 0;

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = this.workImage.GetPixel(col, row);
                            R = color.R;
                            G = color.G;
                            B = color.B;

                            RSum += R * H[row - i + 1, col - j + 1];
                            GSum += G * H[row - i + 1, col - j + 1];
                            BSum += B * H[row - i + 1, col - j + 1];
                        }
                    }

                    if (RSum > 255) RSum = 255;
                    if (GSum > 255) GSum = 255;
                    if (BSum > 255) BSum = 255;

                    if (RSum < 0) RSum = 0;
                    if (GSum < 0) GSum = 0;
                    if (BSum < 0) BSum = 0;

                    color = Color.FromArgb(RSum, GSum, BSum);
                    workImage.SetPixel(j, i, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
            this.workImage.Unlock();
        }

        private void SortFilterBtn_Click(object sender, EventArgs e)
        {
            workImage.Lock();
            Color color;

            for (int i = 1; i < workImage.height - 1; i++)
            {
                for (int j = 1; j < workImage.width - 1; j++)
                {
                    List<int> red = new List<int>();
                    List<int> blue = new List<int>();
                    List<int> green = new List<int>();

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(row, col);
                            red.Add(color.R);
                            green.Add(color.G);
                            blue.Add(color.B);
                        }
                    }
                    red.Sort();
                    blue.Sort();
                    green.Sort();
                    workImage.SetPixel(i, j, Color.FromArgb(red[4], green[4], blue[4])); //setare pixel cu elementul central
                }
            }
            Finish();
        }

        //detectie contururi orizontale
        private void KirschBtn_Click(object sender, EventArgs e)
        {
            Color color;
            Bitmap image = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(image);
            workImage.Lock();
            Bitmap newImage = new Bitmap(workImage.width, workImage.height);
            FastImage newFastImage = new FastImage(newImage);
            newFastImage.Lock();
            List<int[,]> HList = new List<int[,]>();

            HList.Add(new int[,] {
                { -1, 0, -1 },
                { -1, 0, -1 },
                { -1, 0, -1 }
            });
            HList.Add(new int[,] {
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            });
            HList.Add(new int[,] {
                { 0, 1, 1 },
                { -1, 0, 1 },
                { -1, -1, 0 }
            });
            HList.Add(new int[,] {
                { 1, 1, 0 },
                { 1, 0, -1 },
                { 0, -1, -1 }
            });

            for (int i = 1; i < workImage.width - 1; i++)
            {
                for (int j = 1; j < workImage.height - 1; j++)
                {
                    int GMax = 0;
                    int BMax = 0;
                    int RMax = 0;
                    foreach (var H in HList)
                    {
                        int GSum = 0;
                        int BSum = 0;
                        int RSum = 0;
                        for (int row = i - 1; row <= i + 1; row++)
                        {
                            for (int col = j - 1; col <= j + 1; col++)
                            {
                                color = workImage.GetPixel(row, col);
                                RSum += color.R * H[row - i + 1, col - j + 1];
                                GSum += color.G * H[row - i + 1, col - j + 1];
                                BSum += color.B * H[row - i + 1, col - j + 1];
                            }
                        }
                        RMax = RSum > RMax ? RSum : RMax;
                        GMax = GSum > GMax ? GSum : GMax;
                        BMax = BSum > BMax ? BSum : BMax;
                    }

                    if (RMax > 255) RMax = 255;
                    if (RMax < 0) RMax = 0;
                    if (GMax > 255) GMax = 255;
                    if (GMax < 0) GMax = 0;
                    if (BMax > 255) BMax = 255;
                    if (BMax < 0) BMax = 0;

                    newFastImage.SetPixel(i, j, Color.FromArgb(RMax, GMax, BMax));
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = newFastImage.GetBitMap();
            newFastImage.Unlock();
            workImage.Unlock();
        }

        private void PrewittBtn_Click(object sender, EventArgs e)
        {
            Color color;
            Bitmap image = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(image);
            workImage.Lock();
            Bitmap newImage = new Bitmap(workImage.width, workImage.height);
            FastImage newFastImage = new FastImage(newImage);
            newFastImage.Lock();

            int[,] PMatrix = new int[,]
            {
                {-1, -1, -1},
                {0, 0, 0},
                {1, 1, 1}
            };

            int[,] QMatrix = new int[,]
            {
                {-1, 0, 1},
                {-1, 0, 1},
                {-1, 0, 1}
            };

            for (int i = 1; i < workImage.width - 1; i++)
            {
                for (int j = 1; j < workImage.height - 1; j++)
                {
                    int PRSum = 0;
                    int PGSum = 0;
                    int PBSum = 0;
                    int QRSum = 0;
                    int QGSum = 0;
                    int QBSum = 0;

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {
                            color = workImage.GetPixel(row, col);
                            PRSum += color.R * PMatrix[row - i + 1, col - j + 1];
                            PGSum += color.G * PMatrix[row - i + 1, col - j + 1];
                            PBSum += color.B * PMatrix[row - i + 1, col - j + 1];

                            QRSum += color.R * QMatrix[row - i + 1, col - j + 1];
                            QGSum += color.G * QMatrix[row - i + 1, col - j + 1];
                            QBSum += color.B * QMatrix[row - i + 1, col - j + 1];
                        }
                    }

                    int R = (int)Math.Sqrt(Math.Pow(PRSum, 2) + Math.Pow(QRSum, 2));
                    int G = (int)Math.Sqrt(Math.Pow(PGSum, 2) + Math.Pow(QGSum, 2));
                    int B = (int)Math.Sqrt(Math.Pow(PBSum, 2) + Math.Pow(QBSum, 2));

                    if (R > 255) R = 255;
                    if (R < 0) R = 0;
                    if (G > 255) G = 255;
                    if (G < 0) G = 0;
                    if (B > 255) B = 255;
                    if (B < 0) B = 0;

                    newFastImage.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = newFastImage.GetBitMap();
            newFastImage.Unlock();
            workImage.Unlock();
        }

        private void FreiChenBtn_Click(object sender, EventArgs e)
        {
            Color color;
            Bitmap image = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(image);
            workImage.Lock();
            Bitmap newImage = new Bitmap(workImage.width, workImage.height);
            FastImage newFastImage = new FastImage(newImage);
            newFastImage.Lock();
            List<double[,]> FMatrics = new List<double[,]>
            {
                new double[,] {
                { 1, Math.Sqrt(2), 1 },
                { 0, 0, 0 },
                { -1, -Math.Sqrt(2), -1 }
            },
                new double[,] {
                { 1, 0, -1 },
                { Math.Sqrt(2), 0, -Math.Sqrt(2) },
                { 1, 0, -1 }
            },
                new double[,] {
                { 0, -1, Math.Sqrt(2) },
                { 1, 0, -1 },
                { -Math.Sqrt(2), 1, 0 }
            },
                new double[,] {
                { Math.Sqrt(2), -1, 0 },
                { -1, 0, 1 },
                { 0, 1, -Math.Sqrt(2) }
            },
                new double[,] {
                { 0, 1, 0 },
                { -1, 0, -1 },
                { 0, 1, 0 }
            },
                new double[,] {
                { -1, 0, 1 },
                { 0, 0, 0 },
                { 1, 0, -1 }
            },
                new double[,] {
                { 1, -2, 1 },
                { -2, 4, -2 },
                { 1, -2, 1 }
            },
                new double[,] {
                { -2, 1, -2 },
                { 1, 4, 1 },
                { -2, 1, -2 }
            },
                new double[,] {
                { 1/9.0, 1/9.0, 1/9.0 },
                { 1/9.0, 1/9.0, 1/9.0 },
                { 1/9.0, 1/9.0, 1/9.0 }
            }
          };

            for (int i = 1; i < workImage.width - 1; i++)
            {
                for (int j = 1; j < workImage.height - 1; j++)
                {
                    double R4sum = 0;
                    double G4sum = 0;
                    double B4sum = 0;

                    double R9Sum = 0;
                    double G9Sum = 0;
                    double B9Sum = 0;

                    //numitor
                    for (int index = 0; index <= 8; index++)
                    {
                        var F = FMatrics[index];

                        double FRSum = 0;
                        double FGsum = 0;
                        double FBSum = 0;
                        for (int row = i - 1; row <= i + 1; row++)
                        {
                            for (int col = j - 1; col <= j + 1; col++)
                            {
                                color = workImage.GetPixel(row, col);
                                FRSum += color.R * F[row - i + 1, col - j + 1];
                                FGsum += color.G * F[row - i + 1, col - j + 1];
                                FBSum += color.B * F[row - i + 1, col - j + 1];
                            }
                        }
                        R9Sum += Math.Pow(FRSum, 2);
                        G9Sum += Math.Pow(FGsum, 2);
                        B9Sum += Math.Pow(FBSum, 2);
                    }

                    //numarator
                    for (int index = 0; index <= 3; index++)
                    {
                        var F = FMatrics[index];
                        double sumFR = 0;
                        double sumFG = 0;
                        double sumFB = 0;
                        for (int row = i - 1; row <= i + 1; row++)
                        {
                            for (int col = j - 1; col <= j + 1; col++)
                            {
                                color = workImage.GetPixel(row, col);
                                sumFR += color.R * F[row - i + 1, col - j + 1];
                                sumFG += color.G * F[row - i + 1, col - j + 1];
                                sumFB += color.B * F[row - i + 1, col - j + 1];
                            }
                        }
                        R4sum += Math.Pow(sumFR, 2);
                        G4sum += Math.Pow(sumFG, 2);
                        B4sum += Math.Pow(sumFB, 2);
                    }

                    //Final *255
                    int R = (int)(Math.Sqrt(R4sum / R9Sum) * 255);
                    int G = (int)(Math.Sqrt(G4sum / G9Sum) * 255);
                    int B = (int)(Math.Sqrt(G4sum / G9Sum) * 255);

                    panelDestination.BackgroundImage = null;
                    panelDestination.BackgroundImage = newFastImage.GetBitMap();
                    newFastImage.Unlock();
                    workImage.Unlock();
                }
            }
        }

        private void GaborBtn_Click(object sender, EventArgs e)
        {
            Color color;
            Bitmap image = new Bitmap(sourceFileName);
            FastImage workImage = new FastImage(image);
            workImage.Lock();
            Bitmap newImage = new Bitmap(workImage.width, workImage.height);
            FastImage newFastImage = new FastImage(newImage);
            newFastImage.Lock();
            int[,] P = new int[,] { { 1, 1, 1 }, { 0, 0, 0 }, { -1, -1, -1 } };
            int[,] Q = new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
            Color[,] vecini;
            double RSum, GSum, BSum, RScale, GScale, BScale, Ru, Gu, Bu;

            for (int i = 1; i < workImage.width - 1; i++)
            {
                for (int j = 1; j < workImage.height - 1; j++)
                {
                    vecini = new Color[,]
                   {
                        {workImage.GetPixel(i - 1, j - 1), workImage.GetPixel(i - 1, j), workImage.GetPixel(i - 1, j + 1)},
                        {workImage.GetPixel(i, j - 1), workImage.GetPixel(i, j), workImage.GetPixel(i, j + 1)},
                        {workImage.GetPixel(i + 1, j - 1), workImage.GetPixel(i + 1, j),workImage.GetPixel(i + 1, j + 1)}
                   };

                    int[] sumaP = new int[] { 0, 0, 0 };
                    int[] sumaQ = new int[] { 0, 0, 0 };
                    RSum = GSum = BSum = 0;

                    for (int q = 0; q < 3; q++)
                    {
                        for (int w = 0; w < 3; w++)
                        {
                            sumaP[0] += vecini[q, w].R * P[q, w];
                            sumaP[1] += vecini[q, w].G * P[q, w];
                            sumaP[2] += vecini[q, w].B * P[q, w];

                            sumaQ[0] += vecini[q, w].R * Q[q, w];
                            sumaQ[1] += vecini[q, w].G * Q[q, w];
                            sumaQ[2] += vecini[q, w].B * Q[q, w];
                        }
                    }
                    //sumaQ pentru R
                    if (sumaQ[0] == 0)
                    {
                        if (sumaP[0] >= 0) Ru = Math.PI / 2;
                        else Ru = 0 - (Math.PI / 2);
                    }
                    else
                    {
                        Ru = Math.Atan(sumaP[0] / sumaQ[0]);
                        if (sumaQ[0] < 0) Ru += Math.PI;
                    }

                    //sumaQ pentru G
                    if (sumaQ[1] == 0)
                    {
                        if (sumaP[1] >= 0) Gu = Math.PI / 2;
                        else Gu = 0 - (Math.PI / 2);
                    }
                    else
                    {
                        Gu = Math.Atan(sumaP[1] / sumaQ[1]);
                        if (sumaQ[1] < 0) Gu += Math.PI;
                    }

                    //sumaQ pentru B
                    if (sumaQ[2] == 0)
                    {
                        if (sumaP[2] >= 0) Bu = Math.PI / 2;
                        else Bu = 0 - (Math.PI / 2);
                    }
                    else
                    {
                        Bu = Math.Atan(sumaP[2] / sumaQ[2]);
                        if (sumaQ[2] < 0) Bu += Math.PI;
                    }

                    //u = u + PI/2
                    Ru += Math.PI / 2;
                    Gu += Math.PI / 2;
                    Bu += Math.PI / 2;

                    for (int q = 0; q < 3; q++)
                    {
                        for (int w = 0; w < 3; w++)
                        {
                            RScale = (Math.Pow(Math.E, 0 - ((Math.Pow(q, 2) + Math.Pow(w, 2)) / (2 * Math.Pow(0.66, 2))))) *
                                     Math.Sin(1.5 * (q * Math.Cos(Ru) + w * Math.Sin(Ru)));
                            GScale = (Math.Pow(Math.E, 0 - ((Math.Pow(q, 2) + Math.Pow(w, 2)) / (2 * Math.Pow(0.66, 2))))) *
                                     Math.Sin(1.5 * (q * Math.Cos(Gu) + w * Math.Sin(Gu)));
                            BScale = (Math.Pow(Math.E, 0 - ((Math.Pow(q, 2) + Math.Pow(w, 2)) / (2 * Math.Pow(0.66, 2))))) *
                                     Math.Sin(1.5 * (q * Math.Cos(Bu) + w * Math.Sin(Bu)));

                            RSum += RScale * vecini[q, w].R;
                            GSum += GScale * vecini[q, w].G;
                            BSum += BScale * vecini[q, w].B;
                        }
                    }

                    if (RSum > 255) RSum = 255;
                    if (GSum > 255) GSum = 255;
                    if (BSum > 255) BSum = 255;

                    if (RSum < 0) RSum = 0;
                    if (GSum < 0) GSum = 0;
                    if (BSum < 0) BSum = 0;

                    color = Color.FromArgb(Convert.ToInt32(RSum), Convert.ToInt32(GSum), Convert.ToInt32(BSum));

                    newFastImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = newFastImage.GetBitMap();
            newFastImage.Unlock();
            workImage.Unlock();
        }
    }
}