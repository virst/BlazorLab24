namespace BlazorLab24
{
    public class Rcv
    {
        int r;
        int c;
        int v;
        readonly int w;

        public Rcv(int W)
        {
            w = W;
            Val = 0;
        }

        public int Row
        {
            get { return r; }
            set { r = value; v = Row * w + Col; }

        }

        public int Col
        {
            get { return c; }
            set { c = value; v = Row * w + Col; }
        }

        public int Val
        {
            get { return v; }
            set { v = value; r = v / w; c = v % w; }
        }

        public void SetRC(int R, int C)
        {
            Row = R;
            Col = C;
        }

        public void SetVal(int V)
        {
            Val = V;
        }
    }
}
