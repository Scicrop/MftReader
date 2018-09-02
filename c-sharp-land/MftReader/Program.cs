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
                Console.WriteLine("### "+Constants.APP_SIGNATURE+" ###");
                Console.WriteLine("### " + Constants.APP_URL + " ###\n");
                Console.Write("Processing...\r");

                Int32 unixTimestampInit = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                string driveLetter = Properties.Settings.Default.search_volume;
                string fileNamePath = Properties.Settings.Default.report_folder;
                string fileExtension = Properties.Settings.Default.search_extensions;
                bool calcMd5 = Properties.Settings.Default.calc_md5;

                string fileNamePathRecommendation = "report_folder (" + fileNamePath + ") must be a valid folder, and the current user must have write access in it. The valid slash must be / and NOT \\.";

                if (!Directory.Exists(fileNamePath))
                {
                    Utils.Instance.ThrowErr(fileNamePathRecommendation);
                }
                else if (fileNamePath[fileNamePath.Length - 1] == '/') fileNamePath = fileNamePath.Substring(0, fileNamePath.Length - 1);
                

                if (driveLetter.Length > 1 || driveLetter.Contains(":") || fileNamePath.Contains("\\") || !fileExtension.Contains(".") || fileExtension.Contains("*"))
                {
                    Utils.Instance.ThrowErr("\n\nCheck the config file:\n\n1. search_volume (" + driveLetter+") must be JUST a NTFS volume/drive letter WITHOUT ':', like C, D, F, G, etc. The current user must have administration rights over this volume.\n\n" +
                        "2. "+fileNamePathRecommendation+"\n\n" +
                        "3. search_extensions (" + fileExtension+ ") is the representation of a file extension, like .txt, .pdf, .doc, etc, WITH dot (.) WITHOUT asterisk (*).");
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
                String jsonFileNamePath = fileNamePath + "/" + driveLetter + "." + unixTimestampInit + ".json";

                Console.Write("Volume: " + driveLetter+"\t\t\n");
                Console.WriteLine("Report folder: " + fileNamePath);
                Console.WriteLine("Extension: " + fileExtension);

                long totalDriveSize = 0;

                try
                {
                    totalDriveSize = Utils.Instance.GetDriveTotalSize(driveLetter);
                }
                catch(Exception e)
                {
                    Utils.Instance.LogException(e);
                }

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

                    if (extractedExtension != null && fileExtension.ToLower().Contains(extractedExtension.ToLower()))
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
                sb.AppendLine("{\"initTime\": "+ unixTimestampInit + ", \"search_volume\": \""+driveLetter+"\", \"report_folder\": \""+fileNamePath+"\", \"search_extensions\": \""+fileExtension+ "\", \"totalDriveSize\": "+ totalDriveSize + ", \"objectLst\": [");
                String comma = ", ";
                int notFoundCount = 0;
                int ownerExceptionCount = 0;
                int machineNameExceptionCount = 0;
                int fqdnExceptionCount = 0;
                int md5ExceptionCount = 0;
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
                        string md5Hash = "";


                        if (calcMd5)
                        {
                            try
                            {
                                byte[] byteArray = File.ReadAllBytes(item);
                                string strMd5 = Utils.Instance.ByteArrayToMd5HashString(byteArray);
                                md5Hash = ", \"md5\": \""+ strMd5 + "\"";
                            }
                            catch(Exception e)
                            {
                                md5ExceptionCount++;
                                md5Hash = ", \"md5\": \"n/a\"";
                                Utils.Instance.LogException(e);
                            }
                        }


                        try { owner = Utils.Instance.GetOwnerName(item); }
                        catch (Exception e) 
                        {
                            ownerExceptionCount++;
                            Utils.Instance.LogException(e);
                        } finally { owner = Uri.EscapeDataString(owner); }

                        try { machineName = Environment.MachineName; }
                        catch (Exception e)
                        {
                            machineNameExceptionCount++;
                            Utils.Instance.LogException(e);
                        }  finally { machineName = Uri.EscapeDataString(machineName); }

                        try { fqdn = Utils.Instance.GetFQDN(); }
                        catch (Exception e)
                        {
                            fqdnExceptionCount++;
                            Utils.Instance.LogException(e);
                        }
                        finally { fqdn = Uri.EscapeDataString(fqdn); }

                        sb.AppendLine("{ \"fileName\": \"" + Uri.EscapeDataString(item) + "\", \"fileSize\": " + fileSize + ", \"fileCreationDate\": \"" + fileCreationDate + "\", \"fileUpdateDate\": \"" + fileUpdateDate + "\", \"fileAuthor\": \"" + owner + "\", \"fqdn\": \""+ fqdn +"\" "+md5Hash+"}" + comma);
                        totalBytes = fileSize + totalBytes;
                    }
                    else
                    {
                        notFoundCount++;
                    }

                    Console.Write("Inspecting file: " + (i + 1) + "/" + finalNameLst.Count + "\r");
                }
                Int32 unixTimestampEnd = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                sb.AppendLine("], \"endTime\": "+unixTimestampEnd+" }");

                Utils.Instance.WriteToFile(sb.ToString(), jsonFileNamePath);

                Console.WriteLine("\nFile references not found: " + notFoundCount);
                Console.WriteLine("FQDN resolution errors: " + fqdnExceptionCount);
                Console.WriteLine("Machine Name resolution errors: " + machineNameExceptionCount);
                Console.WriteLine("File ownership resolution errors: " + ownerExceptionCount);
                Console.WriteLine("MD5 hash errors: " + md5ExceptionCount);
                Console.WriteLine("\nTotal bytes: " + Utils.Instance.FormatBytesLength(totalBytes));
                Console.WriteLine("Process ended. Check the results in: " + jsonFileNamePath);

                Console.WriteLine("\n[PRESS ENTER]");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Utils.Instance.ThrowErr(e.Message);
                Utils.Instance.LogException(e);
            }

        }
    }
        


}
