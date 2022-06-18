using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Мутан
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";//фильтры 
        }
        OpenFileDialog openFileDialog1;
        #region Параметры
        double[] listArray;
        double[] StatListArray;
        List<double> GroupListArray;
        double min;
        double max;
        double step_H;
        double R;
        int K;
        double D;
        double D2;
        double X_Sr;
        Double S;
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private double[] ReadArray(string adress)
        {
            string str = "";// not null
            List<double> list = new List<double>();
            using (StreamReader Reader = new StreamReader(adress, Encoding.Default))
            {
                while (Reader.Peek() != -1) // построчное чтение
                {
                    str = Reader.ReadLine();
                    list.Add(Convert.ToDouble(str));
                }
            }
            return list.ToArray();
        }
        static void quickSort(double[] a, int l, int r)
        {
            double temp;
            double x = a[l + (r - l) / 2];
            //запись эквивалентна (l+r)/2, 
            //но не вызввает переполнения на больших данных
            int i = l;
            int j = r;
            //код в while обычно выносят в процедуру particle
            while (i <= j) // определение опорного элемента для сравнения
            {
                while (a[i] < x) i++;
                while (a[j] > x) j--;
                if (i <= j) //если текущий <= то обмен
                {
                    temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;//обмен
                    i++;
                    j--;
                }
            }
            if (i < r)
                quickSort(a, i, r);//рекурсивный вызов
            if (l < j)
                quickSort(a, l, j);//рекурсивный вызов
        }
        void ShowList()
        {
            listBox1.Items.Clear();
            foreach (double el in listArray)
                listBox1.Items.Add(el);
        }
        void ShowStatList()
        {
            listBox2.Items.Clear();
            double t = 0;
            List<double> tmpList = new List<double>();
            for (int i = 0; i < K; i++)
            {
                if (i == 0)
                    t = min - D2;
                if (i == (K-1))
                    t = max + D2;
                listBox2.Items.Add(t.ToString());
                tmpList.Add(Convert.ToDouble(listBox2.Items[listBox2.Items.Count -1]));
                t += step_H;
            }
            StatListArray = tmpList.ToArray();
        }
        double GetMin(double[] Array)
        {
            double min = Array[0];
            int minIndex = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                if (min > Array[i])
                {
                    min = Array[i];
                    minIndex = i;
                }
            }
            return Array[minIndex];
        }
        double GetMax(double[] Array)
        {
            double max = Array[0];
            int minIndex = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                if (max < Array[i])
                {
                    max = Array[i];
                    minIndex = i;
                }
            }
            return Array[minIndex];
        }
        void TextAddToLabel(Label L)
        {
            string s = L.Text;
        }
        void Show_Params__()
        {
            label2Min.Text = "Min = " + min;
            label3Max.Text = "Max = " + max;
            label1R.Text = "R = " + max + "-" + min + "=" + R;
            label1K.Text = "K = 1 + 3.222 * Lg(" + (listArray.Length) + ")=" + K;
            label1Шаг.Text = "Шаг h = " + R + "/" + K + "=" + step_H;
            label3D.Text = "d = " + step_H + "*" + K + " - "+R+" = " + D;
            label3d2.Text = "d/2 = " + D + "/2 = " + D2;
        }
        #region CalcParams
        void Calc_X̅()//расчет среднего значения
        {
            textBox1.Text = "";
            textBox1.Text = "X̅ = 1/" + (listArray.Length).ToString() + " * (";
            Double sum = 0;
            foreach (Double el in listArray)
            {
                textBox1.Text += el.ToString() + " + ";
                sum += el;
            }
            textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 3);//удаление лишнего плюса
            textBox1.Text += ") = " + String.Format("{0:0.####}", sum / (listArray.Length));
            X_Sr = Convert.ToDouble(String.Format("{0:0.####}", sum / (listArray.Length)));
        }
        void CalcStep()
        {
            step_H = Math.Round(R / K, 1);
        }
        void CalcR()
        {
            R = max - min;
        }
        void CalcK()
        {
            K = Convert.ToInt32(Math.Ceiling(1 + 3.322 * Math.Log10(listArray.Length)));
        }
        void CalcD()
        {
            D = Math.Round(step_H * K - R, 1);
        }
        void CalcD2()
        {
            D2 = Math.Round(D / 2,1);
        }
        void Calc_S()
        {
            S = 0;
            textBox1.Text += "\r\nВыборочное среднее квадратическое\r\n";
            textBox1.Text += "S^2 = 1/"+listArray.Length.ToString() + " = ∑(Xi-X̅)²ni = \r\n";
            for (int i = 0; i < Xi.Length; i++)
            {
                string ssqs = Ni_N[i].Split('/')[0];// количество совпадений
                textBox1.Text += "("+ Xi[i] + "-" + X_Sr.ToString()+ ")²"+ ssqs+"+";
                S += Math.Pow((Convert.ToDouble(Xi[i]) - X_Sr),2)*Convert.ToDouble(ssqs);
                //MessageBox.Show((Convert.ToDouble(Xi[i]) - X_Sr).ToString());
            }
            textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length-1);
            textBox1.Text += " = " + String.Format("{0:0.####}", S/(listArray.Length));
            textBox1.Text += "\r\nS = " + String.Format("{0:0.####}",Math.Sqrt(S));
            S = Convert.ToDouble(String.Format("{0:0.####}", Math.Sqrt(S)));
        }
        void Calc_Desp_to_grouplist()
        {
            double summ = 0;
            double sr = 0;
            textBox1.Text += "\r\nВыборочное среднее по группированому ряду\r\n X = (";
            double[] RealGroupListArray = GroupListArray.ToArray();
            for (int i = 0; i < Ni_N.Length; i++)
            {
                summ += RealGroupListArray[i] * Convert.ToDouble(Ni_N[i].Split('/')[0]);
                textBox1.Text += " (" + String.Format("{0:0.####}", RealGroupListArray[i]) + " * "+ Ni_N[i].Split('/')[0] + ") +";
            }
            textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 3);
            sr = Convert.ToDouble(String.Format("{0:0.####}", summ /= listArray.Length));
            textBox1.Text += ") / " + listArray.Length.ToString() + " = " + String.Format("{0:0.####}", sr);
            textBox1.Text += "\r\nИсправленная дисперсия по группированому ряду\r\n S^2 = (";
            summ = 0;
            for (int i = 0; i < Ni_N.Length; i++)
            {
                summ += Math.Pow(RealGroupListArray[i] - sr,2)* Convert.ToInt32(Ni_N[i].Split('/')[0]);
                textBox1.Text += "(" + String.Format("{0:0.####}", RealGroupListArray[i]) + " - " + sr.ToString() + ")^2 * ";
                textBox1.Text += Ni_N[i].Split('/')[0] + " + ";
            }
            textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 3);
            sr = Convert.ToDouble(String.Format("{0:0.####}", summ /= (listArray.Length - 1)));
            textBox1.Text += ") / " + (listArray.Length-1).ToString() + " = " + String.Format("{0:0.####}", sr);
        }
        void Sigma_Check()
        {
            textBox1.Text += "\r\nПравило трех сигм\r\n";
            //не не буду
        }
        #endregion
        #region расчет и построение таблицы
        int FindSimilar(double dStart, double dStop)// подсчет совпадений
        {
            int res = 0;
            for (int i = 0; i < listArray.Length; i++)
                if (listArray[i] <= dStop && listArray[i] >= dStart)
                    res++;
            return res;
        }
        Label[,] UltraSAS;//матрица лейблов
        void SetLabelRow (int i, string TextTo) // установка свойств для ряда//вторая задача передача параметров графика (в регионах)
        {
            string[] ToLabelSeparate = TextTo.Split(' ');
            for (int j = 0; j < 6; j++)
            {
                UltraSAS[i, j].Text = ToLabelSeparate[j];
                UltraSAS[i, j].AutoSize = true;
                tableLayoutPanel1.Controls.Add(UltraSAS[i, j], j, i);
                //MessageBox.Show(j.ToString()+" "+ UltraSAS[i, j].Text);//3 1/34 // 4 ...
                #region передача параметров графика
                if (j == 1)
                    Ai[i] = ToLabelSeparate[j];
                if (j == 5)
                {
                    GroupListArray.Add(Convert.ToDouble(ToLabelSeparate[j]));
                    Xi[i] = ToLabelSeparate[j];
                }
                if (j == 3)
                    Ni_N[i] = ToLabelSeparate[j];
                if (j == 4)
                    Ni_Nh[i] = ToLabelSeparate[j];
                #endregion
            }
        }
        #region Data
        //массивы для передачи в форму с графиком
        string[] Xi;
        string[] Ni_N;
        string[] Ni_Nh;
        string[] Ai;
        //
        #endregion
        void BuildTab() // построение таблицы
        {
            double t=0;
            double Xi=0;
            int SimilarCount;
            string TextPrepearing = "";
            tableLayoutPanel1.RowCount = 1;
            #region инициализация передатчиков
            this.Xi = new string[K];
            Ni_N = new string[K];
            Ni_Nh = new string[K];
            Ai = new string[K];
            #endregion
            //нужна матрица лейблов
            int Table_size_Y = 0;
            tableLayoutPanel1.Size = new System.Drawing.Size(441, 0);
            UltraSAS = new Label[K, 6];//матрица лейблов в таблицу
            GroupListArray = new List<double>();
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //MessageBox.Show("asdsad" + i);
                    UltraSAS[i, j] = new Label();//
                    UltraSAS[i, j].Text = "asdasd";
                    UltraSAS[i, j].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                } // иннициализация, установка свойств
            }
            for (int i = 0; i < K; i++)
            {
                if (i==0)
                {
                    t = min - D2;
                    SimilarCount = FindSimilar(t, t + step_H);
                    TextPrepearing = ((i+1).ToString() + " [" + t.ToString() + ";" + (t+step_H).ToString() + ") " + SimilarCount + " " + SimilarCount + "/" + (listArray.Length).ToString());
                    Xi = (t + t + step_H) / 2;
                    t += step_H;
                    TextPrepearing += " " + String.Format("{0:0.####}", SimilarCount / ((listArray.Length) * step_H));
                    TextPrepearing += " " + String.Format("{0:0.####}", Xi);
                    
                    //
                    //tableLayoutPanel1.RowCount++; первый раз пропускаем тк row count = 1
                    Table_size_Y += 40;//растягивание под новую строку
                    tableLayoutPanel1.Size = new System.Drawing.Size(462, Table_size_Y);
                    tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
                    //tableLayoutPanel1.Controls.Add(saveButton, 2, 2);
                    SetLabelRow(i, TextPrepearing);
                    continue;
                }
                if (i == (K-1))
                {
                    SimilarCount = FindSimilar(t, t + step_H+D2);
                    TextPrepearing = ((i+1).ToString() + " [" + t.ToString() + ";" + (max+D2).ToString() + ") " + SimilarCount + " "+ SimilarCount + "/" + (listArray.Length).ToString());
                    TextPrepearing += " " + String.Format("{0:0.####}", SimilarCount / ((listArray.Length) * step_H));
                    Xi = (t + t + step_H) / 2;
                    TextPrepearing += " " + String.Format("{0:0.####}", Xi);
                    
                    tableLayoutPanel1.RowCount++;
                    Table_size_Y += 40;//растягивание под новую строку
                    tableLayoutPanel1.Size = new System.Drawing.Size(462, Table_size_Y);
                    tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
                    SetLabelRow(i, TextPrepearing);
                    continue;
                }
                SimilarCount = FindSimilar(t, t+step_H);
                TextPrepearing = ((i).ToString() + " [" + t.ToString() + ";" + (t + step_H).ToString() + ") " +SimilarCount +" "+ SimilarCount + "/" + (listArray.Length).ToString());
                Xi = (t + t + step_H) / 2;
                t += step_H;
                //
                TextPrepearing += " " + String.Format("{0:0.####}", SimilarCount / ((listArray.Length)*step_H));
                TextPrepearing += " " + String.Format("{0:0.####}",  Xi);
                //
                
                tableLayoutPanel1.RowCount++;
                Table_size_Y += 40;//растягивание под новую строку
                tableLayoutPanel1.Size = new System.Drawing.Size(462, Table_size_Y);
                tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
                SetLabelRow(i, TextPrepearing);
            }
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            try
            {
                string filename = openFileDialog1.FileName;
                listArray = ReadArray(filename);
                quickSort(listArray, 0, listArray.Length - 1);
                ShowList();
                min = GetMin(listArray);
                max = GetMax(listArray);
                CalcK();
                CalcR();
                CalcStep();
                CalcD();
                CalcD2();
                Show_Params__();
                ShowStatList();
                BuildTab();
                Calc_X̅();
                Calc_S();
                Calc_Desp_to_grouplist();
                button2.Enabled = true;
                button3.Enabled = true;
            } catch(Exception exe)
            { MessageBox.Show(exe.Message+"Проверьте правильность файла", "Ошибка"); }
        }
        bool NotACastil = true; // на случай неправльного расставления Label_X
        private void button2_Click(object sender, EventArgs e)
        {
            if (NotACastil) // тот самый не костыль
            {
                Form2 newForm1 = new Form2(K, Ni_N, Ni_Nh, Xi, Ai);//передача параметров для графика
                newForm1.Show();
                NotACastil = false;
            }
            else
            {
                Form2 newForm1 = new Form2(K, Ni_N, Ni_Nh, Xi, Ai);//передача параметров для графика
                newForm1.Visible = false;
                newForm1.Show();
                newForm1.Close();
                NotACastil = true;
                button2_Click(sender, e);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form3 newForm1 = new Form3(Ni_N, Xi, listArray.Length);//передача параметров для графика
            newForm1.Visible = false;
            newForm1.Show();
            newForm1.Close();
            newForm1 = new Form3(Ni_N, Xi, listArray.Length);//передача параметров для графика
            newForm1.Show();
        }
    }
}
