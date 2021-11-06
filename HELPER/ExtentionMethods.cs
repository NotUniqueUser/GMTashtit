using System.Text;

namespace HELPER
{
    public static class ExtentionMethods
    {
        public static byte[] ToByteArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}