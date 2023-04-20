namespace Scripts.Logic
{
    public class Rectangle
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int Value { get; }
        public int Area => Width * Height;

        public Rectangle(int x, int y, int width, int height, int value)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Value = value;
        }
    }
}