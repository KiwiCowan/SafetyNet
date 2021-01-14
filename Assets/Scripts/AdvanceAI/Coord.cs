public class Coord
{
    public int row;
    public int col;

    public Coord(int r, int c)
    {
        row = r;
        col = c;
    }

    public static string ToString(Coord instance)
    {
        return instance.row + "-" + instance.col;
    }
}
