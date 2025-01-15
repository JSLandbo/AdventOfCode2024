public static class StringExtensions 
{  
    public static int ToIntFast(this string input)
    {
        int result = 0;  
        int sign = 1;  
        int startIndex = 0; 
        if (input[0] == '-')
        { 
            sign = -1;  
            startIndex = 1;  
        } 
        for (int i = startIndex; i < input.Length; i++)
        {
            result = (result * 10) + (input[i] - '0');
        }  
        return result * sign; 
    }
}