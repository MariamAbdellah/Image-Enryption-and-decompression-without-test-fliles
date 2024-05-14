using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{
    public class Encryption
    {
        static string CreatePassword(string initialSeed, int tapPosition, out string updatedSeed)
        {
            string k = "";

            for (int i = 0; i < 8; i++)
            {
                char initPosition = initialSeed[0];

                // Convert initPosition and tapPosition to integer values
                int initPositionInt = int.Parse(initPosition.ToString());
                int tapPositionInt = int.Parse(initialSeed[initialSeed.Length - tapPosition - 1].ToString());

                // Perform XOR on the integer values
                int returnedKey = initPositionInt ^ tapPositionInt;

                // Remove the first character from initialSeed
                initialSeed = initialSeed.Substring(1);

                initialSeed += returnedKey.ToString();
                k += returnedKey;
            }
            updatedSeed = initialSeed;
            return k;
        }

        public static void encryption(RGBPixel[,] ImageMatrix, string initialSeed, int tapPosition)
        {
            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);

            string updatedSeed1, updatedSeed2, updatedSeed3 = "";
            string redPassword, greenPassword, bluePassword;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    redPassword = CreatePassword(initialSeed, tapPosition, out updatedSeed1);
                    greenPassword = CreatePassword(updatedSeed1, tapPosition, out updatedSeed2);
                    bluePassword = CreatePassword(updatedSeed2, tapPosition, out updatedSeed3);

                    // XOR the red, green, and blue values of each pixel with the corresponding password
                    ImageMatrix[i, j].red ^= Convert.ToByte(redPassword, 2);
                    ImageMatrix[i, j].green ^= Convert.ToByte(greenPassword, 2);
                    ImageMatrix[i, j].blue ^= Convert.ToByte(bluePassword, 2);

                    initialSeed = updatedSeed3;
                }

            }

            // Create a new Bitmap from the modified ImageMatrix
            Bitmap encryptedImage = new Bitmap(Width, Height);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    encryptedImage.SetPixel(j, i, Color.FromArgb(ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue));
                }
            }

            string outputPath = @"D:\algorithms_bonus_1\Algo project\final_Algo_project\encryptedImg\encrypted_image.bmp";
            encryptedImage.Save(outputPath);
        }

        public static void decryption(RGBPixel[,] ImageMatrix, string initialSeed, int tapPosition)
        {
            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);

            string updatedSeed1, updatedSeed2, updatedSeed3 = "";
            string redPassword, greenPassword, bluePassword;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    redPassword = CreatePassword(initialSeed, tapPosition, out updatedSeed1);
                    greenPassword = CreatePassword(updatedSeed1, tapPosition, out updatedSeed2);
                    bluePassword = CreatePassword(updatedSeed2, tapPosition, out updatedSeed3);

                    // XOR the red, green, and blue values of each pixel with the corresponding password
                    ImageMatrix[i, j].red ^= Convert.ToByte(redPassword, 2);
                    ImageMatrix[i, j].green ^= Convert.ToByte(greenPassword, 2);
                    ImageMatrix[i, j].blue ^= Convert.ToByte(bluePassword, 2);
                    initialSeed = updatedSeed3;

                }

            }
        }

    }
}
