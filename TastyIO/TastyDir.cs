using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace TastyIO
{
    static public class TastyDir
    {

        public static bool Verify(string dir)
        {
            string root = dir.Split("\\").First();

            //Win32
            switch (Environment.OSVersion.Platform)
            {

                case PlatformID.Win32NT:
                    Regex rootVerification = new (@"[A-Z]:\\", RegexOptions.Compiled);
                    string unRooted = Path.GetRelativePath(root, dir);

                    if (dir.EndsWith(' ') || dir.EndsWith('.'))
                        return false;

                    if (dir.Length >= Win.maxPathLenght)
                        return false;

                    if(!rootVerification.IsMatch(root))
                    {
                        return false;
                    }

                    foreach( char character in unRooted.ToUpper().ToCharArray())
                    {
                        if (Win.forbiddenChars.Contains(character))
                            return false;
                    }

                    foreach(string part in dir.Split("\\"))
                    {
                        if (Win.reservedNames.Contains(part.ToUpper()))
                            return false;
                    }
                    break;

                case PlatformID.Unix:
                    //Linux and macos
                    if(dir.Length >= Unix.maxPathLenght)
                    {
                        return false;
                    }
                    if(dir.Split("\\").Last().Length >= Unix.maxFilenameLenght)
                    {
                        return false;
                    }
                    break;


                case PlatformID.Other:
                    //Unkown
                    throw new PlatformNotSupportedException($"{Environment.OSVersion} is not supported.");
            }
            

            return true;
        }

        public static string[] GetDirecoriesRecursive(string dir)
        {
            return GetDirecories(dir);
        }

        private static string[] GetDirecories(string parrentDir)
        {
            List<string> result = new();
            try
            {
                string[] dirs = Directory.GetDirectories(parrentDir);
                foreach(string dir in dirs)
                {
                    result.AddRange(GetDirecories(dir));
                }
            }
            catch
            {

            }
            return result.ToArray();
        }
        
    }
}