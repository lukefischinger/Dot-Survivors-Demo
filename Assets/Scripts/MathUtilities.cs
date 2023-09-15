public static class MathUtilities
{
    
    // Mod function that correctly handles negative numbers
    // returns a mod n
    public static int Mod(int a, int n)
    {
        return (a % n + n) % n;
    }


}
