using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using JPEG;

namespace KDZ_1
{
    public partial class Form1 : Form
    {
        #region Init

        private float posx, posy, pox, poy;
        
        private string name = "";
        private string message = "";
        private Bitmap bmp = null;
        private Bitmap rawbmp = null;
        private jpg img = new jpg();
        private bool draw_step_by_step = true;
        private bool isdrawing = false;
        double scale = 1;
        ProgressBur pb;
        Timer timer = new Timer();
        bool fenableformwhendrawing = false;
        bool flagmb = false;

        #endregion

        /// <summary>
        /// Конструктор принимающий ссылку на окно шкалы прогресса
        /// </summary>
        /// <param name="pb">Ссылка на окно шкалы прогресса</param>
        internal Form1(ProgressBur pb)
        {
            this.pb = pb;
            this.pb.Visible = false;
            this.pb.Enabled = false;
            DoubleBuffered = true;
            InitializeComponent();

            this.comboBox_type_of_operations.SelectedIndex = 0;
            this.textBox1.Text = "1";
            this.textBox1.Visible = false;
            this.Closed += Form1Closed;
            this.TopMost = overAllWindowsToolStripMenuItem.Checked = false;

            Invalidate();
            Init();
            
            
        }

        #region Image


        #region GUI
        double textBox1ov = 1;
        /// <summary>
        /// Проверка введённого коэффициента на корректность
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double a = 0;
            try
            {
                a = double.Parse(textBox1.Text);
            }
            catch(Exception ex)
            {
                textBox1.Text = "" + textBox1ov;
                DropExWindow("Введите вещественное число больше 0 (через \".\")");
            }
            textBox1ov = a;
        }

        /// <summary>
        /// Обработка выбора операции над изображением
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_type_of_operations_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Visible = false;
            if (((ComboBox)sender).SelectedIndex==1 || ((ComboBox)sender).SelectedIndex == 2)
            {
                this.textBox1.Visible = true;
            }
        }

        /// <summary>
        /// Отображение результата вычислений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.isdrawing) return;
            //Rewrite();
            DrawImage();
        }

        #endregion

        /// <summary>
        /// Перерисовываем изображение на Bitmap
        /// </summary>
        void Rewrite()
        {
            if (this.isdrawing) return;
            try
            {
                if (name == "")
                {
                    loadToolStripMenuItem_Click(null, null);
                }
                this.rawbmp = new Bitmap(name);
                this.bmp = new Bitmap(rawbmp,(int)(rawbmp.Width*scale),(int)(rawbmp.Height*scale));
            }
            catch (ArgumentNullException ex)
            {
                //Исключение выбрасываемое в предыдущих версиях программы
            }
            catch (System.ArgumentException ex)
            {
                //Вывод окна с сообщением об ошибке
                DropExWindow("Слишком большой размер изображения, возможно это связано со слишком большим приближением или удалением \n" + ex.Message);
                Init(false);
            }
            catch (OverflowException ex)
            {
                //Вывод окна с сообщением об ошибке
                DropExWindow("Слишком большая глубина рекурсии \n" + ex.Message);
            }
            catch (Exception ex)
            {
                DropExWindow("\n" + ex.Message);
            }
            Invalidate();
        }

        /// <summary>
        /// Применение операций к закодированному изображению и его вывод на форму
        /// </summary>
        void DrawImage()
        {
            if (name == "")
            {
                DropExWindow("Выберите изображение для изменения");
                loadToolStripMenuItem_Click(null, null);
            }
            if (name == "")
                return;
            JPEGReader.Read(name, img);
            switch (this.comboBox_type_of_operations.SelectedIndex)
            {
                case 0: //"Без изменений",
                    
                    break;
                case 1: //"Скалярное произведение",
                    double a = 0;
                    try
                    {
                        a = double.Parse(textBox1.Text);
                    }
                    catch(Exception ex)
                    {
                        return;
                    }
                    img=img * a;
                    break;
                case 2: //"Скалярное увеличение",
            
                    break;
                case 3: //"Пиксельное увеличение",

                    break;
                case 4: //"Пиксельное произведение"

                    break;

            }
            saveAsToolStripMenuItem_Click(null, null);
            JPEGReader.Write(name, img);
            Rewrite();
            if (f) Init();
        }
        
        #endregion

        /// <summary>
        /// Переопределение перерисовки окна
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (bmp != null)
            {
                e.Graphics.DrawImage(bmp, posx, posy);
            }
            base.OnPaint(e);
        }

        #region Move

        bool f = false;
        /// <summary>
        /// ЛКМ нажата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_fractal_MouseDown(object sender, MouseEventArgs e)
        {
            if (bmp == null) return;
            if (!f)
            {
                pox = e.X;
                poy = e.Y;
                f = true;
                return;
            }
            else
            {
                
            }
        }

        /// <summary>
        /// ЛКМ отпущена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_fractal_MouseUp(object sender, MouseEventArgs e)
        {
            if (bmp == null) return;
            if (f)
            {
                f = false;
            }
        }

        /// <summary>
        /// Перемещение курсора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_fractal_MouseMove(object sender, MouseEventArgs e)
        {
            if (bmp == null) return;
            if (f)
            {
                float dx = e.X - pox, dy = e.Y - poy;
                pox += dx;
                poy += dy;
                posx += dx;
                posy += dy;

                if (posx > this.Width) posx = this.Width;
                if (posx - pictureBox1.Width + bmp.Width < 0) posx = 0 - bmp.Width + pictureBox1.Width;
                if (posy + menuStrip1.Height > this.Height) posy = - menuStrip1.Height + this.Height;
                if (posy - menuStrip1.Height + bmp.Height < 0) posy = menuStrip1.Height - bmp.Height;

                Invalidate();
            }
        }

        /// <summary>
        /// Отцентровать изображение и сбросить масштаб
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (bmp == null) return;
            if (this.isdrawing) return;
            Init();
        }

        /// <summary>
        /// Приближение в конкретной точке
        /// </summary>
        /// <param name="e"></param>
        void ZoomUp(MouseEventArgs e)
        {
            if (this.isdrawing) return;
            pox -= (posx - e.X) - (posx - e.X) * (float)(1.5);
            poy -= (posy - e.Y) - (posy - e.Y) * (float)(1.5);
            posx -= (posx - e.X) - (posx - e.X) * (float)(1.5);
            posy -= (posy - e.Y) - (posy - e.Y) * (float)(1.5);
            this.scale *= (float)1.5;
            //Rewrite();
        }
        
        /// <summary>
        /// Приближение в левом верхнем углу
        /// </summary>
        void ZoomUp()
        {
            if (this.isdrawing) return;
            this.scale *= (float)1.5;
            Rewrite();
        }

        /// <summary>
        /// Удаление в конкретной точке
        /// </summary>
        /// <param name="e"></param>
        void ZoomDown(MouseEventArgs e)
        {
            if (this.isdrawing) return;
            this.scale /= (float)1.5;
            pox -= (posx - e.X) - (posx - e.X) / (float)(1.5);
            poy -= (posy - e.Y) - (posy - e.Y) / (float)(1.5);
            posx -= (posx - e.X) - (posx - e.X) / (float)(1.5);
            posy -= (posy - e.Y) - (posy - e.Y) / (float)(1.5);
            //Rewrite();
        }

        /// <summary>
        /// Удаление в левом вехнем углу экрана
        /// </summary>
        void ZoomDown()
        {
            if (this.isdrawing) return;
            this.scale /= (float)1.5;
            Rewrite();
        }

        /// <summary>
        /// Масштабирование изображения с помощью колёсика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pictureBox_fractal_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.isdrawing) return;
            if (bmp == null) return;
            if (e.Delta > 0)
            {
                ZoomUp(e);
            }
            else
            {
                ZoomDown(e);
            }
            Rewrite();
        }

        /// <summary>
        /// Стартовые значения позиции и размера и если draw, то перерисование фрактала
        /// </summary>
        /// <param name="draw">Перерисовать фрактал?Да:Нет</param>
        private void Init(bool draw=true)
        {
            if (this.isdrawing) return;
            if (bmp == null) return;
            //posx = Frac.xspace - Frac.xsize * Frac.scale / 2 + (this.Width - Frac.xspace) / 2;
            //posy = Frac.yspace - Frac.ysize * Frac.scale / 2 + (this.Height - Frac.yspace * 2) / 2;
            posx = 15;
            posy = 15;
            this.scale = 1;
            if (draw)
            {
                Rewrite();
                Invalidate();
            }
        }

        /// <summary>
        /// Отлов нажатий клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (bmp == null) return;
                if (e.KeyCode == Keys.C)
                {
                    Init();
                }
                if (e.KeyCode == Keys.Q)
                {
                    ZoomUp();
                }
                if (e.KeyCode == Keys.E)
                {
                    ZoomDown();
                }

            }
            catch (Exception ex)
            {
                ApplicationClosingByException(ex);
            }
        }

        #endregion

        #region Save

        /// <summary>
        /// Сохранение изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (name.Length == 0)
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    bmp.Save("" + name, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            catch (NullReferenceException ex)
            {
                DropExWindow("Невозможно сохранить несуществующий оъект\n" + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (Exception ex)
            {
                DropExWindow("" + ex.Message);
            }
        }

        /// <summary>
        /// Сохранение изображения как (старый интерфейс)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    name = FBD.SelectedPath;
                    bmp.Save("" + name, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            catch (NullReferenceException ex)
            {
                DropExWindow("Невозможно сохранить несуществующий оъект\n" + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (Exception ex)
            {
                DropExWindow("" + ex.Message);
            }
        }

        /// <summary>
        /// Сохранение изображения как (новый интерфейс)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //saveAsToolStripMenuItem_Click(sender, e);
            try
            {
                if (bmp == null) throw (new NullReferenceException());
                SaveFileDialog FBD = new SaveFileDialog();
                FBD.Filter = "JPEG files (*.jpg; *.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*";
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    name = FBD.FileName;
                    bmp.Save("" + name, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
            catch (NullReferenceException ex)
            {
                DropExWindow("Невозможно сохранить несуществующий оъект\n" + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                DropExWindow("Невозможно сохранить оъект\n" + ex.Message);
            }
            catch (Exception ex)
            {
                DropExWindow("" + ex.Message);
            }
        }

        #endregion

        #region Load

        /// <summary>
        /// Load data from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "";
            try
            {
                OpenFileDialog FBD = new OpenFileDialog();
                FBD.AddExtension = false;
                if (name.Length > 0) { FBD.FileName = name; }
                FBD.Filter = "JPEG files (*.jpg; *.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*";

                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    bmp = new Bitmap(FBD.FileName);
                    name = FBD.FileName;
                }
            }
            catch (NullReferenceException ex)
            {
                DropExWindow("Невозможно загрузить несуществующий оъект\n" + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                DropExWindow("Невозможно загрузить оъект\n" + ex.Message);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                DropExWindow("Невозможно загрузить оъект\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                DropExWindow(str + " because you give path in wrong format", ex);
            }
            catch (PathTooLongException ex)
            {
                DropExWindow(str + " because given path is too long", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                DropExWindow(str + " because you give path to nonexistent directory", ex);
            }
            catch (FileNotFoundException ex)
            {
                DropExWindow(str + " because you give path to nonexistent file", ex);
            }
            catch (IOException ex)
            {
                DropExWindow(str + " because program has error when reading data from file", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                DropExWindow(str + " because your permissions is insufficient to open this file", ex);
            }
            catch (NotSupportedException ex)
            {
                DropExWindow(str + " because stream does not support invoked functionality", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                DropExWindow(str + " because program take security error", ex);
            }
            catch (Exception ex)
            {
                DropExWindow("" + ex.Message);
            }
        }


        #endregion

        #region New windows

        /// <summary>
        /// Новое окно фрактала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Form1(pb)).Show(this);
        }

        /// <summary>
        /// Новое окно ожидания
        /// </summary>
        private void ProgressBur()
        {
            this.isdrawing = true;
            float Max_length = 100;
            float step = (float)(10000.0) / Max_length;
            this.pb.init();
        }

        #endregion

        /// <summary>
        /// Вывод сообщения об ошибке
        /// </summary>
        /// <param name="s"></param>
        void DropExWindow(string s, Exception e = null)
        {
            if (flagmb) return;
            flagmb = true;
            if (e != null)
            {
                s += "\n" + e.Message;
            }
            if(MessageBox.Show(s) == DialogResult.OK)
            {
                flagmb = false;
            }
        }

                

        /// <summary>
        /// Изменение выбранности пункта меню Поверх остальных окон и статуса Поверх остальных окон основного окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void overAllWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            overAllWindowsToolStripMenuItem.Checked ^= true;
            TopMost = overAllWindowsToolStripMenuItem.Checked;
        }

        

        /// <summary>
        /// Закрытие приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1Closed(object sender, EventArgs e)
        {
            Dispose();
            Application.Exit();
        }

        /// <summary>
        /// Метод для закрытия приложения при возникновении исключения
        /// </summary>
        private void ApplicationClosingByException(Exception ex = null, string s = "")
        {
            Program.isclosedbyex = true;
            if (ex != null)
            {
                Program.exmessage = $"Возникло исключение. Форма будет перещапущена. \r\nДополнительная информация: \r\n{ex.Message}";
            }
            else
            {
                Program.exmessage = s;
            }
            foreach (var i in this.OwnedForms)
            {
                i.Close();
            }
            bool f = MessageBox.Show(Program.exmessage, "Перезапустить приложение?", MessageBoxButtons.YesNo) == DialogResult.Yes;
            this.Close();
            Program.isclosedbyex = f;
        }


    }

}
