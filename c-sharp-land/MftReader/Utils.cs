using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
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
    }
}

