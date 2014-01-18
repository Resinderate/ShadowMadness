//no usings are required

namespace GDLibrary.Utilities
{
    public class Integer2
    {
        private int x, y;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public Integer2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
