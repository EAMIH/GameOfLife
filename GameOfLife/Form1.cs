using System;
using System.Drawing;
using System.Windows.Forms;


namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private int currentGeneration = 0;
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;
        public Form1()
        {
            InitializeComponent();
        }
        private void Start_Game()
        {
            if (timer1.Enabled)
                return;

            currentGeneration = 0;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;

            // bStart.Enabled = false; // Отключает кнопку "Start"
            // bStop.Enabled = true; // Включает кнопку "Stop"

            Text = $"Generation {currentGeneration}";

            resolution = (int)nudResolution.Value;

            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;

            field = new bool[cols, rows];

            Random random = new Random();
           
            for (int x = 0; x < cols; x++) 
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
                }
            }
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void Stop_Game()
        {
            if (!timer1.Enabled)
                return;
            
            timer1.Stop();

            nudResolution.Enabled = true;
            nudDensity.Enabled = true;

            // bStart.Enabled = true; // Включает кнопку "Start"
            // bStop.Enabled = false; // Отключает кнопку "Stop"
        }

        private void Next_Generation()
        {
            graphics.Clear(Color.Black);

            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neightboursCount = Count_Neightbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neightboursCount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neightboursCount < 2 || neightboursCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];
                    

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            }
                    field = newField;

            pictureBox1.Refresh();

            Text = $"Generation {++currentGeneration}";
        }

        private int Count_Neightbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;

                    bool isSelfCheking = col == x && row == y;
                    bool hasLife = field[col, row];

                    if (hasLife && !isSelfCheking)
                        count++;
                }
            }
            return count;
        }
        private void bStart_Click(object sender, EventArgs e)
        {
            Start_Game();
        }
        private void bStop_Click(object sender, EventArgs e)
        {
            Stop_Game();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Next_Generation();
        }

        private bool Validate_Mouse_Position(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                if (Validate_Mouse_Position(x, y))
                    field[x, y] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                if (Validate_Mouse_Position(x, y))
                    field[x, y] = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {currentGeneration}";
        }
    }
}


