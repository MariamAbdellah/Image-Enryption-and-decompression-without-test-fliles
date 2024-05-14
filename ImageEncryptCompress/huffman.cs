using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace ImageEncryptCompress
{

    [Serializable] // Add this attribute to make the Node class serializable
    public class Node : IComparable<Node>
    {
        public byte pixelVal { get; set; }
        public int pixelFreq { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }

        public Node()
        {
            pixelFreq = 0;
            left = right = null;
        }

        public int CompareTo(Node other)
        {
            return pixelFreq.CompareTo(other.pixelFreq);
        }
    }

    public class Huffman
    {
        public Dictionary<byte, int>[] CalculateFrequencies(RGBPixel[,] image)
        {
            Dictionary<byte, int>[] frequencies = new Dictionary<byte, int>[3];
            for (int i = 0; i < 3; i++)
                frequencies[i] = new Dictionary<byte, int>();

            int width = image.GetLength(0);
            int height = image.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    RGBPixel pixel = image[i, j];

                    if (!frequencies[0].ContainsKey(pixel.red))
                        frequencies[0][pixel.red] = 1;
                    else
                        frequencies[0][pixel.red]++;

                    if (!frequencies[1].ContainsKey(pixel.green))
                        frequencies[1][pixel.green] = 1;
                    else
                        frequencies[1][pixel.green]++;

                    if (!frequencies[2].ContainsKey(pixel.blue))
                        frequencies[2][pixel.blue] = 1;
                    else
                        frequencies[2][pixel.blue]++;
                }
            }

            return frequencies;
        }

        public Dictionary<byte, string>[] encodingTable;
        public Dictionary<string, byte>[] decodingTable;

        public void CompressImage(RGBPixel[,] image, string outputPath, int width, int height)
        {
            // Step 1: Calculate pixel frequencies
            Dictionary<byte, int>[] frequencies = CalculateFrequencies(image);

            // Step 2: Build Huffman trees and generate encoding tables
            encodingTable = new Dictionary<byte, string>[3];
            //decodingTable = new Dictionary<string, byte>[3];
            Node[] roots = new Node[3];

            for (int i = 0; i < 3; i++)
            {
                encodingTable[i] = new Dictionary<byte, string>();
                //decodingTable[i] = new Dictionary<string, byte>();

                roots[i] = BuildTree(frequencies[i]);
                GenerateEncodingTable(roots[i], "", encodingTable[i]);
                //GenerateDecodingTable(roots[i], "", decodingTable[i]);
            }

            if(File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

            using (BinaryWriter bw = new BinaryWriter(fileStream))
            {
                //writing trees
                //List<Node> redNodes = new List<Node>();
                //TraverseTree(roots[0], redNodes);
                //int redCount = redNodes.Count;
                //bw.Write(redCount);
                //foreach (Node node in redNodes)
                //{
                //    bw.Write(node.pixelVal);
                //    bw.Write(node.pixelFreq);
                //}

                //List<Node> greenNodes = new List<Node>();
                //TraverseTree(roots[1], greenNodes);
                //int greenCount = greenNodes.Count;
                //bw.Write(greenCount);
                //foreach (Node node in greenNodes)
                //{
                //    bw.Write(node.pixelVal);
                //    bw.Write(node.pixelFreq);
                //}

                //List<Node> blueNodes = new List<Node>();
                //TraverseTree(roots[2], blueNodes);
                //int blueCount = blueNodes.Count;
                //bw.Write(blueCount);
                //foreach (Node node in blueNodes)
                //{
                //    bw.Write(node.pixelVal);
                //    bw.Write(node.pixelFreq);
                //}


                //for (int i = 0; i < 3; i++)
                //{
                //    int count = frequencies[i].Count;
                //    bw.Write(count);
                //    foreach (var lvl in frequencies[i])
                //    {
                //        bw.Write(lvl.Key);
                //        bw.Write(lvl.Value);
                //    }
                //}


                //write each tree
                for (int i = 0; i < 3; i++)
                {
                    WriteHuffmanTree(bw, roots[i]);
                }

                //writing compressed image
                //int width = ImageOperations.GetWidth(image);
                //int height = ImageOperations.GetHeight(image);

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        RGBPixel pixel = image[i, j];
                        string redCode = encodingTable[0][pixel.red];
                        string greenCode = encodingTable[1][pixel.green];
                        string blueCode = encodingTable[2][pixel.blue];

                        //Put each color channel's code separately
                        bw.Write(redCode);
                        bw.Write(greenCode);
                        bw.Write(blueCode);
                    }
                }

                

            }


            



            // Serialize Huffman trees and save them to memory stream
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    foreach (var root in roots)
            //    {
            //        SerializeHuffmanTree(root, ms);

            //        BinaryFormatter formatter = new BinaryFormatter();
            //        formatter.Serialize(ms, root);
            //    }

            //     Step 3: Compress image data
            //    List<string> compressedData = new List<string>();
            //    int width = image.GetLength(0);
            //    int height = image.GetLength(1);

            //    for (int i = 0; i < width; i++)
            //    {
            //        for (int j = 0; j < height; j++)
            //        {
            //            RGBPixel pixel = image[i, j];
            //            string redCode = encodingTable[0][pixel.red];
            //            string greenCode = encodingTable[1][pixel.green];
            //            string blueCode = encodingTable[2][pixel.blue];

            //             Put each color channel's code separately
            //            compressedData.Add(redCode);
            //            compressedData.Add(greenCode);
            //            compressedData.Add(blueCode);
            //        }
            //    }

            //     Convert compressed data to bytes
            //    List<byte> compressedBytes = new List<byte>();
            //    foreach (string code in compressedData)
            //    {
            //        byte[] bytes = ConvertBinaryStringToBytes(code);
            //        compressedBytes.AddRange(bytes);
            //    }

            //     Write Huffman trees and compressed data to file
            //    using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            //    {
            //         Write trees length followed by trees bytes
            //        byte[] treesLength = BitConverter.GetBytes((long)ms.Length);
            //        fs.Write(treesLength, 0, treesLength.Length);
            //        ms.Seek(0, SeekOrigin.Begin);
            //        ms.CopyTo(fs);

            //         Write compressed data
            //        fs.Write(compressedBytes.ToArray(), 0, compressedBytes.Count);
            //    }
            //}
        }

        /*public void SerializeHuffmanTree(Node root, MemoryStream ms)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, root);
        }*/

        // Helper function to convert binary string to bytes
        public byte[] ConvertBinaryStringToBytes(string binaryString)
        {
            int numOfBytes = binaryString.Length / 8; //round?
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                bytes[i] = Convert.ToByte(binaryString.Substring(i * 8, 8), 2);
            }
            return bytes;
        }


        public Node BuildTree(Dictionary<byte, int> frequencies)
        {
            PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>();

            foreach (var kvp in frequencies)
            {
                priorityQueue.Enqueue(new Node
                {
                    pixelVal = kvp.Key,
                    pixelFreq = kvp.Value
                });
            }

            while (priorityQueue.Count > 1)
            {
                Node rightChild = priorityQueue.Dequeue();
                Node leftChild = priorityQueue.Dequeue();

                Node parent = new Node
                {
                    pixelFreq = leftChild.pixelFreq + rightChild.pixelFreq,
                    left = leftChild,
                    right = rightChild
                };

                priorityQueue.Enqueue(parent);
            }

            return priorityQueue.Dequeue();
        }

        public void TraverseTree(Node root, List<Node> nodes)
        {
            
            if(root != null)
            {
                nodes.Add(root);
                TraverseTree(root.left, nodes);
                TraverseTree(root.right, nodes);
            }

            //return nodes;
        }

        // Method to serialize and save the Huffman tree to a file
        public void WriteHuffmanTree(BinaryWriter writer, Node node)
        {
            if (node == null)
                return;

            // Write whether it's a leaf node
            writer.Write(node.left == null && node.right == null);

            if (node.left == null && node.right == null)
            {
                // Write node value and frequency
                writer.Write(node.pixelVal);
                writer.Write(node.pixelFreq);
            }
            else
            {
                // Recursively serialize left and right subtrees
                WriteHuffmanTree(writer, node.left);
                WriteHuffmanTree(writer, node.right);
            }
        }

        //method to deserialize and read huffman tree from a file
        public Node ReadHuffmanTree(BinaryReader reader)
        {
            bool isLeaf = reader.ReadBoolean();

            if (isLeaf)
            {
                // Read node value and frequency
                byte value = reader.ReadByte();
                int frequency = reader.ReadInt32();
                return new Node { pixelVal = value, pixelFreq = frequency };
            }
            else
            {
                // Recursively deserialize left and right subtrees
                var left = ReadHuffmanTree(reader);
                var right = ReadHuffmanTree(reader);
                return new Node { left = left, right = right, pixelFreq = left.pixelFreq + right.pixelFreq };
            }
        }

        private void GenerateEncodingTable(Node node, string code, Dictionary<byte, string> table)
        {
            if (node.left == null && node.right == null)
            {
                table[node.pixelVal] = code;
            }
            else
            {
                GenerateEncodingTable(node.left, code + "0", table);
                GenerateEncodingTable(node.right, code + "1", table);
            }
        }

        private void GenerateDecodingTable(Node node, string code, Dictionary<string, byte> table)
        {
            if (node.left == null && node.right == null)
            {
                table[code] = node.pixelVal;
            }
            else
            {
                GenerateDecodingTable(node.left, code + "0", table);
                GenerateDecodingTable(node.right, code + "1", table);
            }
        }

        //---------------------------------------------------------------------------------------------------//
        // ... (existing methods)

        public RGBPixel[,] DecompressImage(string inputPath, int width, int height)
        {
            RGBPixel[,] decompressedImage = new RGBPixel[height, width];
            decodingTable = new Dictionary<string, byte>[3];
            FileStream fileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (BinaryReader br = new BinaryReader(fileStream))
            {
                //reading trees(not sure how yet)
                //Node redTree = new Node();

                //int redNodes = br.ReadInt32();

                //Dictionary<byte, int> redFrequency = new Dictionary<byte, int>();

                //List<Node> redList = new List<Node>();
                //List<Node> greenList = new List<Node>();
                //List<Node> blueList = new List<Node>();

                //byte val;
                //int freq;
                //for (int i = 0; i < redNodes; i++)
                //{
                //    val = br.ReadByte();
                //    freq = br.ReadInt32();
                //    Node node = new Node { pixelFreq = freq, pixelVal = val };
                //    redList.Add(node);  //what is the val of non leafe nodes?
                //}
                //Node redTree = BuildTree(redFrequency);

                //decodingTable[0] = new Dictionary<string, byte>();
                //GenerateDecodingTable(redTree, "", decodingTable[0]);

                //int greenNodes = br.ReadInt32();
                //Dictionary<byte, int> greenFrequency = new Dictionary<byte, int>();
                //for (int i = 0; i < greenNodes; i++)
                //{
                //    val = br.ReadByte();
                //    freq = br.ReadInt32();
                //    Node node = new Node { pixelFreq = freq, pixelVal = val };
                //    greenList.Add(node);
                //}
                //Node greenTree = BuildTree(greenFrequency);
                //decodingTable[1] = new Dictionary<string, byte>();
                //GenerateDecodingTable(greenTree, "", decodingTable[1]);

                //int blueNodes = br.ReadInt32();
                //Dictionary<byte, int> blueFrequency = new Dictionary<byte, int>();
                //for (int i = 0; i < blueNodes; i++)
                //{
                //    val = br.ReadByte();
                //    freq = br.ReadInt32();
                //    blueFrequency[val] = freq;
                //}
                //Node blueTree = BuildTree(blueFrequency);
                //decodingTable[2] = new Dictionary<string, byte>();
                //GenerateDecodingTable(blueTree, "", decodingTable[2]);


                //read each tree
                Node redTree = ReadHuffmanTree(br);
                Node greenTree = ReadHuffmanTree(br);
                Node blueTree = ReadHuffmanTree(br);

                decodingTable[0] = new Dictionary<string, byte>();
                decodingTable[1] = new Dictionary<string, byte>();
                decodingTable[2] = new Dictionary<string, byte>();

                GenerateDecodingTable(redTree, "", decodingTable[0]);
                GenerateDecodingTable(greenTree, "", decodingTable[1]);
                GenerateDecodingTable(blueTree, "", decodingTable[2]);




                for (int i = 0; i < height; i++)
                {
                    for(int j = 0; j < width; j++)
                    {
                        string red = br.ReadString();
                        string green = br.ReadString();
                        string blue = br.ReadString();

                        decompressedImage[i, j].red = decodingTable[0][red];
                        decompressedImage[i, j].green = decodingTable[1][green];
                        decompressedImage[i, j].blue = decodingTable[2][blue];
                    }
                }

            }

            return decompressedImage;
            // Read trees length and trees bytes
            //byte[] treesLengthBytes = new byte[sizeof(long)];
            //using (FileStream fs = new FileStream(inputPath, FileMode.Open))
            //{
            //    //fs.Read(treesLengthBytes, 0, treesLengthBytes.Length);
            //    //long treesLength = BitConverter.ToInt64(treesLengthBytes, 0);


            //    //long startPos = fs.Position;
            //    //fs.Seek(startPos, SeekOrigin.Current);
            //    //List<byte> compressedData = new List<byte>();
            //    //int bytesRead;
            //    //byte[] buffer = new byte[fs.Length - startPos];
            //    //while((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            //    //{
            //    //    compressedData.AddRange(buffer.Take(bytesRead));
            //    //}
            //    //foreach(byte b in compressedData)
            //    //{
                    
            //    //}


            //    // Deserialize Huffman trees
            //    Node[] trees = new Node[3];
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        fs.CopyTo(ms);
            //        ms.Seek(0, SeekOrigin.Begin);
            //        for (int i = 0; i < 3; i++)
            //        {
            //            /*trees[i] = DeserializeHuffmanTree(ms);*/
            //            BinaryFormatter formatter = new BinaryFormatter();
            //            trees[i] = (Node)formatter.Deserialize(ms);
            //        }

            //        // Decompress image data
            //        StringBuilder binaryString = new StringBuilder();
            //        while (ms.Position < ms.Length)
            //        {
            //            byte b = (byte)ms.ReadByte();
            //            binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            //        }

            //        RGBPixel[,] decompressedImage = new RGBPixel[width, height];
            //        int index = 0;
            //        for (int i = 0; i < width; i++)
            //        {
            //            for (int j = 0; j < height; j++)
            //            {
            //                string redCode = GetNextCode(binaryString, ref index, decodingTable[0]);
            //                string greenCode = GetNextCode(binaryString, ref index, decodingTable[1]);
            //                string blueCode = GetNextCode(binaryString, ref index, decodingTable[2]);
            //                decompressedImage[i, j].red = decodingTable[0][redCode];
            //                decompressedImage[i, j].green = decodingTable[1][greenCode];
            //                decompressedImage[i, j].blue = decodingTable[2][blueCode];
            //            }
            //        }

            //        return decompressedImage;
            //    }
            //}
        }


        public void SaveImage(RGBPixel[,] imageArray, string outputPath)
        {
            // Determine the width and height from the image array
            int width = imageArray.GetLength(0);
            int height = imageArray.GetLength(1);

            // Create a Bitmap from the RGBPixel array
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // Assuming RGBPixel has properties red, green, and blue
                        Color color = Color.FromArgb(imageArray[i, j].red, imageArray[i, j].green, imageArray[i, j].blue);
                        bitmap.SetPixel(i, j, color);
                    }
                }

                // Save the Bitmap to the specified output path
                bitmap.Save(outputPath);
            }
        }


        // Helper method to get the next Huffman code from the binary string
        private string GetNextCode(StringBuilder binaryString, ref int index, Dictionary<string, byte> decodingTable)
        {
            StringBuilder code = new StringBuilder();
            while (index < binaryString.Length)
            {
                string key1 =  code.ToString() + binaryString[index].ToString();
                if (decodingTable.ContainsKey(key1))
                {
                    code.Append(binaryString[index]);
                    index++; // Move to the next bit
                    break;
                }
                //index++;
            }
            return code.ToString();
        }

        // Method to deserialize and load the Huffman tree from a file
        /*public Node DeserializeHuffmanTree(string filePath)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (Node)formatter.Deserialize(stream);
            }
        }*/
        //-----------------------//
    }
}