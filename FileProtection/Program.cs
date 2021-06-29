using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FileProtection
{
    class Program
    {
        const int MB_MULTIPLIER= 1;
        const int CHUNK_SIZE = 1024 * MB_MULTIPLIER;
        static string EXTENTION = ".cnk";
        static List<string> ABSOLUTE_PATHS = new List<string>();
        static string REF_PATH = "/Users/thamerasiri/Desktop/Chunks";
        
        static void manageFile(string path)
        {
            //string path = @"c:\temp\MyTest.txt";
            try
            {
                if (File.Exists(path))
                {
                    // Open the stream and read it back.
                    using (StreamReader sr = File.OpenText(path))
                    {
                        long size =  sr.BaseStream.Length; //in bytes
                        int reminder = (int)(size % CHUNK_SIZE);
                        int chunk_count = (int)(size / CHUNK_SIZE);
                        string ref_path = REF_PATH + "/main.ref";

                        StringBuilder Ref_File = new StringBuilder();
                        Ref_File.Append("Target File Size: " + size);
                        Ref_File.Append("\nChunk Max Size: " + CHUNK_SIZE);
                        Ref_File.Append("\nLast Chunk Size: " + reminder);


                        Console.WriteLine("Size: " + size);
                        int s =0;

                        char[] buffer = new char[CHUNK_SIZE];
                        int c = 0;
                        Ref_File.Append("\n\nChunks Paths: ");
                        while ((s = sr.ReadBlock(buffer, 0, CHUNK_SIZE)) > 0)
                        {
                            Console.WriteLine("current chunk_count: " + chunk_count);
                            Console.WriteLine("current size: " + s);
                            string chunk_content = new string(buffer);
                            Console.WriteLine("chunk content:" + chunk_content);

                            //hash of content
                            byte[] bytes = Encoding.ASCII.GetBytes(chunk_content);
                            string hash = MD5Hash(chunk_content);
                            Console.WriteLine("hash : "+ hash);


                            c++;
                            //create chunk file
                            //get random path from  ABSOLUTE_PATHS ,ABSOLUTE_PATHS[rnd.Next()]
                            Random rnd = new Random();
                            string chunk_path = ABSOLUTE_PATHS[rnd.Next(ABSOLUTE_PATHS.Count)] + "/" + hash + EXTENTION;
                            Ref_File.Append("\n" + chunk_path);

                            using (FileStream sw = File.Create(chunk_path, CHUNK_SIZE))
                            {
                                foreach (byte b in bytes)
                                {
                                    sw.WriteByte(b);
                                }
                            }
                            //
                            chunk_count--;
                        }


                        //createRef(dist_path, Ref_File.ToString());
                        using (FileStream sw = File.Create(REF_PATH + "/main.ref", 1024))
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes(Ref_File.ToString());
                            foreach (byte b in bytes)
                            {
                                sw.WriteByte(b);
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("File Does Not Exists");
                    //using (FileStream fs = File.Create(path))
                    //{
                    //    byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                    //    // Add some information to the file.
                    //    fs.Write(info, 0, info.Length);
                    //}
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static void createRef(string path, string content)
        {
            using (FileStream sw = File.Create(path, 1024))
            {

                byte[] bytes = Encoding.ASCII.GetBytes(content);
                foreach (byte b in bytes)
                {
                    sw.WriteByte(b);
                }
            }
        }
        public void protect()
        {

        }
        public static string encrypt(string str)
        {
            return "";
        }


        public static void Main()
        {
            Random rnd = new Random();

            ABSOLUTE_PATHS.Add("/Users/thamerasiri/Desktop/ScatterPath1");
            ABSOLUTE_PATHS.Add("/Users/thamerasiri/Desktop/ScatterPath2");
            ABSOLUTE_PATHS.Add("/Users/thamerasiri/Desktop/ScatterPath3");

            Console.WriteLine(ABSOLUTE_PATHS[rnd.Next(ABSOLUTE_PATHS.Count)]) ;

            manageFile("/Users/thamerasiri/Desktop/Target.txt");
        }
    }
}
