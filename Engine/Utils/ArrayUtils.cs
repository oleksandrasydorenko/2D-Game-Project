using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Utils
{
	public class ArrayUtils
	{

		/// <summary>
		/// Fills a Jagged Array with a 2D Array, if the Array is not big enough the rest of the 2D Array gets cut off
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void FillJaggedArrayWith2DArray<T>(T[,] source, T[][] target)
		{
			for (int _x = 0; _x < source.GetLength(0); _x++)
			{
				for (int _y = 0; _y < source.GetLength(1); _y++)
				{
					if (_x < target.Length && _y < target[_x].Length)
					{
						target[_x][_y] = source[_x, _y];
					}
					else { break; }
				}
			}
		}

		/// <summary>
		/// Fills a 2D Array with the elements if a Jagged Array, if the array is not big enough the rest of the Jagged Array gets cut off
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Fill2DArrayWithJaggedArray<T>(T[][] source, T[,] target)
		{
			for (int _x = 0; _x < source.Length; _x++)
			{
				for (int _y = 0; _y < source[_x].Length; _y++)
				{
					if (_x < target.GetLength(0) && _y < target.GetLength(1))
					{
						target[_x, _y] = source[_x][_y];
					}
					else { break; }
				}
			}
		}

		/// <summary>
		/// Converts a 2D Array to a Jagged Array the dimensions are the one from the 2D Array
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T[][] Convert2DArrayToJaggedArray<T>(T[,] source) 
		{
			T[][] arrayToFill = new T[source.GetLength(0)][];

			for (int _x = 0; _x < source.GetLength(0); _x++)
			{
				arrayToFill[_x] = new T[source.GetLength(1)];

				for (int _y = 0; _y < source.GetLength(1); _y++)
				{
					arrayToFill[_x][_y] = source[_x, _y];
				}
			}

			return arrayToFill;
		}


		/// <summary>
		/// converts a Jagged Array to a 2D array, the dimensions are the jagged Array x length and the longest y length 
		/// the rest will be filled with default values
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T[,] ConvertJaggedArrayTo2DArray<T>(T[][] source)
		{
			int longestRow = 0;
			for(int i = 1; i < source.Length; i++)
			{
				if (source[longestRow].Length < source[i].Length)
				{
					longestRow = i;
				}
			}

			T[,] arrayToFill = new T[source.Length, source[longestRow].Length]; // creates a new 2D array with the dimensions of the longest row for the y

			for (int _x = 0; _x < source.Length; _x++)
			{
				for (int _y = 0; _y < source[_x].Length; _y++)
				{
					arrayToFill[_x, _y] = source[_x][_y];
				}
			}

			return arrayToFill;
		}

		public static void PrintArray<T>(T[] arrayToPrint, bool newLineAfterEachElement = false, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;

			for (int i = 0; i < arrayToPrint.Length; i++)
			{
				if (newLineAfterEachElement)
				{
					Console.WriteLine(arrayToPrint[i]);
				}
				else
				{
					Console.Write(arrayToPrint[i] + ", ");
				}
			}

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void Print2DArray<T>(T[,] arrayToPrint, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;

			for (int y = 0; y < arrayToPrint.GetLength(1); y++)
			{
				for (int x = 0; x < arrayToPrint.GetLength(0); x++)
				{
					Console.Write(arrayToPrint[x, y] + ", ");
				}
				Console.WriteLine();
			}

			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void PrintJaggedArray<T>(T[][] arrayToPrint, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;

			int maxRows = arrayToPrint.Length;
			int x = 0;

			while (x < maxRows)
			{
				for (int y = 0; y < arrayToPrint[x].Length; y++)
				{
					Console.Write(arrayToPrint[y][x] + ", ");
				}

                Console.WriteLine();

				x++;
			}

			Console.ForegroundColor = ConsoleColor.White;
		}

		
	}
}
