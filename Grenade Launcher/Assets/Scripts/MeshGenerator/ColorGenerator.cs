using UnityEngine;

static class ColorGenerator
{
    public static Color GenerateRandomColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue);
    }
}