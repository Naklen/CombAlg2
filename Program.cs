using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombAlg2
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialPositions = GetInitialPositions();
            var route = BuildRouteToPawn(findPawnPos(initialPositions));
            WriteRoute(route);
        }

        static List<string> BuildRouteToPawn(Position foundPawnPos)
        {
            var result = new List<string>();
            var position = foundPawnPos;
            while (position != null)
            {
                result.Add("" + position.Letter + position.Numeral);
                position = position.PreviousPosition;
            }
            result.Reverse();
            return result;
        }

        static Position findPawnPos(Tuple<Position, Position> initPositions) 
        {
            var knightInitPos = initPositions.Item1;
            var pawnInitPos = initPositions.Item2;
            var queue = new Queue<Position>();
            queue.Enqueue(knightInitPos);
            while (queue.Count != 0)
            {
                var position = queue.Dequeue();
                if (IsHitByPawn(pawnInitPos, position))
                    continue;
                if (pawnInitPos.isSamePosition(position))
                    return position;
                if (position.Numeral <= 6) 
                {
                    var newNumeral = position.Numeral + 2;
                    if (position.Letter <= 'g')
                        queue.Enqueue(new Position(Convert(position.Letter + 1), newNumeral, position));
                    if (position.Letter >= 'b')
                        queue.Enqueue(new Position(Convert(position.Letter - 1), newNumeral, position));
                }
                if (position.Letter >= 'c')
                {
                    var newLetter = Convert(position.Letter - 2);
                    if (position.Numeral <= 7)
                        queue.Enqueue(new Position(newLetter, position.Numeral + 1, position));
                    if (position.Numeral >= 2)
                        queue.Enqueue(new Position(newLetter, position.Numeral - 1, position));
                }
                if (position.Numeral >= 3)
                {
                    var newNumeral = position.Numeral - 2;
                    if (position.Letter <= 'g')
                        queue.Enqueue(new Position(Convert(position.Letter + 1), newNumeral, position));
                    if (position.Letter >= 'b')
                        queue.Enqueue(new Position(Convert(position.Letter - 1), newNumeral, position));
                }
                if (position.Letter <= 'f')
                {
                    var newLetter = Convert(position.Letter + 2);
                    if (position.Numeral <= 7)
                        queue.Enqueue(new Position(newLetter, position.Numeral + 1, position));
                    if (position.Numeral >= 2)
                        queue.Enqueue(new Position(newLetter, position.Numeral - 1, position));
                }
            }
            return null;
        }

        static Tuple<Position, Position> GetInitialPositions()
        {
            var knightPosStr = "";
            var pawnPosStr = "";          
            try
            {
                using (var sr = new StreamReader("./in.txt"))
                {
                    knightPosStr = sr.ReadLine();
                    pawnPosStr = sr.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }
            var knightPos = new Position(knightPosStr[0], int.Parse(knightPosStr[1].ToString()));
            var pawnPos = new Position(pawnPosStr[0], int.Parse(pawnPosStr[1].ToString()));
            return new Tuple<Position, Position>(knightPos, pawnPos);
        }

        static void WriteRoute(List<string> route)
        {
            try
            {
                using (var sw = new StreamWriter("./out.txt", false, Encoding.Default))
                {
                    foreach (var pos in route)
                    {
                        sw.WriteLine(pos);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        static bool IsHitByPawn(Position pawnPos, Position pos)
        {
            return (pawnPos.Letter == pos.Letter + 1 && pawnPos.Numeral == pos.Numeral + 1) || (pawnPos.Letter == pos.Letter + 1 && pawnPos.Numeral == pos.Numeral - 1)
                || (pawnPos.Letter == pos.Letter - 1 && pawnPos.Numeral == pos.Numeral + 1) || (pawnPos.Letter == pos.Letter - 1 && pawnPos.Numeral == pos.Numeral - 1);
        }

        static char Convert(int numeral) //converter from integer to char
        {
            return char.ConvertFromUtf32(numeral)[0];
        }
    }

    class Position
    {
        public char Letter { get; }
        public int Numeral { get; }
        public Position PreviousPosition { get; }

        public Position(char letter, int numeral, Position previous = null)
        {
            Letter = letter;
            Numeral = numeral;
            PreviousPosition = previous;
        }

        public bool isSamePosition(Position position)
        {
            return Letter == position.Letter && Numeral == position.Numeral;
        }
    }
}
