using UnityEngine;

public static class MaterialGenerator
{
    public static Material GenerateRandomMaterial()
    {
        Material material = new Material(Shader.Find("Standard"));
        material.color = ColorGenerator.GenerateRandomColor();
        return material;
    }
}