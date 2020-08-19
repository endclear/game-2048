using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Input;

namespace Demo
{
    public partial class Form1 : Form
    {
        private int KhoangCach = 5;
        int Diem_Undo, Da_undo = 0, giay = 30, phut = 0;
        private Random rand = new Random();
        //StreamReader doc = new StreamReader("diem.txt");

        private int Diem = 0,Sobuoc=0;
        private Label[,] OSo = new Label[4, 4];
        int[,] SoO = new int[4, 4];
        int[,] ArrUndo = new int[4, 4];
        public Form1()
        {
            InitializeComponent();
        }
        private void frm_laod()
        {
            //lb_diem.Text = "0";
            //lb_kl.Text = doc.ReadToEnd();
            //doc.Close();
            //if (lb_kl.Text == "")
            //    lb_kl.Text = "0";
            
            //tạo ma tran 4x4
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    OSo[i, j] = new Label();
                    OSo[i, j].Location = new Point(KhoangCach + i * (100 + KhoangCach), KhoangCach + j * (100 + KhoangCach));
                    OSo[i, j].Size = new Size(100, 100);
                    OSo[i, j].TabIndex = i * 4 + j;
                    OSo[i, j].Name = String.Format("lb%d%d", i, j);
                    OSo[i, j].BackColor = Color.FromName("ActiveBorder");
                    OSo[i, j].Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    OSo[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    groupBox1.Controls.Add(OSo[i, j]);
                }
            }
            LoadO();
            LoadTime(0, 30);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            frm_laod();
            
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    ArrUndo[x, y] = SoO[x, y];
                }
            }
        }
        //set màu và text cho các ô
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (SoO[i, j] == 0) //nếu là ô trống
                        OSo[i, j].Text = "";
                    else//ko là ô trống
                        OSo[i, j].Text = SoO[i, j].ToString();
                    setMauChoO(i, j);
                }
            }
            lb_diem.Text = Diem.ToString();
            lblBuoc.Text = Sobuoc.ToString();
            
        }

        bool RanDomViTriHien()
        {
            bool isDo = false;
            List<int> test = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                if (SoO[i / 4, i % 4] == 0)
                {
                    test.Add(i);
                    isDo = true;
                }
            }
            if (test.Count > 0)
            {
                int set = test[rand.Next(0, test.Count - 1)];//lấy ngẫu nhiên các ô số trống
                while (SoO[set / 4, set % 4] != 0 && test.Count > 1)
                {
                    test.Remove(set);
                    set = test[rand.Next(0, test.Count - 1)];
                }
                SoO[set / 4, set % 4] = rand.Next(1, 100) > 90 ? 4 : 2;
                Diem += SoO[set / 4, set % 4];
            }
            return isDo;
        }

        #region Su kien Bam Phim
        bool PhimLen()
        {
            bool isDo = false;
            Sobuoc++;
            Set_Undo();
            Diem_Undo = Diem;
            Da_undo = 1;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int y1 = y + 1; y1 < 4; y1++)
                    {
                        if (SoO[x, y1] > 0)
                        {
                            
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x, y1];
                                SoO[x, y1] = 0;
                                y--;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x, y1])
                            {
                                SoO[x, y] *= 2;
                                SoO[x, y1] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }
        
        bool PhimXuong()
        {
            bool isDo = false;
            Sobuoc++;
            Set_Undo();
            Diem_Undo = Diem;
            Da_undo = 1;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 3; y >= 1; y--)
                {
                    for (int y1 = y - 1; y1 >= 0; y1--)
                    {
                        if (SoO[x, y1] > 0)
                        {
                            
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x, y1];
                                SoO[x, y1] = 0;
                                y++;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x, y1])
                            {
                                SoO[x, y] *= 2;
                                SoO[x, y1] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }

        bool PhimPhai()
        {
            bool isDo = false;
            Sobuoc++;
            Set_Undo();
            Diem_Undo = Diem;
            Da_undo = 1;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 3; x >= 1; x--)
                {
                    for (int x1 = x - 1; x1 >= 0; x1--)
                    {
                        if (SoO[x1, y] > 0)
                        {
                            
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x1, y];
                                SoO[x1, y] = 0;
                                x++;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x1, y])
                            {
                                SoO[x, y] *= 2;
                                SoO[x1, y] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }

        bool PhimTrai()
        {
            bool isDo = false;
            Sobuoc++;
            Diem_Undo = Diem;
            Set_Undo();
            Da_undo = 1;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int x1 = x + 1; x1 < 4; x1++)
                    {
                        if (SoO[x1, y] > 0)
                        {
                            
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x1, y];
                                SoO[x1, y] = 0;
                                x--;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x1, y])
                            {
                                SoO[x, y] *= 2;
                                SoO[x1, y] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }
        #endregion

        private void Set_Undo()
        {
            for (int x=0 ; x<4 ; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    ArrUndo[x, y] = SoO[x, y];
                }
            }
        }

        private void Load_Undo()
        {
            if (Da_undo == 1)
            {
                if (Diem_Undo > 0)
                {
                    Diem -= (Diem-Diem_Undo);
                }
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        SoO[x, y] = ArrUndo[x, y];
                    }
                }
                Da_undo = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (phut == 0 && giay == 0)
            {
                timer1.Stop();
                if (MessageBox.Show("Điểm: " + Diem.ToString() + "\n" + "Bạn có muốn chơi lại không?",
                "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    Application.Exit();
                else
                {
                    this.Hide();
                    Form1 fr = new Form1();
                    fr.Show();
                }
            }
            if (!CheckGameOver())
            {
                giay--;
                if (giay == 0&&phut>=1)
                {
                    giay = 59;
                    phut--;

                }      
                LoadTime(phut, giay);
            }
        }
        private void LoadTime(int _phut, int _giay)
        {
            String strTime = "";
            if (_phut < 10)
            {
                strTime = "0" + _phut.ToString();
            }
            else
            {
                strTime = _phut.ToString();
            }
            strTime = strTime + ":";
            if (_giay < 10)
            {
                lbTime.Text = strTime.ToString() + "0" + _giay.ToString();
            }
            else
            {
                lbTime.Text = strTime + _giay.ToString();
            }
            

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyData == Keys.Up)
                PhimLen();
            if (e.KeyData == Keys.Down)
                PhimXuong();
            if (e.KeyData == Keys.Right)
                PhimPhai();
            if (e.KeyData == Keys.Left)
                PhimTrai();
            if (e.KeyData == (Keys.Control|Keys.Z))
                Load_Undo();
            if (e.KeyData == Keys.F1)
            {
                //if (Convert.ToInt32(lb_diem.Text) > Convert.ToInt32(lb_kl.Text))
                //{
                    //doc.Close();
                    //StreamWriter ghi = new StreamWriter("diem.txt");
                    //ghi.WriteLine(lb_diem.Text);
                    //ghi.Flush();
                    //ghi.Close();
                    this.Hide();
                    Form1 fr = new Form1();
                    fr.Show();
                //}
                //else
                //{
                //    this.Hide();
                //    Form1 fr = new Form1();
                //    fr.Show();
                //}
            }
            this.Refresh();
            if (CheckGameOver())
            {
                //if (Convert.ToInt32(lb_diem.Text) > Convert.ToInt32(lb_kl.Text))
                //{
                //    //doc.Close();
                //    StreamWriter ghi = new StreamWriter("diem.txt");
                //    ghi.WriteLine(lb_diem.Text);
                //    ghi.Flush();
                //    ghi.Close();
                //}
                if(MessageBox.Show("Điểm: " + Diem.ToString() + "\n" + "Bạn có muốn chơi lại không?",
                    "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    Application.Exit();
                else
                {
                    this.Hide();
                    Form1 fr = new Form1();
                    fr.Show();
                }
            }
        }

       

        bool CheckGameOver()
        {

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (SoO[x, y] == 0 ||
                        (y < 3 && SoO[x, y] == SoO[x, y + 1]) ||
                        (x < 3 && SoO[x, y] == SoO[x + 1, y]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        void LoadO()
        {
            Diem = 0;
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    SoO[x, y] = 0;
            RanDomViTriHien();
            RanDomViTriHien();
            this.Refresh();
        }

        void setMauChoO(int x, int y)
        {
            switch (SoO[x, y])
            {
                case 0: OSo[x, y].BackColor = Color.FromArgb(220,220,220); break;
                case 2: OSo[x, y].BackColor = Color.FromArgb(255, 192, 192); break;
                case 4: OSo[x, y].BackColor = Color.FromArgb(255, 128, 128); break;
                case 8: OSo[x, y].BackColor = Color.FromArgb(255, 224, 192); break;
                case 16: OSo[x, y].BackColor = Color.FromArgb(255, 192, 128); break;
                case 32: OSo[x, y].BackColor = Color.FromArgb(255, 255, 192); break;
                case 64: OSo[x, y].BackColor = Color.FromArgb(255, 255, 128); break;
                case 128: OSo[x, y].BackColor = Color.FromArgb(192, 255, 192); break;
                case 256: OSo[x, y].BackColor = Color.FromArgb(128, 255, 128); break;
                case 512: OSo[x, y].BackColor = Color.FromArgb(192, 255, 255); break;
                case 1024: OSo[x, y].BackColor = Color.FromArgb(128, 255, 255); break;
                case 2048: OSo[x, y].BackColor = Color.FromArgb(192, 192, 255); break;
                case 4096: OSo[x, y].BackColor = Color.FromArgb(128, 128, 255); break;
                case 8192: OSo[x, y].BackColor = Color.FromArgb(255, 192, 255); break;
            }
        }
        
    }
}
