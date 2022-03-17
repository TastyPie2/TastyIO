namespace TastyIO.OS
{
    internal static class Win
    {
        #region win_verification
        public readonly static int maxPathLenght = 260;
        public readonly static char[] forbiddenChars = new char[] { '<', '>', ':', '"', '|', '?', '*' };
        public readonly static string[] reservedNames = new string[] { "CON", "PRN", "AUX", "NUL",
        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"};
        #endregion
    }
}
