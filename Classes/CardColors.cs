namespace codenames_solver
{
    public enum CardColor
    {
        Blue,
        Red,
        Black,
        Neutral
    }
    public class CardColors
    {
        public record ColorIndeces(int[] Blue, int Black, int[] Red);

        public static ColorIndeces GenerateIndeces()
        {
            int[] allNumbers = Enumerable.Range(0, 25).ToArray();
            Random rand = new Random();

            int[] blue = allNumbers.OrderBy(x => rand.Next()).Take(8).ToArray();
            allNumbers = allNumbers.Except(blue).ToArray();

            int[] red = allNumbers.OrderBy(x => rand.Next()).Take(9).ToArray();
            allNumbers = allNumbers.Except(red).ToArray();

            int black = allNumbers[rand.Next(allNumbers.Length)];

            return new ColorIndeces(blue, black, red);
        }
    }
}
