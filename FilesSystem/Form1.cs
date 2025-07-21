using System.Drawing.Imaging;

namespace FilesSystem
{
    public partial class Form1 : Form
    {
        List<int> listAscii = new List<int>(); //���� Ascii
        int _reserved = 1; //����������� ��������� �������
        public Form1()
        {
            InitializeComponent();
            /*
             ���� ����������� ������������� � �������� ���� ������ � ascii � ����� � rgb
            string ����� -> int Ascii -> struct RGB(Ascii)
            ��� ������������ �������� ��������� �� �����
             */
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return; //������ ����������
            string filename = saveFileDialog1.FileName;
            label1.Text = filename;
            File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("���� ��������!", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return; //������ ��������
            string filename = openFileDialog1.FileName;
            string text = File.ReadAllText(filename);
            textBox1.Text = text;
            label1.Text = filename;
            MessageBox.Show("���� ������!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "����� ����� ���� ����� | ��� �����";
            label2.Text = "����� ����� ��� ����";
            textBox1.Text = "�������� ���-������ �����...";
        }

        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return; //������ ����������
            string filename = saveFileDialog1.FileName;
            foreach (char symbol in textBox1.Text) //����� ���������?
            {
                listAscii.Add(symbol);
            }
            CreateImage(filename);
            MessageBox.Show("���� ��������!", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateImage(string filename) //����������� �������������
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                //-----��������� ������� ��������� ����������-------
                while(bmp.Width / listAscii.Count > 255 * _reserved || listAscii.Count > 255 * _reserved)
                {
                    _reserved++;
                }
                //-----������ ��������� ����������-------------
                /*
                 * ������ ��������� ������� �������� ��� (������ �������, ���������� ��������, 0)
                 * ����� �� ���� ���������� ������� �����, ���������� ������� - ���� 255, ���� �������
                 * ����� �����������, ��� ������� �� ����� ������ 0
                 */
                for (int step = 0; step < _reserved; step++)
                {
                    Brush info = new SolidBrush( Color.FromArgb(Math.Min(Math.Max(0, (bmp.Width / listAscii.Count) - (255 * step)), 255), Math.Min(Math.Max(0, listAscii.Count - (255 * step)), 255), 0 ));
                    graphics.FillRectangle(info, step, 0, _reserved, bmp.Height);
                }
                //------���� ����������-----
                /*
                 * ������ ������� ��� (�������������� ������, ���������� ������, ������� ������)
                 * ������ 3 ������� ����������� �� �� ������������ ����������
                 * ������� � ����������� �������� = ������ / ���������� ��������
                 */
                for (int i = 0; i < listAscii.Count; i++) //��������� ����� �� ������ picbox
                {
                    Brush brush = new SolidBrush(Color.FromArgb(listAscii[Math.Max(0, i - 2)], listAscii[Math.Max(0, i - 1)], listAscii[i]));
                    graphics.FillRectangle(brush, (bmp.Width / listAscii.Count) * i+_reserved, 0, bmp.Width / listAscii.Count, bmp.Height);
                }
            }
            pictureBox1.Image = bmp;
            bmp.Save(filename, ImageFormat.Bmp);
        }

        private void CreateText(Bitmap bmp, int widthRow, int Rows) //����������� ������������� - ��������
        {
            string text = "";
            char R; char G; char B;
            int c = 0;
            for (int i = _reserved; i < Rows* widthRow; i+=widthRow) //���� �� ��������
            {
                R = (char)bmp.GetPixel(i, 0).R; //������� ������� � �����
                G = (char)bmp.GetPixel(i, 0).G;
                B = (char)bmp.GetPixel(i, 0).B;
                if (c < 1) //������� ������ ���� ��������
                {
                    text += (R.ToString());
                    c++;
                }
                else
                {
                    text += (B.ToString()); //������ ������ ��������������� ��������
                }
                
            }
            textBox1.Text = text;
        }

        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return; //������ ��������
            string filename = openFileDialog1.FileName;
            Bitmap bmp = new Bitmap(filename);
            pictureBox1.Image = bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            int R = 0; int G = 0; int B = 0;
            for (int step = 0; step < _reserved; step++) //���������� ��������� ����������
            {
                R += bmp.GetPixel(step, 0).R; //������
                G += bmp.GetPixel(step, 0).G; //����������
                B += bmp.GetPixel(step, 0).B;
            }
            CreateText(bmp, R, G);

        }
    }
}
