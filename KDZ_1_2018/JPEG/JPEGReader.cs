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
        public List<byte[]> Harr;
        //quant Q;
        public List<List<List<int>>> Q;
        public List<bool> onebyte;
        public List<int> Qid;
        public List<byte[]> Qarr;
        public List<flow> F;
        public List<byte[]> Farr;
        public List<flows> Fs;
        public List<List<int[,]>> img;
        public byte[] imgharr;
        public byte[] imgarr;


        public jpg()
        {
            H = new List<haffman>();
            Harr = new List<byte[]>();
            Q = new List<List<List<int>>>(); // quant();
            onebyte = new List<bool>(); // q elems length
            Qid = new List<int>(); //quantid
            Qarr = new List<byte[]>();
            F = new List<flow>();
            Farr = new List<byte[]>();
            Fs = new List<flows>();
            img = new List<List<int[,]>>();
            //        Flow Number
            //             Tabe in flow number  
            //                  rows of table
            //                      cell of table
        }

        public void setQ(byte[] arr, bool ob, int id)
        {
            onebyte.Add(ob);
            Qid.Add(id);
            Qarr.Add(arr);
            int len = (int)Math.Sqrt(arr.Length);
            if (len * len < arr.Length) len += 1;
            Q.Add(new List<List<int>>());
            for (int i = 0; i < len; ++i)
            {
                Q[Q.Count - 1].Add(new List<int>(len));
                for (int j = 0; j < len; ++j)
                {
                    Q[Q.Count - 1][i].Add(0);
                }
            }
            int o = 1;
            int ji = 1, jj = -1;
            for (int i = 0; i < arr.Length; ++i)
            {
                flows.next(ref ji, ref jj, ref o, len);
                if (ob)
                {
                    Q[Q.Count - 1][ji][jj] = arr[i];
                }
                else
                {
                    Q[Q.Count - 1][ji][jj] = arr[i] * 256 + arr[i + 1];
                    i++;
                }
            }
        }

        public byte[] getQ(int id)
        {
            List<byte> arr = new List<byte>();
            int len = Q[id].Count;
            int o = 1;
            int ji = 1, jj = -1;
            for (int i = 0; i < len * len; ++i)
            {
                flows.next(ref ji, ref jj, ref o, len);
                if (onebyte[id])
                {
                    arr.Add((byte)(Math.Min(Q[Q.Count - 1][ji][jj],256)));
                }
                else
                {
                    arr.Add((byte)(Math.Min(((Q[Q.Count - 1][ji][jj]) / 256),256)));
                    arr.Add((byte)(Math.Min(((Q[Q.Count - 1][ji][jj]) % 256),256)));
                }
            }

            return (arr.ToArray<byte>());
        }

        public byte[] readallbytes(FileStream f)
        {
            List<byte> l = new List<byte>();
            List<byte> imga = new List<byte>();
            byte[] arr = new byte[1];
            byte[] arr2 = new byte[1];
            while (true)
            {
                f.Read(arr, 0, 1);
                imga.Add(arr[0]);
                if (arr[0] == 0xff)
                {
                    f.Read(arr2, 0, 1);
                    imga.Add(arr2[0]);
                    if (arr2[0] == 0xd9)
                    {
                        imgarr = imga.ToArray<byte>();
                        return l.ToArray<byte>();
                    }
                }
                l.Add(arr[0]);
            }
        }


        public static jpg operator *(jpg imgo, double a)
        {
            jpg img = imgo;
            for (int i = 0; i < img.Q.Count; ++i)
            {
                for (int j = 0; j < img.Q[i].Count; ++j)
                {
                    for (int k = 0; k < img.Q[i][j].Count; ++k)
                    {
                        img.Q[i][j][k] = (int)(img.Q[i][j][k] * a);
                    }
                }
            }
            return img;
        }

        public static jpg operator *(double a, jpg imgo)
        {
            return imgo * a;
        }

        public static jpg operator +(jpg imgo, int a)
        {
            jpg img = imgo;
            for (int i = 0; i < img.Q.Count; ++i)
            {
                for (int j = 0; j < img.Q[i].Count; ++j)
                {
                    for (int k = 0; k < img.Q[i][j].Count; ++k)
                    {
                        if (j == 0 && k == 0)
                        {
                            //img.Q[i][j][k] +=8*a/img.
                        }
                        else
                        {
                            img.Q[i][j][k] *= a;
                        }
                    }
                }
            }
            return img;
        }


    }

    public class node
    {
        public int v;
        public node l = null, r = null;
        public node p = null;

        public node() { }
        public node(int v)
        {
            this.v = v;
        }
        public node(int v, node p) : this(v)
        {
            this.p = p;
        }
    }

    public class haffman
    {
        public int id = -1;
        public char c = ' ';
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
                    now.l = new node(-1, now);
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
                    if (nowd == 0)
                    {
                        break;
                    }
                }
                if (now.r == null)
                {
                    now.r = new node(-1, now);
                    now = now.r;
                    nowd += 1;
                }
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
            n += 1;
            int[] count = new int[16];
            for (int i = 0; i < 16; ++i, ++n)
            {
                count[i] = arr[n];
            }
            for (int i = 0; i < count.Length; ++i)
            {
                while (count[i] > 0)
                {
                    count[i] -= 1;
                    add(arr[n++], i+1);
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
            FileStream f = null;
            try
            {
                f = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);
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
                    if (first && head[0] == 0xff && head[1] == 0xd8)
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
                        f.Read(arr, 0, len * (onebyte ? 1 : 2));
                        image.setQ(arr, onebyte, tableid);
                        continue;
                    }
                    //Base coding
                    if (head[0] == 0xff && head[1] == 0xc0)
                    {
                        f.Read(head, 0, 2);
                        int len = head[0] * 256 + head[1] - 2;
                        arr = new byte[len];
                        f.Read(arr, 0, len);
                        image.Farr.Add(arr);
                        int precision = arr[0];
                        int height = arr[1] * 256 + arr[2];
                        int width = arr[3] * 256 + arr[4];
                        int components = arr[5];
                        for (int i = 0; i < components; ++i)
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
                        image.Harr.Add(arr);
                        continue;
                    }
                    if (head[0] == 0xff && head[1] == 0xda)
                    {
                        List<byte> headarr = new List<byte>();
                        head = new byte[3];
                        f.Read(head, 0, 3);
                        headarr.AddRange(head);
                        int len = head[0] * 256 + head[1] - 3;
                        int components = head[2];

                        arr = new byte[len];
                        f.Read(arr, 0, len);
                        headarr.AddRange(arr);
                        int i = 0;
                        int j = 8;
                        for (i = 0; i < components; ++i)
                        {
                            image.Fs.Add(new flows(new byte[] { arr[i * 2], arr[1 + i * 2] }));
                        }
                        image.imgharr = headarr.ToArray<byte>();

                        arr = new byte[1];
                        byte[] arr2 = new byte[1];
                        for (i = 0; i < image.H.Count; ++i)
                        {
                            image.H[i].now = image.H[i].h;
                        }
                        image.img.Add(new List<int[,]>());
                        int fn = 0;
                        i = 0;
                        j = 8;
                        byte[] b = image.readallbytes(f);
                        bool changefn = true;
                        int value = 0;
                        int readnbits = 0;
                        bool DC = true;
                        //while (true)
                        {

                            for (; i < b.Length; ++i)
                            {
                                start: if (changefn)
                                {
                                    //fn += 1;
                                    int[,] nl = new int[8, 8];

                                    image.img[fn].Add(nl);
                                    fn %= image.Fs.Count;
                                    changefn = false;
                                }
                                if (j == 8)
                                {
                                    j = 0;
                                    i++;
                                    //    //f.Read(arr, 0, 1);
                                    //    //if (arr[0] == 0xff)
                                    //    //{
                                    //    //    f.Read(arr2, 0, 1);
                                    //    //    if (arr2[0] == 0xd9)
                                    //    //    {
                                    //    //        return 0;
                                    //    //    }
                                    //    //}
                                }
                                for (; j < 8; ++j)
                                {
                                    if (readnbits-- > 1)
                                    {
                                        value = value << 1 + (((1 << j) & b[i]) == 0 ? 0 : 1);
                                        goto end;
                                    }
                                    if (readnbits-- == 1)
                                    {
                                        value = value << 1 + (((1 << j) & b[i]) == 0 ? 0 : 1);
                                        image.img[fn][image.img[fn].Count - 1][image.Fs[fn].i, image.Fs[fn].j] = value;
                                        value = 0;
                                        goto end;
                                    }

                                    if (((1 << j) & b[i]) == 0)
                                    {
                                        image.H[fn].now = image.H[fn].now.l;
                                    }
                                    else
                                    {
                                        image.H[fn].now = image.H[fn].now.r;
                                    }

                                    if (image.H[fn].now.v != -1)
                                    {
                                        image.Fs[fn].next();
                                        if (image.Fs[fn].pos >= 64)
                                        {
                                            changefn = true;
                                            goto start;
                                        }
                                        if (image.H[fn].now.v == 0)
                                        {
                                            image.img[fn][image.img[fn].Count - 1][image.Fs[fn].i, image.Fs[fn].j] = image.H[fn].now.v;
                                            if (!DC)
                                            {
                                                while (image.Fs[fn].pos < 64)
                                                {
                                                    image.Fs[fn].next();
                                                    image.img[fn][image.img[fn].Count - 1][image.Fs[fn].i, image.Fs[fn].j] = image.H[fn].now.v;
                                                }
                                                changefn = true;
                                                DC = true;
                                                goto start;
                                            }
                                        }
                                        else
                                        {
                                            readnbits = image.H[fn].now.v;
                                        }
                                        image.H[fn].now = image.H[fn].h;
                                        goto end;
                                    }

                                    end: int prostometka = 0;
                                }
                            }
                        }
                        //continue;
                    }
                    //Tresh
                    if (head[0] == 0xff)
                    {
                        f.Read(head, 0, 2);
                        int len = head[0] * 256 + head[1] - 2;
                        comment = new byte[len];
                        f.Read(comment, 0, len);
                        continue;
                    }

                    break;
                }
            }
            catch (Exception ex) { }
            finally
            {
                f?.Close();
            }
            return 0;
        }

        public static int Write(string path, jpg image)
        {
            FileStream f = null;
            try
            {
                f = new FileStream(path, FileMode.Create, FileAccess.Write);
                byte[] head = new byte[2];


                {
                    head = new byte[2] { 0xff, 0xd8 };
                    f.Write(head, 0, 2); //Start header

                    for (int i = 0; i < image.Q.Count; ++i)
                    {
                        head = new byte[2] { 0xff, 0xdb };
                        f.Write(head, 0, 2); //Quantirized table
                        {
                            head = new byte[3];
                            head[0] = (byte)((image.Q[i].Count * image.Q[i].Count + 3) / 256);
                            head[1] = (byte)((image.Q[i].Count * image.Q[i].Count + 3) % 256);
                            head[2] = (byte)((image.onebyte[i] ? 0 : 1) * 16 + image.Qid[i]);
                            f.Write(head, 0, 3);
                            //f.Write(image.Qarr[i], 0, image.Qarr[i].Length);
                            byte[] arr = image.getQ(i);
                            f.Write(arr, 0, arr.Length);

                        }
                    }

                    for (int i = 0; i < image.Farr.Count; ++i)
                    {
                        head = new byte[2] { 0xff, 0xc0 };
                        f.Write(head, 0, 2); //Base coding
                        head[0] = (byte)((image.Farr[i].Length + 2) / 256);
                        head[1] = (byte)((image.Farr[i].Length + 2) % 256);
                        f.Write(head, 0, 2);
                        f.Write(image.Farr[i], 0, image.Farr[i].Length);

                    }

                    for (int i = 0; i < image.Harr.Count; ++i)
                    {
                        head = new byte[2] { 0xff, 0xc4 };
                        f.Write(head, 0, 2); // Haar
                        head[0] = (byte)((image.Harr[i].Length + 2) / 256);
                        head[1] = (byte)((image.Harr[i].Length + 2) % 256);
                        f.Write(head, 0, 2);
                        f.Write(image.Harr[i], 0, image.Harr[i].Length);
                    }

                    head = new byte[2] { 0xff, 0xda };
                    f.Write(head, 0, 2); // Body of image
                    f.Write(image.imgharr, 0, image.imgharr.Length);
                    f.Write(image.imgarr, 0, image.imgarr.Length);

                }
                return 0;
            }
            catch (Exception ex) { }
            finally
            {
                f?.Close();
            }
            return 0;
        }

    }
}
