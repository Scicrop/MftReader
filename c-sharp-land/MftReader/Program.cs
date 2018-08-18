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
                Console.Write("Processing...\r");
                if (args.Length != 3)
                {
                    Utils.Instance.ThrowErr(null);
                }

                string driveLetter = args[0];   // C
                string fileNamePath = args[1];  // c:/temp;
                string fileExtension = args[2].ToLower(); // .txt;

                string fileNamePathRecommendation = "The second argument (" + fileNamePath + ") must be a valid folder, and the current user must have write access in it. The valid slash must be / and NOT \\.";

                if (!Directory.Exists(fileNamePath))
                {
                    Utils.Instance.ThrowErr(fileNamePathRecommendation);
                }
                else if (fileNamePath[fileNamePath.Length - 1] == '/') fileNamePath = fileNamePath.Substring(0, fileNamePath.Length - 1);
                

                if (driveLetter.Length > 1 || driveLetter.Contains(":") || fileNamePath.Contains("\\") || !fileExtension.Contains(".") || fileExtension.Contains("*"))
                {
                    Utils.Instance.ThrowErr("\n\n1. The first argument ("+driveLetter+") must be JUST a NTFS volume/drive letter WITHOUT ':', like C, D, F, G, etc. The current user must have administration rights over this volume.\n\n" +
                        "2. "+fileNamePathRecommendation+"\n\n" +
                        "3. The third argument ("+fileExtension+ ") is the representation of a file extension, like .txt, .pdf, .doc, etc, WITH dot (.) WITHOUT asterisk (*).");
                }


                nameLst = new List<string>();
                Dictionary<ulong, FileNameAndParentFrn> mDict = new Dictionary<ulong, FileNameAndParentFrn>();

                long totalBytes = 0l;
                EnumerateVolume.PInvokeWin32 mft = new EnumerateVolume.PInvokeWin32();
                mft.Drive = driveLetter;
                mft.Drive = mft.Drive + ":";
                mft.EnumerateVolume(out mDict);
                StringBuilder sb = new StringBuilder();
                StringBuilder pathSb = null;
                String jsonFileNamePath = fileNamePath + "/" + driveLetter + fileExtension + ".json";

                Console.Write("Volume: " + driveLetter+"\t\t\n");
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
                int ownerExceptionCount = 0;
                int machineNameExceptionCount = 0;
                int fqdnExceptionCount = 0;
                for (int i = 0; i < finalNameLst.Count; i++)
                {

                    String item = finalNameLst[i];

                    if (File.Exists(item))
                    {
                        if (i + 1 == finalNameLst.Count) comma = "";
                        long fileSize = new FileInfo(item).Length;
                        DateTime fileCreationDate = File.GetCreationTime(item);
                        DateTime fileUpdateDate = File.GetLastWriteTime(item);
                        string owner = "n/a";
                        string machineName = "n/a";
                        string fqdn = "n/a";

                        try { owner = Utils.Instance.GetOwnerName(item); }
                        catch (Exception) 
                        {
                            ownerExceptionCount++;    
                        } finally { owner = Uri.EscapeDataString(owner); }

                        try { machineName = Environment.MachineName; }
                        catch (Exception)
                        {
                            machineNameExceptionCount++;
                        }  finally { machineName = Uri.EscapeDataString(machineName); }

                        try { fqdn = Utils.Instance.GetFQDN(); }
                        catch (Exception)
                        {
                            fqdnExceptionCount++;
                        }
                        finally { fqdn = Uri.EscapeDataString(fqdn); }

                        sb.AppendLine("{ \"fileName\": \"" + Uri.EscapeDataString(item) + "\", \"fileSize\": " + fileSize + ", \"fileCreationDate\": \"" + fileCreationDate + "\", \"fileUpdateDate\": \"" + fileUpdateDate + "\", \"fileAuthor\": \"" + owner + "\", \"fqdn\": \""+ fqdn +"\", \"machineName\": \""+ machineName +"\"}" + comma);
                        totalBytes = fileSize + totalBytes;
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
                Console.WriteLine("FQDN resolution errors: " + fqdnExceptionCount);
                Console.WriteLine("Machine Name resolution errors: " + machineNameExceptionCount);
                Console.WriteLine("File ownership resolution errors: " + ownerExceptionCount);
                Console.WriteLine("\nTotal bytes: " + Utils.Instance.FormatBytesLength(totalBytes));
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
