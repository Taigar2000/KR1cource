using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KDZ_1;

namespace KDZ_1
{
    /// <summary>
    /// Класс для вывода шкалы прогресса построения фрактала
    /// </summary>
    public partial class ProgressBur : Form
    {
        /// <summary>
        /// ссылка на фрактал, для получения информации об отрисовке фрактала
        /// </summary>
        private int max_length;
        /// <summary>
        /// bool переменная, обозначающая нужно ли завершать выполнение
        /// </summary>
        public bool isexit = false;
        /// <summary>
        /// таймер для обновления шкалы прогресса отрисовки фрактала
        /// </summary>
        public Timer timer = new Timer();
        /// <summary>
        /// Шкала прогресса
        /// </summary>
        public System.Windows.Forms.ProgressBar progressBar1 = new System.Windows.Forms.ProgressBar();

        /// <summary>
        /// Конструктор устанавливающий ссылку на фрактал
        /// </summary>
        /// <param name="f">Ссылка на фрактал</param>
        public ProgressBur()
        {
            this.Closed += ProgressBurClosed;
            InitializeComponent();
            init();
            this.Hide();
            this.Visible = false;
            timer.Interval = 10; //интервал между срабатываниями 10 миллисекунд
            timer.Tick += new EventHandler(Draw);
        }
        
        /// <summary>
        /// Получение ссылки на фрактал
        /// </summary>
        /// <param name="f"></param>
        public void gfrac(int maxl)
        {
            this.max_length = maxl;
            init();
        }

        /// <summary>
        /// Инициализация шкалы прогресса
        /// </summary>
        public void init()
        {
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = this.max_length;
        }
        
        /// <summary>
        /// Обновление шкалы прогресса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Draw(object sender, EventArgs e)
        {
            this.Enabled = true;
            Invalidate();
        }

        /// <summary>
        /// Переопределение метода закрытия окна шкалы прогресса, для отключения возможности его закрыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProgressBurClosed(object sender, EventArgs e)
        {
            //Do nothing (sovsem nothing)
            return;
        }

        /// <summary>
        /// Отлов события Закрытие окна шкалы прогресса и прекращение отрисовки или выход из программы
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!isexit)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Отлов нажатий клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.OnFormClosing(new FormClosingEventArgs(new CloseReason(), false));
            }
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
            this.Owner.Close();
            this.Close();
            Program.isclosedbyex = f;
        }

    }
}
