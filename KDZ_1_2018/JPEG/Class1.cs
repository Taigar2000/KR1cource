using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPEG
{
    public class Block
    {
        public List<List<int>> b;
        /// <summary>
        /// Block 8x8
        /// </summary>
        Block()
        {
            int n = 8;
            b = new List<List<int>>(n);
            for(int i = 0; i < n; ++i) {
                b[i] = new List<int>(n);
            }
        }
        /// <summary>
        /// Block nxn
        /// </summary>
        /// <param name="n">Count of rows and columns</param>
        Block(int n)
        {
            b = new List<List<int>>(n);
            for (int i = 0; i < n; ++i)
            {
                b[i] = new List<int>(n);
            }
        }
        /// <summary>
        /// Copy
        /// </summary>
        /// <param name="bl"></param>
        Block(List<List<int>> bl)
        {
            List<List<int>> bl1 = new List<List<int>>(bl.Count);
            for(int i = 0; i < bl.Count; ++i)
            {
                bl1[i] = new List<int>(bl[i].Count);
                for(int j = 0; j < bl[i].Count; ++j)
                {
                    bl1[i][j] = bl[i][j];
                }
            }
            b = bl1;
        }

        public int this[int i, int j]
        {
            get {
                i = (b.Count + i) % b.Count;
                j = (b[i].Count + j) % b[i].Count;

                return b[i][j];
            }

            set
            {
                i = (b.Count + i) % b.Count;
                j = (b[i].Count + j) % b[i].Count;

                b[i][j] = value;
            }
        }

    }



    public class Doing
    {
        public void Normalize(Block b)
        {
            for(int i = 0; i < b.b.Count;++i)
            {
                for (int j = 0; j < b.b[i].Count; ++j)
                {
                    b.b[i][j] -= 128;
                }
            }
        }

    }

}
