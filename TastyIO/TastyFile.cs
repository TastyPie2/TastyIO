namespace TastyIO
{
    public static class TastyFile
    {
        public static bool Verify(string path)
        {
            return TastyDir.Verify(path);
        }

        public static string[] GetFilesRecursive(string dir)
        {
            List<string> result = new();

            foreach (string di in TastyDir.GetDirecoriesRecursive(dir))
            {
                result.AddRange(Directory.GetFiles(di));
            }

            return result.ToArray();
        }
    }
}
