public static class ExtensionString
{
    
    // testing for a string with a length of 2553
    public static string CustomReverse(this string script)
    {
        string strValue = script;
        int strLength = strValue.Length;

        // Allocate HGlobal memory for source and destination strings
        IntPtr sourcePtr = Marshal.StringToHGlobalAnsi(strValue);
        IntPtr destPtr = Marshal.AllocHGlobal(strLength + 1);

        // The unsafe section where byte pointers are used.
        unsafe
        {
            byte *src = (byte *)sourcePtr.ToPointer();
            byte *dst = (byte *)destPtr.ToPointer();

            if (strLength > 0)
            {
                // set the source pointer to the end of the string
                // to do a reverse copy.
                src += strLength - 1;
                while (strLength-- > 0)
                {
                    *dst++ = *src--;
                }
                *dst = 0;
            }
        }
        var result = Marshal.PtrToStringAnsi(destPtr);
        Marshal.FreeHGlobal(sourcePtr);
        Marshal.FreeHGlobal(destPtr);

        return result;
    }
    
    public static string LinqReverse(this string script)
    {
        var tmpString = new StringBuilder();
        var reversedStr = script.Reverse();
        foreach (var c in reversedStr)
        {
            tmpString.Append(c);
        }

        return tmpString.ToString();
    }
}
