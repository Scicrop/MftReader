using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace MftReader
{
    public class Utils
    {

        private static readonly Utils instance = new Utils();


        static Utils()
        {
        }

        private Utils()
        {
        }

        public static Utils Instance
        {
            get
            {
                return instance;
            }
        }

        public String ExtractExtension(String fileName)
        {
            String extension = null;
            if (fileName.Contains("."))
            {
                for (int i = fileName.Length - 1; i >= 0; i--)
                {
                    if (fileName[i] == '.')
                    {
                        extension = fileName.Substring(i, fileName.Length - (i));
                        break;
                    }
                }
            }
            return extension;
        }

        public string GetOwnerName(string path)
        {
            //string user = System.IO.File.GetAccessControl(path).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            FileSecurity fileSecurity = File.GetAccessControl(path);
            IdentityReference identityReference = fileSecurity.GetOwner(typeof(SecurityIdentifier));
            NTAccount ntAccount = identityReference.Translate(typeof(NTAccount)) as NTAccount;
            return ntAccount.Value;
        }



        public void ThrowErr(string extraMessage)
        {
            String message = "Invalid parameters. Usage example: MftReader.exe C C:/temp/ .txt";
            Console.WriteLine(message);
            if (extraMessage != null)
            {
                Console.WriteLine(extraMessage);
            }
            Console.WriteLine("\n[PRESS ENTER]");
            Console.ReadLine();

            System.Environment.Exit(0);
        }

        public FileNameAndParentFrn SearchId(ulong key, Dictionary<ulong, FileNameAndParentFrn> mDict)
        {

            FileNameAndParentFrn file = null;
            if (mDict.ContainsKey(key))
            {
                file = mDict[key];

                Program.nameLst.Add(file.Name);

                while (file != null)
                {
                    file = SearchId(file.ParentFrn, mDict);
                }
            }


            return file;
        }

        public void WriteToFile(String line, String fileNamePath)
        {
            try
            {

                line = line + Environment.NewLine;
                if (!File.Exists(fileNamePath))
                {

                    File.WriteAllText(fileNamePath, line);

                } else File.AppendAllText(fileNamePath, line);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            domainName = "." + domainName;
            if (!hostName.EndsWith(domainName))
            {
                hostName += domainName;
            }

            return hostName;
        }


        public string FormatBytesLength(long length)
        {
            int chunk = 1024;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = (double)length;
            int order = 0;
            while (len >= chunk && order < sizes.Length - 1)
            {
                ++order;
                len = len / chunk;
            }
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }

        private void WriteEventLog(string message)
        {
            try
            {
                string sSource = Constants.APP_SIGNATURE;
                string sLog = "Application";

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, message, EventLogEntryType.Information);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LogException(Exception e)
        {
            WriteEventLog("Exception: " + e.Message + " [" + e.StackTrace + "]");
        }

        public string ByteArrayToMd5HashString(byte[] input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(input);

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public long GetDriveTotalSize(string driveLetter)
        {
            long totalSize = 0;
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if(d.Name.ToLower().Equals(driveLetter.ToLower()+":\\"))
                {
                    totalSize = d.TotalSize;
                    break;
                }
            }

            return totalSize;
        }
    }
}

