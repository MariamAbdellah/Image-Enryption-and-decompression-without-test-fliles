using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageEncryptCompress
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;


            //ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            //
            Huffman huffman = new Huffman();
            huffman.CompressImage(ImageMatrix, "compressed.bin", ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetHeight(ImageMatrix));

            //huffman.SerializeHuffmanTree(huffman.BuildTree(huffman.CalculateFrequencies(ImageMatrix)[0]), "treeRed.bin");
            //huffman.SerializeHuffmanTree(huffman.BuildTree(huffman.CalculateFrequencies(ImageMatrix)[1]), "treeGreen.bin");
            //huffman.SerializeHuffmanTree(huffman.BuildTree(huffman.CalculateFrequencies(ImageMatrix)[2]), "treeBlue.bin");

            //ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            RGBPixel[,] DecompressedImage = huffman.DecompressImage("compressed.bin", ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetHeight(ImageMatrix));
            ImageOperations.DisplayImage(DecompressedImage, pictureBox2);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}