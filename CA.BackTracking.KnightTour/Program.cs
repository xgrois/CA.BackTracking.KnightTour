using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CA.BackTracking.KnightTour
{
    class Program
    {
        static Random rng = new Random();

        static string _programTitle = "The Knight's tour problem";

        static int[,] _board;
        static int _boardSize;
        static int _emptyVal = -1;
        static int _count = 0;

        static List<Delta> _deltas; // Used to generate all moves given a position
        struct Delta
        {
            public readonly int deltaRow;
            public readonly int deltaCol;

            public Delta(int dr, int dc)
            {
                deltaRow = dr;
                deltaCol = dc;
            }
        }

        struct Position
        {
            public readonly int row;
            public readonly int col;

            public Position(int r, int c)
            {
                row = r;
                col = c;
            }

            public override string ToString()
            {
                return $"({row}, {col})";
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"::: {_programTitle} :::\n\r");

            InitializeBoard(5); // board size
            GenerateAllDeltas();

            _board[0, 0] = 1;
            if (!KnightTour(0, 0, 1))
                Console.WriteLine("There is no solution for this combination.\n\r");
            else
                Console.WriteLine($"One solution after {_count} recursive calls:\n\r");

            PrintBoard();

        }

        static void GenerateAllDeltas()
        {
            int[] ones = new int[] { -1, 1};
            int[] twos = new int[] { -2, 2};

            _deltas = new List<Delta>();

            for (int i = 0; i < ones.Length; i++)
            {
                for (int j = 0; j < twos.Length; j++)
                {
                    _deltas.Add(new Delta(ones[i], twos[j]));
                    _deltas.Add(new Delta(twos[i], ones[j]));
                }
            }
        }

        static void InitializeBoard(int boardSize)
        {
            _boardSize = boardSize;
            _board = new int[_boardSize, _boardSize];

            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    _board[i, j] = _emptyVal;
                }
            }
            Console.WriteLine($"Initiallized {_boardSize}x{_boardSize} board.");
        }

        static List<Position> FindAllValidKnightMoves(int row, int col)
        {
            List<Position> options = new List<Position>();

            bool isOutOfBoard, isOccupied;
            foreach (var d in _deltas)
            {
                isOutOfBoard =  (row + d.deltaRow < 0) || (row + d.deltaRow >= _boardSize) ||
                                (col + d.deltaCol < 0) || (col + d.deltaCol >= _boardSize);

                if ( !isOutOfBoard)
                {
                    isOccupied = _board[row + d.deltaRow, col + d.deltaCol] != _emptyVal;
                    if (!isOccupied)
                        options.Add(new Position(row + d.deltaRow, col + d.deltaCol));
                }
                
            }

            return options;
        }

        static bool IsBoardFull()
        {
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    if (_board[i, j] == _emptyVal) return false;
                }
            }

            return true;
        }

        static bool KnightTour(int row, int col, int step)
        {
            _count++;
            if (step == _boardSize*_boardSize) return true;

            var options = FindAllValidKnightMoves(row, col);
            var shuffledOptions = options.OrderBy(a => rng.Next());

            foreach (var position in shuffledOptions)
            {
                step += 1;
                _board[position.row, position.col] = step;
                //Console.Clear();
                //Console.WriteLine(); PrintBoard();
                if (KnightTour(position.row, position.col, step))
                    return true;
                step -= 1;
                _board[position.row, position.col] = _emptyVal;
            }
            return false;
        }

        static void PrintBoard(string marker = "*")
        {
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    if (_board[i, j] == _emptyVal)
                        Console.Write($"{marker}{marker} ");
                    else
                    {
                        if (_board[i, j] > 9)
                            Console.Write($"{_board[i, j]} ");
                        else
                            Console.Write($"{_board[i, j]}  ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
