using System;

public static class StringExtension
{
    public static int ExtractNumber(this string text)
    {
        string number = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            if (Char.IsDigit(text[i]))
                number += text[i];
        }

        if (number.Length > 0)
            return int.Parse(number);
        return 0;
    }
}
