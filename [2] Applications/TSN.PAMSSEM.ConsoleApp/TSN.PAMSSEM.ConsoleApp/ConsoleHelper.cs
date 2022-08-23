/*
* PAMSSEM CALCULATOR
* Version   : 1.0
* Author    : MUSTAFA TOSUN, https://mustafatosun.net
* Date      : 2022-08-24
* Licence   : NONE
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TSN.PAMSSEM.ConsoleApp
{
    internal static class ConsoleHelper
    {
        static ConsoleHelper()
        {
            _assemblyTitle = typeof(Program).Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            _assemblyFileVersion = typeof(Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            _horizontalSeperator = new string('-', 100);
        }


        private static readonly string _assemblyTitle;
        private static readonly string _assemblyFileVersion;
        private static readonly string _horizontalSeperator;



        public static void Write(params string[] lines)
        {
            if ((lines?.Length ?? 0) == 0)
                return;
            var lastIndex = lines.Length - 1;
            foreach (var s in lines.Take(lastIndex))
                Console.WriteLine(s);
            Console.Write(lastIndex);
        }
        public static void WriteHorizontalSeperator(ushort linesBefore = 0, ushort linesAfter = 1)
        {
            for (ushort i = 0; i < linesBefore; i++)
                Console.WriteLine();
            Console.Write(_horizontalSeperator);
            for (ushort i = 0; i < linesAfter; i++)
                Console.WriteLine();
        }
        public static string ReadLine(string prompt, bool lineAfterPrompt = false)
        {
            if (lineAfterPrompt)
                Console.WriteLine(prompt);
            else
                Console.Write(prompt);
            return Console.ReadLine();
        }
        public static IList<string> ReadLines(Func<int, string> prompt, bool lineAfterPrompt = false, ConsoleKey acceptKey = ConsoleKey.Enter, ConsoleKey cancelKey = ConsoleKey.Escape, bool acceptEmptyLines = false, int maxLines = -1, Func<string, int, bool> validateLine = null, Func<string, int, string> invalidLineMessage = null)
        {
            var max = maxLines < 0 ? int.MaxValue : maxLines;
            var write = lineAfterPrompt ? new Action<int>(x => Console.WriteLine(prompt(x))) : new Action<int>(x => Console.Write(prompt(x)));
            var lines = new List<string>();
            var cancel = false;
            for (List<char> s; !cancel && lines.Count < max;)
            {
                write(lines.Count);
                s = new List<char>();
                for (ConsoleKeyInfo cki; !(cancel = (cki = Console.ReadKey(true)).Key == cancelKey && lines.Count > 0);)
                {
                    if (cki.Key == acceptKey)
                    {
                        if (!acceptEmptyLines && s.Count == 0)
                            continue;
                        break;
                    }
                    if (cki.Key == ConsoleKey.Backspace)
                    {
                        s.RemoveAt(s.Count - 1);
                        Console.Write("\b \b");
                    }
                    else if (char.IsLetterOrDigit(cki.KeyChar) && (!char.IsWhiteSpace(cki.KeyChar) || (cki.KeyChar == ' ' && s.Count > 0 && s.Last() != ' ')))
                    {
                        s.Add(cki.KeyChar);
                        Console.Write(cki.KeyChar);
                    }
                }
                Console.WriteLine();
                if (cancel)
                    break;
                var line = string.Concat(s).Trim();
                if (validateLine?.Invoke(line, lines.Count) ?? true)
                    lines.Add(line);
                else
                    Console.WriteLine(invalidLineMessage?.Invoke(line, lines.Count));
            }
            lines.TrimExcess();
            return lines;
        }
        public static IList<string> ReadLines(string prompt, bool lineAfterPrompt = false, ConsoleKey acceptKey = ConsoleKey.Enter, ConsoleKey cancelKey = ConsoleKey.Escape, bool acceptEmptyLines = false, int maxLines = -1, Func<string, int, bool> validateLine = null, Func<string, int, string> invalidLineMessage = null) => ReadLines(i => prompt, lineAfterPrompt, acceptKey, cancelKey, acceptEmptyLines, maxLines, validateLine, invalidLineMessage);

        public static void WriteAppInfo()
        {
            Console.Title = _assemblyTitle;
            Console.WriteLine(_assemblyTitle);
            Console.WriteLine($"Versiyon: {_assemblyFileVersion}");
        }
    }
}
