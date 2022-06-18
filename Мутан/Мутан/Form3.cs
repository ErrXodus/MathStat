using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Мутан
{
    public partial class Form3 : Form
    {
        public Form3(string[] Ni_Nt,string[] Xit,int Nt)
        {
            /*
             нужны Ni/N и Xi
             */
            Ni_N = Ni_Nt;
            Xi = Xit;
            N = Nt;
            InitializeComponent();
        }
        string[] Ni_N;
        string[] Xi;
        int N;
        Label[,] Labels;
        double sum = 0;
        List<Double> Y_List;
        string SumText(string t) // получение 
        {
            string[] arr = t.Split('/');
            sum += Convert.ToInt32(t[0])-48;
            double tmp = Convert.ToDouble(String.Format("{0:0.####}", sum / N));
            //MessageBox.Show(tmp.ToString());
            Y_List.Add(tmp);
            return sum.ToString() + '/' + arr[1] + " = " + tmp.ToString();
        }
        #region DrawPar
        Graphics graphics1;
        Color cl1 = Color.Red;
        Bitmap bitmapFasten = new Bitmap(780, 550);
        Label[] Labels_X;
        #endregion
        private void Form3_Load(object sender, EventArgs e)
        {
            Y_List = new List<double>();
            #region Заполнение таблицы
            //по 50 для каждой новой строки
            tableLayoutPanel1.Size = new Size(550, 0);
            tableLayoutPanel1.RowCount = 1;
            Labels = new Label[Ni_N.Length+1, 3];
            int Y = 0;
            for (int i = 0; i < Ni_N.Length+1; i++)
            {
                Y += 45;
                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.Size = new Size(550, Y);
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
                for (int j = 0; j < 3; j++)
                {
                    Labels[i, j] = new Label();
                    if (i == Ni_N.Length) // Последний случай
                    {
                        if (j == 0)
                            Labels[i, j].Text = "1";
                        if (j == 1)
                            Labels[i, j].Text = "при";
                        if (j == 2)
                            Labels[i, j].Text = "X ⩾ " + Xi[i - 1];
                    }
                    else
                    {
                        if (j == 0)
                            Labels[i, j].Text = SumText(Ni_N[i]);
                        if (j == 1)
                            Labels[i, j].Text = "при";
                        if (j == 2)
                            if (i == 0)
                                Labels[i, j].Text = "X < " + Xi[i]; // первый 
                            else
                                Labels[i, j].Text = Xi[i - 1] + " ⩽ X < " + Xi[i];//обычный 
                    }
                    Labels[i, j].Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
                    Labels[i, j].AutoSize = true;
                    tableLayoutPanel1.Controls.Add(Labels[i,j],j,i);
                } // иннициализация, установка свойств
            }
            #endregion
            Double[] Arr_Y = Y_List.ToArray();
            //780 550
            //666 590
            /*
             переписыва
             */
            //1355 590
            #region Рисование графика, расставление лейблов
            pictureBox1.Image = bitmapFasten;
            Pen pen = new Pen(cl1);           // перо с выбранным цветом
            pen.Width = 5;
            graphics1 = Graphics.FromImage(bitmapFasten);
            float step_X, step_Y,step_XLabel;
            step_X = 780 / Xi.Length;
            step_Y = 550 / Arr_Y.Count();
            step_XLabel = 690 / Xi.Length;
            Labels_X = new Label[Xi.Length];
            string[] Xi_Rev = Xi;
            Array.Reverse(Xi_Rev);
            for (int i = 0; i < Xi.Length; i++)
            {
                Labels_X[i] = new Label();
                Labels_X[i].Text = Xi_Rev[i];
                Labels_X[i].Location = new Point(1355 - Convert.ToInt32(i*step_XLabel),590);
                Controls.Add(Labels_X[i]);
                graphics1.DrawLine(pen, 0, (550-(i*step_Y)), i*step_X, (550 - (i * step_Y)));
            }
            #endregion
        }
    }
}
