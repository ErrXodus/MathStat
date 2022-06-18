using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Мутан
{
    public partial class Form2 : Form
    {
        public Form2(int Kt, string[] Ni_Nt, string[] Ni_Nht,string[] Xit,string[]Ait)
        {
            InitializeComponent();
            K = Kt;
            Ni_N = Ni_Nt;
            Ni_Nh = Ni_Nht;
            Xi = Xit;
            Ai = Ait;
        }
        #region params
        int K;
        string[] Ni_N; //текстовое соотношение
        string[] Ni_Nh; // уже число
        string[] Xi;
        string[] Ai;
        #endregion
        #region DrawingParams
        Graphics graphics1;
        Graphics graphics2;
        Color cl1 = Color.Red;
        Color cl2 = Color.Blue;
        Bitmap bitmapFasten = new Bitmap(780, 550);
        Bitmap bitmapFasten2 = new Bitmap(780, 550);
        struct Range : IComparable //диапазон
        {
            public int CompareTo(object obj)
            {
                if (obj is Range arr)
                {
                    return this.val.CompareTo(arr.val);
                }
                else
                {
                    throw new ArgumentException("object is not myArray");
                }
            }
            public string Name;
            public double val;
            public int num;
        };
        Range[] Sorted_Range_Ni;
        Range[] Sorted_Range_Nih;
        Range[] Real_Range_Ni;//постоянные значения для рисования
        Range[] Real_Range_Nih;//постоянные значения
        int[] pos_X;
        int[] pos_Y;
        Label[] Label_X;
        Label[] Label_Y;
        //
        #endregion
        void BuildUpData() // построение всей необходимой информации
        {
            Sorted_Range_Ni = DeleteSimilar(SetRangeStruct(Ni_N));
            Sorted_Range_Nih = DeleteSimilar(SetRangeStruct(Ni_Nh));
            Array.Sort(Sorted_Range_Ni);
            Array.Sort(Sorted_Range_Nih);
            pos_X = PointAllocator(K, 780); // построение областей
            pos_Y = PointAllocator(Sorted_Range_Ni.Length, 550); // построение областей
            //
            Real_Range_Ni = SetRangeStruct(Ni_N);
            Real_Range_Nih = SetRangeStruct(Ni_Nh);
            for (int i = 0; i < Real_Range_Ni.Length; i++)
                Real_Range_Ni[i] = SetNumOfStruct(Sorted_Range_Ni,Real_Range_Ni[i]); // присвоение каждому шагового значения
            for (int i = 0; i < Real_Range_Nih.Length; i++)
                Real_Range_Nih[i] = SetNumOfStruct(Sorted_Range_Nih, Real_Range_Nih[i]); // присвоение каждому шагового значения                                                           //780, 550
            #region построение лейблов для 1 графика
            int AreaX = 766; // + 144 // 
            int AreaY = 540; // +20 // 
            double step_X_Label = AreaX / K;
            double step_Y_Label = AreaY / Sorted_Range_Ni.Length; // пересчет не нужен так как len равны
            Label_X = new Label[K];
            Label_Y = new Label[Sorted_Range_Ni.Length];
            for (int i = 0; i < Sorted_Range_Ni.Length; i++) //циклы установки лейблов
            {
                Label_Y[i] = SetLabel(Sorted_Range_Ni[i].Name, new Point(65, Convert.ToInt32(560 - (i * step_Y_Label))));
                Controls.Add(Label_Y[i]);
            }
            Array.Reverse(Xi);
            for (int i = 0; i < K; i++)
            {
                Label_X[i] = SetLabel(Xi[i], new Point(Convert.ToInt32(838-(i * step_X_Label)), 620));
                Controls.Add(Label_X[i]);
            }
            #endregion
            pictureBox2.Location = new Point(144, 650);// перенос второго пикчербокса
            #region Построение лейблов для 2 графика
            for (int i = 0; i < Sorted_Range_Nih.Length; i++) //циклы установки лейблов
            {
                Label_Y[i] = SetLabel(Sorted_Range_Nih[i].Name, new Point(65, Convert.ToInt32(1190 - (i * step_Y_Label))));
                Controls.Add(Label_Y[i]);
            }
            Array.Reverse(Ai);
            for (int i = 0; i < K; i++)
            {
                Label_X[i] = SetLabel(Ai[i], new Point(Convert.ToInt32(838 - (i * step_X_Label)), 1240));
                Controls.Add(Label_X[i]);
            }
            #endregion
            #region Помойные мысли
            /*
            очистили повторения
            разделить поле на N не повторяющихся частей (получаем шаг)
            соритировать по возростанию для присваивания конкретных координат затем
            (номер в последовательности) * шаг
            параллельно в структуре будет и строковое значение для подписи через лейблы внизу и слева
            //
            после того как есть шаги X и Y нужно проставить точки слева направо расставляя по Y
            //
            есть шаговое значения в структурах Real_Range_Ni и Real_Range_Nih
            теперь проставление точек и рисование
            //
            pos_X будет постоянно увеличиватся а pos_Y по num * step или номеру в последовательности (Range.num)
             */
            //
            /*
            65 560
            65 20
            для x
            144 620
            910 620
             */
            //
            #endregion
        }
        Label SetLabel(string text,Point p) // установка лейбла
        {
            Label l = new Label();
            if (text.Contains(";"))
            {
                string[] spText = text.Split(';');
                l.Text = spText[0] + "\r\n" + spText[1];
            }
            else
            l.Text = text;
            l.Location = p;
            return l;
        }
        int [] PointAllocator(int len,int area_size) // диапазон координат
        {
            step = area_size / len;
            int[] arr = new int[len];
            for (int i = 0; i < len; i++)
                arr[i] = i * step;
            return arr;
        }
        Range SetNumOfStruct(Range[] arr,Range R)//опираясь на сортированные структуры определяется номер высоты
        {//из диапазона
            for (int i = 0; i < arr.Length; i++)
                if (R.val == arr[i].val)
                {
                    R.num = i;
                    //MessageBox.Show(R.num.ToString());
                    return R;
                }
            return R;
        }
        Range[] SetRangeStruct(string[] arr2) // уствновка массива структур
        {
            Range[] t = new Range[arr2.Length];
            for (int i = 0; i < arr2.Length; i++)
            {
                t[i].Name = arr2[i];
                if (arr2[i].Contains("/"))//проверка число это или соотношение
                {
                    string[] tmp = arr2[i].Split('/');
                    t[i].val = Convert.ToDouble(tmp[0]) / Convert.ToDouble(tmp[1]);
                }
                else
                    t[i].val = Convert.ToDouble(arr2[i]);
            }
            return t;
        }
        Range[] DeleteSimilar(Range[] arr) //удаление повторяющихся
        {
            var hashset = new HashSet<Range>(); //Создаём объект типа множество.
            foreach (var x in arr) // Проходимся по массиву и добавляем только те элементы, которых во множестве ещё нет
                if (!hashset.Contains(x))
                    hashset.Add(x);
            Array.Resize(ref arr, hashset.Count); // Изменяем размерность массива на необходимую
            arr = hashset.ToArray(); // Перебрасываем элементы из множества обратно в массив
            return arr;
        }
        int step;
        private void Form2_Load(object sender, EventArgs e) // рисование
        {
            pictureBox1.Image = bitmapFasten;
            Pen pen = new Pen(cl1);           // перо с выбранным цветом
            pen.Width = 5;
            graphics1 = Graphics.FromImage(bitmapFasten);
            BuildUpData();//построение всей необходимой информации
            //foreach (int el in pos_Y)
            //    MessageBox.Show(el.ToString());
            int Y = 0;
            float prevX=0, prevY=0;
            bool b = false;
            for (int i = 0; i < pos_X.Length; i++)
            {
                Y = pos_Y[Real_Range_Ni[i].num]; // дает 0
                //MessageBox.Show(Y.ToString()+" "+ pos_Y[Real_Range_Ni[i].num]); ok
                graphics1.DrawEllipse(pen, pos_X[i], (550-Y), 3, 3);
                if (b)// со второго запуска начать рисование
                    graphics1.DrawLine(pen, prevX, prevY, pos_X[i], (550 - Y));
                prevX = pos_X[i];
                prevY = 550 - Y;
                if (!b) 
                    b = true;
            }
            b = false;
            pictureBox2.Image = bitmapFasten2;
            Pen pen2 = new Pen(cl2);           // перо с выбранным цветом
            pen2.Width = 4;
            graphics2 = Graphics.FromImage(bitmapFasten2);
            for (int i = 0; i < pos_X.Length; i++)
            {
                Y = pos_Y[Real_Range_Ni[i].num]; // дает 0
                graphics2.DrawEllipse(pen2, pos_X[i], (550 - Y), 3, 3);
                if (b)// со второго запуска начать рисование
                {
                    graphics2.DrawLine(pen2, prevX, prevY, pos_X[i], prevY);
                    graphics2.DrawLine(pen2, pos_X[i], prevY, pos_X[i], (550 - Y));
                }
                prevX = pos_X[i];
                prevY = 550 - Y;
                if (!b)
                    b = true;
            }
            //graphics1.DrawEllipse(pen, t.x * 20 + 8, 299 - t.y * 20, 5, 5);
            //MessageBox.Show(K+" "+Ni_N.Length.ToString()+" "+Ni_Nh.Length.ToString());
        }
    }
}
