using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;


namespace MftReader
{
    class Program
    {

        public static List<String> nameLst = null;
        public static List<String> finalNameLst = null;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("### MftReader ###");
                Console.WriteLine("### https://github.com/Scicrop/MftReader ###\n");

                if (args.Length != 3)
                {
                    Utils.Instance.ThrowErr(null);
                }

                String driveLetter = args[0];   // C
                String fileNamePath = args[1];  // c:/temp;
                String fileExtension = args[2].ToLower(); // .txt;

                if (driveLetter.Length > 1 || driveLetter.Contains(":") || fileNamePath.Contains("\\") || !fileExtension.Contains("."))
                {
                    Utils.Instance.ThrowErr(null);
                }


                nameLst = new List<string>();
                Dictionary<ulong, FileNameAndParentFrn> mDict = new Dictionary<ulong, FileNameAndParentFrn>();

                EnumerateVolume.PInvokeWin32 mft = new EnumerateVolume.PInvokeWin32();
                mft.Drive = driveLetter;
                mft.Drive = mft.Drive + ":";
                mft.EnumerateVolume(out mDict);
                StringBuilder sb = new StringBuilder();
                StringBuilder pathSb = null;
                String jsonFileNamePath = fileNamePath + "/" + driveLetter + ".json";

                Console.WriteLine("Volume: " + driveLetter);
                Console.WriteLine("Report folder: " + fileNamePath);
                Console.WriteLine("Extension: " + fileExtension);

                if (File.Exists(jsonFileNamePath))
                {
                    File.Delete(jsonFileNamePath);
                    Console.WriteLine("Old file deleted: " + jsonFileNamePath);
                }

                finalNameLst = new List<string>();
                Console.WriteLine("MFT items: " + mDict.Count);
                foreach (KeyValuePair<UInt64, FileNameAndParentFrn> entry in mDict)
                {
                    FileNameAndParentFrn file = (FileNameAndParentFrn)entry.Value;
                    pathSb = new StringBuilder();

                    string extractedExtension = Utils.Instance.ExtractExtension(file.Name);

                    if (extractedExtension != null && extractedExtension.ToLower().Equals(fileExtension))
                    {
                        pathSb.Append(driveLetter + ":\\");
                        Utils.Instance.SearchId(file.ParentFrn, mDict);

                        nameLst.Reverse();

                        foreach (var item in nameLst)
                        {
                            pathSb.Append(item + "\\");
                        }

                        pathSb.Append(file.Name);

                        finalNameLst.Add(pathSb.ToString());

                        Console.Write("File references found: " + finalNameLst.Count + "\r");

                        nameLst.Clear();
                    }
                }

                Console.WriteLine();
                sb.AppendLine("{\"objectLst\": [");
                String comma = ", ";
                int notFoundCount = 0;
                for (int i = 0; i < finalNameLst.Count; i++)
                {

                    String item = finalNameLst[i];

                    if (File.Exists(item))
                    {
                        if (i + 1 == finalNameLst.Count) comma = "";
                        long fileSize = new System.IO.FileInfo(item).Length;
                        DateTime fileCreationDate = File.GetCreationTime(item);
                        DateTime fileUpdateDate = File.GetLastWriteTime(item);

                        sb.AppendLine("{ \"fileName\": \"" + Uri.EscapeDataString(item) + "\", \"fileSize\": " + fileSize + ", \"fileCreationDate\": \"" + fileCreationDate + "\", \"fileUpdateDate\": \"" + fileUpdateDate + "\", \"fileAuthor\": \"" + Uri.EscapeDataString(Utils.Instance.GetOwnerName(item)) + "\"}" + comma);
                    }
                    else
                    {
                        notFoundCount++;
                    }

                    Console.Write("Inspecting file: " + (i + 1) + "/" + finalNameLst.Count + "\r");
                }

                sb.AppendLine("]}");

                Utils.Instance.WriteToFile(sb.ToString(), jsonFileNamePath);

                Console.WriteLine("\nFile references not found: " + notFoundCount);
                Console.WriteLine("Process ended. Check the results in: " + jsonFileNamePath);

                Console.WriteLine("\n[PRESS ENTER]");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Utils.Instance.ThrowErr(e.Message);
            }

        }
    }
        


}
