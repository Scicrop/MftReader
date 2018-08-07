using System;
using System.Collections.Generic;
using System.Text;

namespace MftReader
{
    public class Utils
    {
        public String extractExtension(String fileName)
        {
            String extension = null;
            for (int i = fileName.Length-1; i >= 0;  i--)
            {
                if (fileName[i] == '.')
                {
                    extension = fileName.Substring(i, fileName.Length - (i));
                    break;
                }
            }
            return extension;
        }
    }
}
