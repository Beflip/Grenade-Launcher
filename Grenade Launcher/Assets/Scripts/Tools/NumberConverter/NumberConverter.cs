using UnityEngine;
using System.Globalization;

public class NumberConverter : MonoBehaviour
{
    public static string Convert(double number)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int suffixIndex = 0;

        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }

        string formatString = number >= 10 ? "0.0" : "0.00";

        CultureInfo culture = new CultureInfo("en-US");

        string formattedNumber = number.ToString(formatString, culture);

        return suffixIndex != 0 ? string.Format("{0}{1}", formattedNumber, suffixes[suffixIndex]) : number.ToString();
    }
}