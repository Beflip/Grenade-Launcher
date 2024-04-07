public static class CustomGravity
{
    public static float Value { get; private set; }

    static CustomGravity()
    {
        Value = 9.81f;
    }
}