using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLife
{
    public class GameEngine
    {
        public uint currentGeneration { get; private set; }
        private bool[,] field;
        public readonly int rows;
        private readonly int cols;

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];
            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }

        public bool[,] GetCurrentGenetation()
        {
            bool[,] result = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }

        private int CountNeightbours(int x, int y)
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

        public void NextGeneration()
        {

            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neightboursCount = CountNeightbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neightboursCount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neightboursCount < 2 || neightboursCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];
                }
            }
            field = newField;
            currentGeneration++;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, false);
        }
    }
}
