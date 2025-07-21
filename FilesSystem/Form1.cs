using System.Drawing.Imaging;

namespace FilesSystem
{
    public partial class Form1 : Form
    {
        List<int> listAscii = new List<int>(); //коды Ascii
        int _reserved = 1; //сохраненные служебные пиксели
        public Form1()
        {
            InitializeComponent();
            /*
             Идея визуального представления в переводе англ текста в ascii и затем в rgb
            string Текст -> int Ascii -> struct RGB(Ascii)
            для демонстрации рисуются полосками на форме
             */
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return; //отмена сохранения
            string filename = saveFileDialog1.FileName;
            label1.Text = filename;
            File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("Файл сохранен!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return; //отмена загрузки
            string filename = openFileDialog1.FileName;
            string text = File.ReadAllText(filename);
            textBox1.Text = text;
            label1.Text = filename;
            MessageBox.Show("Файл открыт!", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Здесь будет путь файла | Ваш текст";
            label2.Text = "Здесь будет ваш шифр";
            textBox1.Text = "Напишите что-нибудь здесь...";
        }

        private void сохранитьШифрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return; //отмена сохранения
            string filename = saveFileDialog1.FileName;
            foreach (char symbol in textBox1.Text) //можно упростить?
            {
                listAscii.Add(symbol);
            }
            CreateImage(filename);
            MessageBox.Show("Шифр сохранен!", "Шифр", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateImage(string filename) //визуального представления
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                //-----изменение размера служебной информации-------
                while(bmp.Width / listAscii.Count > 255 * _reserved || listAscii.Count > 255 * _reserved)
                {
                    _reserved++;
                }
                //-----запись служебной информации-------------
                /*
                 * Каждый служебный пиксель хранится как (ширина столбца, количество столбцов, 0)
                 * Чтобы не было превышения размера байта, информация делится - либо 255, либо остаток
                 * Также учитывается, что остаток не будет меньше 0
                 */
                for (int step = 0; step < _reserved; step++)
                {
                    Brush info = new SolidBrush( Color.FromArgb(Math.Min(Math.Max(0, (bmp.Width / listAscii.Count) - (255 * step)), 255), Math.Min(Math.Max(0, listAscii.Count - (255 * step)), 255), 0 ));
                    graphics.FillRectangle(info, step, 0, _reserved, bmp.Height);
                }
                //------сама информация-----
                /*
                 * Каждый пиксель это (предпредыдущий символ, предыдущий символ, текущий символ)
                 * Первые 3 символа повторяются из за особенностей реализации
                 * Полоски с информацией размером = ширина / количеству символов
                 */
                for (int i = 0; i < listAscii.Count; i++) //рисование полос по ширине picbox
                {
                    Brush brush = new SolidBrush(Color.FromArgb(listAscii[Math.Max(0, i - 2)], listAscii[Math.Max(0, i - 1)], listAscii[i]));
                    graphics.FillRectangle(brush, (bmp.Width / listAscii.Count) * i+_reserved, 0, bmp.Width / listAscii.Count, bmp.Height);
                }
            }
            pictureBox1.Image = bmp;
            bmp.Save(filename, ImageFormat.Bmp);
        }

        private void CreateText(Bitmap bmp, int widthRow, int Rows) //визуального представления - обратное
        {
            string text = "";
            char R; char G; char B;
            int c = 0;
            for (int i = _reserved; i < Rows* widthRow; i+=widthRow) //цикл по полоскам
            {
                R = (char)bmp.GetPixel(i, 0).R; //перевод пикселя в буквы
                G = (char)bmp.GetPixel(i, 0).G;
                B = (char)bmp.GetPixel(i, 0).B;
                if (c < 1) //пропуск первых трех повторов
                {
                    text += (R.ToString());
                    c++;
                }
                else
                {
                    text += (B.ToString()); //запись только неповторяющихся символов
                }
                
            }
            textBox1.Text = text;
        }

        private void загрузитьШифрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return; //отмена загрузки
            string filename = openFileDialog1.FileName;
            Bitmap bmp = new Bitmap(filename);
            pictureBox1.Image = bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            int R = 0; int G = 0; int B = 0;
            for (int step = 0; step < _reserved; step++) //считывание служебной информации
            {
                R += bmp.GetPixel(step, 0).R; //ширина
                G += bmp.GetPixel(step, 0).G; //количество
                B += bmp.GetPixel(step, 0).B;
            }
            CreateText(bmp, R, G);

        }
    }
}
