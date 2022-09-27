using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Brush [,] frame;
        Tetromino tetromino;
        Random r;
        bool is_game_over;
        public Form1()
        {
            is_game_over = false;
            r = new Random();
            frame = new Brush[20,10];
            for(int i = 0; i<20; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    frame[i, j] = Brushes.White;
                }
            }
            InitializeComponent();
            generate();
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int a = (panel1.Width - 9)/10;
            for (int i = 0; i < 20; i++)
            {
                for(int j = 0; j<10; j++)
                {
                    Brush brush = frame[i,j];
                    int x = j * (a + 1) + 1;
                    int y = (i) * (a + 1) + 1;
                    if (gray_tiles(i, j))
                    {
                        brush = Brushes.LightGray;
                    }
                    if (tetromino != null &&
                       ((j == tetromino.ax && i == tetromino.ay) ||
                        (j == tetromino.bx && i == tetromino.by) ||
                        (j == tetromino.cx && i == tetromino.cy) ||
                        (j == tetromino.dx && i == tetromino.dy)))
                    {
                        brush = tetromino.color;
                    }
                    e.Graphics.FillRectangle(brush, x, y, a, a);                    
                }
            }
        }
        void generate()
        {
            Species current = (Species) r.Next(7);
            tetromino = new Tetromino();
            tetromino.species = current;
            tetromino.x = 3;
            tetromino.y = 0;
            tetromino.ay = 0;
            tetromino.by = 0;
            tetromino.cy = 0;
            tetromino.dy = 0;
            switch (tetromino.species)
            {
                case Species.Cyan:                    
                    tetromino.ax = 3;
                    tetromino.bx = 4;
                    tetromino.cx = 5;
                    tetromino.dx = 6;
                    tetromino.color = Brushes.Cyan;
                    break;
                case Species.Blue:
                    tetromino.ay = 1;
                    tetromino.by = 1;
                    tetromino.cy = 1;
                    tetromino.ax = 3;
                    tetromino.bx = 4;
                    tetromino.cx = 5;
                    tetromino.dx = 3;
                    tetromino.color = Brushes.Blue;
                    break;
                case Species.Orange:
                    tetromino.ay = 1;
                    tetromino.by = 1;
                    tetromino.cy = 1;
                    tetromino.ax = 3;
                    tetromino.bx = 4;
                    tetromino.cx = 5;
                    tetromino.dx = 5;
                    tetromino.color = Brushes.Orange;
                    break;
                case Species.Yellow:
                    tetromino.cy = 1;
                    tetromino.dy = 1;
                    tetromino.ax = 4;
                    tetromino.bx = 5;
                    tetromino.cx = 4;
                    tetromino.dx = 5;
                    tetromino.color = Brushes.Yellow;
                    break;
                case Species.Green:
                    tetromino.cy = 1;
                    tetromino.dy = 1;
                    tetromino.ax = 3;
                    tetromino.bx = 4;
                    tetromino.cx = 4;
                    tetromino.dx = 5;
                    tetromino.color = Brushes.Green;
                    break;
                case Species.Purple:
                    tetromino.ay = 1;
                    tetromino.by = 1;
                    tetromino.cy = 1;
                    tetromino.ax = 3;
                    tetromino.bx = 4;
                    tetromino.cx = 5;
                    tetromino.dx = 4;
                    tetromino.color = Brushes.Purple;
                    break;
                case Species.Red:
                    tetromino.ax = 3;
                    tetromino.bx = 4;
                    tetromino.cx = 4;
                    tetromino.dx = 5;
                    tetromino.ay = 1;
                    tetromino.by = 1;
                    tetromino.color = Brushes.Red;
                    break;
            }
            loser();
        }
        int most_LR(char side, int [] a)
        {
            int temp = a[0];
            for(int i = 1; i < 4; i++)
            {
                if (side == 'L' && temp > a[i]) temp = a[i];
                if (side == 'R' && temp < a[i]) temp = a[i];
            }
            return temp;
        }
        int most_down(int [] a)
        {
            int temp = a[0];
            for (int i = 1; i < 4; i++)
            {
                if (temp < a[i]) temp = a[i];
            }
            return temp;
        }
        bool gray_tiles(int a, int b) // i    j
        {
            int[] tab = { tetromino.ax, tetromino.bx, tetromino.cx, tetromino.dx };
            int[] tab2 = { tetromino.ay, tetromino.by, tetromino.cy, tetromino.dy };

            for (int i = 0; i < 4; i++)
            {
                if(b == tab[i])
                {
                    if (a == gray_distance(tab,tab2) + tab2[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        int gray_distance(int [] tab, int [] tab2)
        {
            int[] distance = new int[4];
            for(int i = 0; i< 4; i++)
            {
                int j = tab2[i];
                while (j < 20 && frame[j,tab[i]] == Brushes.White)
                {
                    j++;
                }
                distance[i] = j - 1 - tab2[i];
            }
            return distance.Min();
        }
        
        public enum Species
        {
            Cyan,
            Blue,
            Orange,
            Yellow,
            Green,
            Purple,
            Red
        }
        public class Tetromino
        {
            public int ax, ay, bx, by, cx, cy, dx, dy, x, y;
            public Brush color;
            public Species species;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            int[] tab = { tetromino.ay, tetromino.by, tetromino.cy, tetromino.dy };            
            if (most_down(tab) >= 19 ||
                frame[tetromino.ay + 1, tetromino.ax] != Brushes.White ||
                frame[tetromino.by + 1, tetromino.bx] != Brushes.White ||
                frame[tetromino.cy + 1, tetromino.cx] != Brushes.White ||
                frame[tetromino.dy + 1, tetromino.dx] != Brushes.White )
            {
                frame[tetromino.ay, tetromino.ax] = tetromino.color;
                frame[tetromino.by, tetromino.bx] = tetromino.color;
                frame[tetromino.cy, tetromino.cx] = tetromino.color;
                frame[tetromino.dy, tetromino.dx] = tetromino.color;
                is_row_full();
                generate();
            }
            else
            {
                tetromino.ay++;
                tetromino.by++;
                tetromino.cy++;
                tetromino.dy++;
                tetromino.y++;
            }            
            panel1.Refresh();
        }
        void loser()
        {
            int[] tab = { tetromino.ax, tetromino.bx, tetromino.cx, tetromino.dx };
            int[] tab2 = { tetromino.ay, tetromino.by, tetromino.cy, tetromino.dy };

            for (int i = 0; i < 4; i++) 
            {
                if(frame[tab2[i],tab[i]] != Brushes.White)
                {
                    timer1.Stop();
                    is_game_over = true;
                    panel1.Enabled = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    button1.Visible = true;
                    button1.Enabled = true;                    
                }
            }
        }
        void is_row_full()
        {
            for (int i = 19; i >= 0; i--) 
            {
                bool full = true;
                for (int j = 0; j < 10; j++)
                {
                    if (frame[i, j] == Brushes.White)
                    {
                        full = false;
                        break;
                    }
                }
                if (full && i>0) clear_row(i++);
            }
        }
        void clear_row(int i)
        {
            while (i > 1)
            {
                for (int j = 0; j < 10; j++) 
                {
                    frame[i, j] = frame[i-1, j];
                }
                i--;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(tetromino != null && !is_game_over)
            {
                int[] tab = { tetromino.ax, tetromino.bx, tetromino.cx, tetromino.dx };
                int[] tab2 = { tetromino.ay, tetromino.by, tetromino.cy, tetromino.dy };
                if (e.KeyData == Keys.A || e.KeyData == Keys.Left)
                {
                    if (most_LR('L', tab) > 0)                        
                    {
                        if( frame[tetromino.ay, tetromino.ax - 1 ] == Brushes.White &&
                            frame[tetromino.by, tetromino.bx - 1 ] == Brushes.White &&
                            frame[tetromino.cy, tetromino.cx - 1 ] == Brushes.White &&
                            frame[tetromino.dy, tetromino.dx - 1 ] == Brushes.White)
                        {
                            tetromino.ax--;
                            tetromino.bx--;
                            tetromino.cx--;
                            tetromino.dx--;
                            tetromino.x--;
                        }                        
                    }
                }
                if (e.KeyData == Keys.D || e.KeyData == Keys.Right)
                {
                    if (most_LR('R', tab) < 9 )                        
                    {
                        if( frame[tetromino.ay, tetromino.ax + 1] == Brushes.White &&
                            frame[tetromino.by, tetromino.bx + 1] == Brushes.White &&
                            frame[tetromino.cy, tetromino.cx + 1] == Brushes.White &&
                            frame[tetromino.dy, tetromino.dx + 1] == Brushes.White)
                        {
                            tetromino.ax++;
                            tetromino.bx++;
                            tetromino.cx++;
                            tetromino.dx++;
                            tetromino.x++;
                        }
                    }
                }
                if (e.KeyData == Keys.S || e.KeyData == Keys.Down)
                {                    
                    timer1.Interval = 50;
                }
                if (e.KeyData == Keys.W || e.KeyData == Keys.Up)
                {
                    clockwise_turn();
                }
                if (e.KeyData == Keys.Z )
                {
                    counter_clockwise_turn();
                }
                if(e.KeyData == Keys.Space)
                {
                    tetromino.ay += gray_distance(tab, tab2);
                    tetromino.by += gray_distance(tab, tab2);
                    tetromino.cy += gray_distance(tab, tab2);
                    tetromino.dy += gray_distance(tab, tab2);
                    tetromino.y += gray_distance(tab, tab2);
                    timer1.Interval = 1;
                }
                panel1.Refresh();
            }
        }
        void clockwise_turn()
        {
            int a, b;
            if (tetromino.species != Species.Yellow && tetromino.species != Species.Cyan)
            {
                a = -2;
                b = 0;
            }
            else
            {
                a = -2;
                b = -1;
            }
            int temp = -(tetromino.ay - tetromino.y + a) + tetromino.x;
            tetromino.ay = (tetromino.ax - tetromino.x + b) + tetromino.y;
            tetromino.ax = temp;

            temp = -(tetromino.by - tetromino.y + a) + tetromino.x;
            tetromino.by = (tetromino.bx - tetromino.x + b) + tetromino.y;
            tetromino.bx = temp;

            temp = -(tetromino.cy - tetromino.y + a) + tetromino.x;
            tetromino.cy = (tetromino.cx - tetromino.x + b) + tetromino.y;
            tetromino.cx = temp;

            temp = -(tetromino.dy - tetromino.y + a) + tetromino.x;
            tetromino.dy = (tetromino.dx - tetromino.x + b) + tetromino.y;
            tetromino.dx = temp;
            collision();
        }
        void counter_clockwise_turn()
        {
            int a, b;
            if (tetromino.species != Species.Yellow && tetromino.species != Species.Cyan)
            {
                a = 0;
                b = -2;
            }
            else
            {
                a = -1;
                b = -2;
            }
            int temp = (tetromino.ay - tetromino.y + a) + tetromino.x;
            tetromino.ay = -(tetromino.ax - tetromino.x + b) + tetromino.y;
            tetromino.ax = temp;

            temp = (tetromino.by - tetromino.y + a) + tetromino.x;
            tetromino.by = -(tetromino.bx - tetromino.x + b) + tetromino.y;
            tetromino.bx = temp;

            temp = (tetromino.cy - tetromino.y + a) + tetromino.x;
            tetromino.cy = -(tetromino.cx - tetromino.x + b) + tetromino.y;
            tetromino.cx = temp;

            temp = (tetromino.dy - tetromino.y + a) + tetromino.x;
            tetromino.dy = -(tetromino.dx - tetromino.x + b) + tetromino.y;
            tetromino.dx = temp;
            collision();
        }
        void collision()
        {
            int[] tab = { tetromino.ax, tetromino.bx, tetromino.cx, tetromino.dx };
             int[] tab2 = { tetromino.ay, tetromino.by, tetromino.cy, tetromino.dy };
            for(int i = 0; i < 4; i++)
            {
                if (tab[i] < 0)
                {
                    tetromino.ax++;
                    tetromino.bx++;
                    tetromino.cx++;
                    tetromino.dx++;
                    tetromino.x++;
                    break;
                }
                if (tab[i] > 9)
                {
                    tetromino.ax--;
                    tetromino.bx--;
                    tetromino.cx--;
                    tetromino.dx--;
                    tetromino.x--;
                    break;
                }
                if (tab2[i] <= -1)
                {
                    tetromino.ay++;
                    tetromino.by++;
                    tetromino.cy++;
                    tetromino.dy++;
                    tetromino.y++;
                    break;
                }
                while (frame[tab2[i],tab[i]] != Brushes.White)
                {
                    tab2[0] = --tetromino.ay;
                    tab2[1] = --tetromino.by;
                    tab2[2] = --tetromino.cy;
                    tab2[3] = --tetromino.dy;
                    tetromino.y--;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.S || e.KeyData == Keys.Down || e.KeyData == Keys.Space)
            {
                timer1.Interval = 500;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            is_game_over = false;
            panel1.Enabled = true;
            label1.Visible = false;
            label2.Visible = false;
            button1.Visible = false;
            button1.Enabled = false;
            panel1.Refresh();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    frame[j, i] = Brushes.White;
                }
            }
        }
    }
}
