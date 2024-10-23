using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Customizable_Windows.CLI
{
    public enum TextColor
    {
        DEFAULT = 0,
        BLACK = 30,
        GREEN = 32,
        YELLOW = 33,
        BLUE = 34,
        MAGENTA = 35,
        CYAN = 36,
        WHITE = 37
    }

    public enum TextFormat
    {
        BOLD = 1,
        UNDERLINE = 4,
        BLINK = 5, // may not be supported
        INVERTED = 7,
        STRIKETHROUGH = 9
    }
    public class TextUtils
    {

        public static string Format(string text, TextColor foreground, TextColor background, params TextFormat[] formats)
        {
            StringBuilder stringBuilder = new StringBuilder("\u001b[");
            foreach (var format in formats)
            {
                stringBuilder.Append($"{(int)format};");
            }
            stringBuilder.Append($"{(int)foreground};{(int)background + 10}m{text}\u001b[0m");

            return stringBuilder.ToString();
        }

        private static int MaxElementStringLength<T>(T[] elements)
        {
            int max = 0;
            foreach (var element in elements) 
            {
                string stringified = element.ToString();
                if (max < stringified.Length) max = stringified.Length;
            }

            return max;
        }

        private static string PadRight(string s, int space)
        {
            StringBuilder sb = new StringBuilder(s);
            for (int i = 1; i <= space; i++)
            {
                sb.Append(" ");
            }
            return sb.ToString();
        }

        private static string PadLeft(string s, int space)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= space; i++)
            {
                sb.Append(' ');
            }
            sb.Append(s);
            return sb.ToString();
        }

        public static void PrintArraySplittedToColumns<T>(string title, T[] entries)
        {
            const int titlespace = 4;
            int tableRowLength = MaxElementStringLength(entries) + 4;
            int columns = Console.BufferWidth / tableRowLength * Console.BufferHeight;
            int rows = Console.BufferHeight - (titlespace+4);

            // to make the "table" look good, each entry has
            // max string size + 4, where 4 is space between rows

            Console.SetCursorPosition(Console.BufferWidth / 2 - title.Length / 2, titlespace / 2);
            Console.Write(Format(title, TextColor.BLUE, TextColor.WHITE, TextFormat.BOLD, TextFormat.UNDERLINE));
            Console.SetCursorPosition(0, titlespace+1);

            int column = 0;
            int row = titlespace;
            foreach (var entry in entries)
            {
                string stringified = entry.ToString();
                int padding = tableRowLength/2 - stringified.Length/2;
                int rest = tableRowLength%2 - stringified.Length%2;
                string paddedLeft = PadLeft(stringified, padding);
                string paddedRight = PadRight(paddedLeft, padding + rest);
                Console.Write($"{paddedRight}|");

                if (++row == Console.BufferHeight-4)
                {
                    column+=tableRowLength+1;
                    row = titlespace+1;
                }
                Console.SetCursorPosition(column, row);
            }

            Console.SetCursorPosition(0, Console.BufferHeight-1);
        }
    }
    public class Form<T>
    {
        private readonly List<T> _items;
        private readonly string _inputDescription;
        public Form(string inputDescription, List<T> availableOptions)
        {
            _items = availableOptions;
            _inputDescription = inputDescription;
        }

        public T Show()
        {
            Console.WriteLine(TextUtils.Format("Available options:", TextColor.WHITE, TextColor.CYAN, TextFormat.BOLD));
            foreach (T value in _items)
            {
                Console.WriteLine($"- {value.ToString()}");
            }

            while (true)
            {
                Console.Write($"{_inputDescription}: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                // search for option (linear)
                foreach (T value in _items)
                {
                    if (value.ToString().ToLower() == input.ToLower())
                    {
                        Console.Clear();
                        return value;
                    }
                }

                Console.WriteLine("Selected option doesn't exist.");
            }
        }
    }
}
