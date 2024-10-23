using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("Available options");
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
