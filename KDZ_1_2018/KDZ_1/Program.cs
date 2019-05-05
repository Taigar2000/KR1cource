using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace KDZ_1
{
    static class Program
    {
        public class MultiFormContext : ApplicationContext
        {
            private int openForms;
            public MultiFormContext(params Form[] forms)
            {
                openForms = forms.Length;

                foreach (var form in forms)
                {
                    form.FormClosed += (s, args) =>
                    {
                        if (Interlocked.Decrement(ref openForms) == 0)
                            ExitThread();
                    };

                    form.Show();
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //int ji = 1;
            //int jj = -1;
            //int o = 1;
            //int size = 8;
            //List<int[]> l = new List<int[]>();
            //int[,] arr = new int[size, size];
            //for (int i = 0; i < 64; i++)
            //{
            //    ji -= o;
            //    jj += o;

            //    bool lastisrot = false;
            //    if (!lastisrot && jj > size - 1) // Turn right and go down
            //    {
            //        o *= -1;
            //        ji += 2;
            //        jj -= 1;
            //        lastisrot = true;
            //    }
            //    if (!lastisrot && ji < 0) // Turn right and go down
            //    {
            //        o *= -1;
            //        ji += 1;
            //        jj += 0;
            //        lastisrot = true;
            //    }
            //    if (!lastisrot && ji > size - 1) // Turn left and go up
            //    {
            //        o *= -1;
            //        ji -= 1;
            //        jj += 2;
            //        lastisrot = true;
            //    }
            //    if (!lastisrot && jj < 0) // Turn left and go up
            //    {
            //        o *= -1;
            //        ji += 0;
            //        jj += 1;
            //        lastisrot = true;
            //    }
            //    l.Add(new int[] { ji, jj });
            //    arr[ji, jj] = i+1;
                    
            //}
            //return;
            //for developer only
            bool debug = false;
            //Заставляем всех использовать английскую локаль
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //while (true)
            //{
                //try
                //{
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    ProgressBur pb = new ProgressBur(new Fractal());
                    Application.Run(new MultiFormContext(new Form1(pb), pb));
                    //break;
                //}
                //catch (Exception ex)
                //{
                    //if (debug) // For developers only
                    //{
                    //    Console.WriteLine("Произошла непредвиденная ошибка\n" + ex.Message + '\n' + ex.Source + '\n' + ex.StackTrace + '\n' + ex.ToString());
                    //    Console.WriteLine("\nДля выхода из программы нажмите ESC\n Для перезапуска программы - клавишу Enter");
                    //    if (Console.ReadKey(true).Key != ConsoleKey.Escape) continue;
                    //}
                //}
                //if(debug) break;
            //}
        }
    }
}
