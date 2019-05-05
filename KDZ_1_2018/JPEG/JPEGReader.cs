using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;


namespace JPEG
{
    public class jpg
    {
        public List<haffman> H;
        //quant Q;
        public List<List<List<int>>> Q;
        public List<flow> F;
        public List<flows> Fs;
        public List<List<List<List<int>>>> img;


        public jpg()
        {
            H = new List<haffman>();
            Q = new List<List<List<int>>>(); // quant();
            F = new List<flow>();
            Fs = new List<flows>();
            img = new List<List<List<List<int>>>>();
            //        Flow Number
            //             Tabe in flow number  
            //                  rows of table
            //                      cell of table
        }

        public void setQ(byte[] arr)
        {
            int len = (int)Math.Sqrt(arr.Length);
            if (len * len < arr.Length) len += 1;
            Q.Add(new List<List<int>>(len));
            for(int i = 0; i < len; ++i)
            {
                Q[Q.Count-1][i] = new List<int>(len);
            }
            int o = 1;
            int ji = 1, jj = -1;
            for (int i = 0; i < arr.Length; ++i)
            {
                flows.next(ref ji, ref jj, ref o, len);
                Q[Q.Count - 1][ji][jj] = arr[i];
                
            }
        }

        public byte[] readallbytes(FileStream f)
        {
            List<byte> l = new List<byte>();
            byte[] arr = new byte[1];
            byte[] arr2 = new byte[1];
            while (true)
            {
                f.Read(arr, 0, 1);
                if (arr[0] == 0xff)
                {
                    f.Read(arr2, 0, 1);
                    if (arr2[0] == 0xd9)
                    {
                        return l.ToArray<byte>();
                    }
                }
                l.Add(arr[0]);
            }
        }

    }

    public class node
    {
        public int v;
        public node l=null, r=null;
        public node p = null;

        public node() { }
        public node(int v)
        {
            this.v = v;
        }
        public node(int v, node p):this(v)
        {
            this.p = p;
        }
    }

    public class haffman
    {
        public int id=-1;
        public char c=' ';
        public node h = null, now = null;
        public int nowd = 0;

        public void add(int v, int d)
        {
            if (h == null)
            {
                h = new node(-1);
                now = h;
            }
            //else
            {
                while (nowd < d)
                {
                    now.l = new node(-1, h);
                    now = now.l;
                    nowd += 1;
                }
                now.v = v;
                now = now.p;
                nowd -= 1;
                while (now.r != null)
                {
                    now = now.p;
                    nowd -= 1;
                }

                now.r = new node(-1, h);
                now = now.r;
                nowd += 1;
            }
        }

        public haffman() { }
        public haffman(byte[] arr)
        {
            int n = 0;
            if (arr[n] / 16 == 0)
            {
                c = 'D';
            }
            if (arr[n] / 16 == 1)
            {
                c = 'A';
            }
            id = arr[n] % 16;
            n+=1;
            int[] count = new int[16];
            for(int i = 0; i < 16; ++i, ++n)
            {
                count[i] = arr[n];
            }
            for(int i = 0; i < count.Length; ++i)
            {
                while (count[i] > 0)
                {
                    count[i] -= 1;
                    add(arr[n++], i);
                }
            }

        }
    }

    public class flow
    {
        int id = -1;
        int Hs = 1;
        int Vs = 1;
        int Qid = -1;

        public flow() { }
        public flow(byte[] arr)
        {
            id = arr[0];
            Hs = arr[1] / 16;
            Vs = arr[1] % 16;
            Qid = arr[2];
        }

    }

    public class flows
    {
        int fid = -1;
        int HDCn = 1;
        int HACn = 1;

        public int i { get { return ji; } }
        public int j { get { return jj; } }
        public int pos { get { return now; } }

        int ji = 1;
        int jj = -1;
        int o = 1;

        int now = -1;

        public void next(int size = 8)
        {
            now += 1;
            
            ji -= o;
            jj += o;

            bool lastisrot = false;
            if (!lastisrot && jj > size - 1) // Turn right and go down
            {
                o *= -1;
                ji += 2;
                jj -= 1;
                lastisrot = true;
            }
            if (!lastisrot && ji < 0) // Turn right and go down
            {
                o *= -1;
                ji += 1;
                jj += 0;
                lastisrot = true;
            }
            if (!lastisrot && ji > size - 1) // Turn left and go up
            {
                o *= -1;
                ji -= 1;
                jj += 2;
                lastisrot = true;
            }
            if (!lastisrot && jj < 0) // Turn left and go up
            {
                o *= -1;
                ji += 0;
                jj += 1;
                lastisrot = true;
            }
            

        }

        static public void next(ref int ji, ref int jj, ref int o, int size = 8)
        {


            ji -= o;
            jj += o;

            bool lastisrot = false;
            if (!lastisrot && jj > size - 1) // Turn right and go down
            {
                o *= -1;
                ji += 2;
                jj -= 1;
                lastisrot = true;
            }
            if (!lastisrot && ji < 0) // Turn right and go down
            {
                o *= -1;
                ji += 1;
                jj += 0;
                lastisrot = true;
            }
            if (!lastisrot && ji > size - 1) // Turn left and go up
            {
                o *= -1;
                ji -= 1;
                jj += 2;
                lastisrot = true;
            }
            if (!lastisrot && jj < 0) // Turn left and go up
            {
                o *= -1;
                ji += 0;
                jj += 1;
                lastisrot = true;
            }


        }

        public flows() { }
        public flows(byte[] arr)
        {
            fid = arr[0];
            HDCn = arr[1] / 16;
            HACn = arr[1] % 16;
        }

    }

    //public class quant
    //{
    //    List<int> q;

    //    quant()
    //    {
    //        q = new List<int>();
    //    }
    //}

    public class JPEGReader
    {
        //public jpg image = null;

        //public JPEGReader()
        //{
        //    image = new jpg();
        //}

        public static int Read(string path, jpg image)
        {
            FileStream f = new FileStream(path, FileMode.Open);
            byte[] head = new byte[2];
            byte[] arr;// = new byte[2];
            byte[] comment = null;
            bool first = true;
            
            while (true)
            {
                head = new byte[2];
                f.Read(head, 0, 2); //Check header
                if (first && !(head[0] == 0xff && head[1] == 0xd8))
                {
                    return 1;
                }
                if(first && head[0] == 0xff && head[1] == 0xd8)
                {
                    first = false;
                    continue;
                }
                //f.Read(head, 0, 2); //Skip any comments
                if (head[0] == 0xff && head[1] == 0xfe)
                {
                    f.Read(head, 0, 2);
                    int len = head[0] * 256 + head[1] - 2;
                    comment = new byte[len];
                    f.Read(comment, 0, len);
                    continue;
                }
                //f.Read(head, 0, 2); //Quantirized table
                if (head[0] == 0xff && head[1] == 0xdb)
                {
                    head = new byte[3];
                    f.Read(head, 0, 3);
                    int len = head[0] * 256 + head[1] - 3;
                    arr = new byte[len];
                    bool onebyte = head[2] / 16 == 0;
                    int tableid = head[2] % 16;
                    f.Read(arr, 0, len);
                    image.setQ(arr);
                    continue;
                }
                //Base coding
                if (head[0] == 0xff && head[1] == 0xc0)
                {
                    f.Read(head, 0, 2);
                    int len = head[0] * 256 + head[1] - 2;
                    arr = new byte[len];
                    f.Read(arr, 0, len);
                    int precision = arr[0];
                    int height = arr[1] * 256 + arr[2];
                    int width = arr[3] * 256 + arr[4];
                    int components = arr[5];
                    for(int i = 0; i < components; ++i)
                    {
                        image.F.Add(new flow(new byte[] { arr[6 + i * 3], arr[7 + i * 3], arr[8 + i * 3] }));
                    }
                    continue;
                }
                if (head[0] == 0xff && head[1] == 0xc4)
                {
                    f.Read(head, 0, 2);
                    int len = head[0] * 256 + head[1] - 2;
                    arr = new byte[len];
                    f.Read(arr, 0, len);
                    image.H.Add(new haffman(arr));
                    continue;
                }
                if (head[0] == 0xff && head[1] == 0xda)
                {
                    f.Read(head, 0, 2);
                    int len = head[0] * 256 + head[1] - 3;
                    int components = head[2];
                    arr = new byte[len];
                    f.Read(arr, 0, len);
                    for (int i = 0; i < components; ++i)
                    {
                        image.Fs.Add(new flows(new byte[] { arr[i * 2], arr[1 + i * 2]}));
                    }
                    arr = new byte[1];
                    byte[] arr2 = new byte[1];
                    for(int i = 0; i < image.H.Count; ++i)
                    {
                        image.H[i].now = image.H[i].h;
                    }
                    int fn = 0;
                    int i = 2;
                    int j = 8;
                    //readallbytes(f);
                    while (true)
                    {
                        fn += 1;
                        fn %= image.Fs.Count;
                        start:if (i == 2 && j == 8)
                        {
                            i = 0;
                            j = 0;

                            f.Read(arr, 0, 1);
                            if (arr[0] == 0xff)
                            {
                                f.Read(arr2, 0, 1);
                                if (arr2[0] == 0xd9)
                                {
                                    return 0;
                                }
                            }
                        }
                        for(i; i < 2; ++i)
                        {
                            for(j; j < 8; ++j)
                            {
                                if (((1 << (j + (1 - i) * 8)) & arr[0]) == 0)
                                {
                                    image.H[fn].now = image.H[fn].now.l;
                                    if (image.H[fn].now.v != -1)
                                    {
                                        image.Fs[fn].next();
                                        if (image.Fs[fn].pos >= 64)
                                        {
                                            goto end;
                                        }
                                        if (image.H[fn].now.v == 0)
                                        {
                                            image.img[fn][image.img[fn].Count - 1][image.Fs[fn].i][image.Fs[fn].j] = image.H[fn].now.v;
                                        }
                                        else {
                                            image.img[fn][image.img[fn].Count - 1][image.Fs[fn].i][image.Fs[fn].j] =
                                        } 
                                        image.H[fn].now = image.H[fn].h;
                                    }
                                }
                                else
                                {
                                    image.H[fn].now = image.H[fn].now.r;

                                }

                            }
                        }
                        end: int prostometka = 0;
                    }
                    continue;
                }

                break;
            }
            return 0;
        }

    }
}
