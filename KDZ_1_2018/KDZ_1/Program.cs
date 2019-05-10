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
        public static bool isclosedbyex = true; // Приложение закрылось из-за ошибки или по желанию пользователя
        public static string exmessage = ""; // Сообщение содержащее информацию об ошибке из-за которой закрылась форма


        public class MultiFormContext : ApplicationContext
        {
            private int openForms;
            public MultiFormContext(params Form[] forms)
            {
                openForms = forms.Length;
                bool f = true;
                foreach (var form in forms)
                {
                    form.FormClosed += (s, args) =>
                    {
                        if (Interlocked.Decrement(ref openForms) == 0)
                            ExitThread();
                    };

                    if(!f)form.Show(forms[0]);
                    else form.Show();
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //All must using english (US) locale
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (isclosedbyex)
            {

                //try
                {
                    isclosedbyex = false;
                    ProgressBur pb = new ProgressBur();
                    Application.Run(new MultiFormContext(new Form1(pb)/*, pb*/));
                    break;
                }
                //catch (Exception ex) { }
                if (!isclosedbyex)
                {
                    Application.Exit(); //Close this application totally
                }
            }
        }
    }
}
