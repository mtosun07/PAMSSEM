using System;
using System.Collections.Generic;
using System.Linq;

namespace TSN.PAMSSEM.ConsoleApp
{
    internal static class Program
    {
        static Program() { }



        private static void Main()
        {
            //NewProblem();
            SolveProblemFromTheBook();
            Console.ReadLine();
        }

        private static void NewProblem()
        {
            string tmp;
            for (var b = false; ;)
            {
                Console.Clear();

                ConsoleHelper.WriteAppInfo();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ALTERNATİFLER");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Alternatiflerin isimlerini giriniz.\nHer bir alternatif adını onaylamak için Enter'a, girişi tamamlamak için ESC'ye basınız.\n");
                var alternativeNames = ConsoleHelper.ReadLines(">:\t");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ÖZELLİKLER");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Özelliklerin isimlerini giriniz.\nHer bir özellik adını onaylamak için Enter'a, girişi tamamlamak için ESC'ye basınız.\n");
                var attributeNames = ConsoleHelper.ReadLines(">:\t");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ÖZELLİK AĞIRLIKLARI");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Özelliklerin ağırlık değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var weights = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} için w >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ORDİNALLİK / KARDİNALLİK");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Özelliklerin ordinallik - kardinallik durumlarını belirtiniz.\nOrdinal özellikler için O, kardinal özellikler için K giriniz.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                for (; !(tmp = ConsoleHelper.ReadLine("O/K >: ").ToUpperInvariant()).Equals("O") && !tmp.Equals("K");)
                    Console.WriteLine("\tHATALI GİRİŞ!");
                var isOrdinal = tmp.Equals("O");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("KAYITSIZLIK EŞİĞİ PARAMETRELERİ");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Her bir özellik için Kayıtsızlık Eşiği Parametrelerinin (q) değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var q = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn q >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("TERCİH EŞİĞİ PARAMETRELERİ");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Her bir özellik için Tercih Eşiği Parametrelerinin (p) değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var p = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn p >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("REDDETME EŞİĞİ PARAMETRELERİ");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Her bir özellik için Reddetme Eşiği Parametrelerinin (v) değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var v = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn v >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                //Console.WriteLine("ALTERNATİFLER ARASI DEĞİŞİM");
                //ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                //Console.WriteLine("Alternatifler arası değişim değerlerini giriniz.\nHer bir değeri onaylamak için Enter'a, girişi tamamlamak için ESC'ye basınız.\n");
                //var differences = new double?[attributeNames.Count, alternativeNames.Count, alternativeNames.Count];
                //for (int i = 0; i < attributeNames.Count; i++)
                //    for (int j = 0; j < alternativeNames.Count; j++)
                //        for (int k = 0; k < alternativeNames.Count; k++)
                //        {
                //            double d;
                //            while (!double.TryParse(ConsoleHelper.ReadLine($"{attributeNames[i]} için {alternativeNames[j]} - {alternativeNames[k]} = "), out d))
                //                Console.WriteLine("\tHATALI GİRİŞ!");
                //            differences[i, j, k] = d;
                //        }
                //ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                var decisionMatrix = new double[alternativeNames.Count, attributeNames.Count];
                Console.WriteLine("KARAR MATRİSİ");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Karar Matrisi'nin değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                for (int i = 0; i < alternativeNames.Count; i++)
                    for (int j = 0; j < attributeNames.Count; j++)
                        while (!double.TryParse(ConsoleHelper.ReadLine($"{alternativeNames[i]} ve {attributeNames[j]} için değer >: "), out decisionMatrix[i, j]))
                            Console.WriteLine("\tHATALI GİRİŞ!");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("KARAR MATRİSİ İÇİN +/- DEĞERLERİ");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine("Karar matrisindeki her bir özellik için +/- değerlerini belirtiniz.\n+ değeri için P, - değeri için N giriniz.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var isPositive = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} için P/N >: ", maxLines: attributeNames.Count, validateLine: (x, i) => {
                    x = x.ToUpperInvariant();
                    return x.Equals("P") || x.Equals("N");
                }, invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => x.ToUpperInvariant()).Select(x => x.Equals("P")).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.Write("Hesaplama aşamasına geçmek için Enter'a, değerleri en baştan girmek için ESC'ye basınız . . .");
                for (ConsoleKeyInfo cki; (cki = Console.ReadKey(true)).Key != ConsoleKey.Enter && !(b = cki.Key == ConsoleKey.Escape);) ;
                if (b)
                    break;
                Console.Clear();
                CalculateWithPAMSSEM(alternativeNames, isOrdinal, Enumerable.Range(0, attributeNames.Count).Select(x => (attributeNames[x], weights[x], q[x], p[x], v[x], isPositive[x])).ToArray()/*, differences*/, decisionMatrix);
            }
        }
        private static void SolveProblemFromTheBook()
        {
            var alternatives = new[] { "A1", "A2", "A3" };
            var isOrdinal = true;
            var attributes = new List<(string Name, double Weight, double q, double p, double v, bool IsPositive)> {
                ("C1", 1D, 5D, 12D, 18D, false),
                ("C2", 1D, 15D, 25D, 32D, true),
                ("C3", 1D, 1D, 2D, 3D, true)
            };
            var decisionMatrix = new double[3, 3];
            decisionMatrix[0, 0] = 80D;
            decisionMatrix[0, 1] = 90D;
            decisionMatrix[0, 2] = 5D;
            decisionMatrix[1, 0] = 65D;
            decisionMatrix[1, 1] = 58D;
            decisionMatrix[1, 2] = 2D;
            decisionMatrix[2, 0] = 83D;
            decisionMatrix[2, 1] = 60D;
            decisionMatrix[2, 2] = 7D;
            CalculateWithPAMSSEM(alternatives, isOrdinal, attributes, decisionMatrix);
        }
        private static void CalculateWithPAMSSEM(IList<string> alternatives, bool IsOrdinal, IList<(string Name, double Weight, double q, double p, double v, bool IsPositive)> attributes/*, double?[,,] differences*/, double[,] decisionMatrix)
        {
            if ((alternatives?.Count ?? 0) == 0/* || differences == null || differences.GetLength(0) != attributes.Count || differences.GetLength(1) != alternatives.Count || differences.GetLength(2) != alternatives.Count*/ || (attributes?.Count ?? 0) == 0 || decisionMatrix == null || decisionMatrix.GetLength(0) != alternatives.Count || decisionMatrix.GetLength(1) != attributes.Count || alternatives.Any(x => string.IsNullOrWhiteSpace(x)) || attributes.Any(x => string.IsNullOrWhiteSpace(x.Name)))
                throw new ArgumentException();

            Console.WriteLine("ALTERNATİFLER");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
            Console.WriteLine(string.Join("\n", alternatives));
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

            Console.WriteLine("ORDİNALLİK");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
            Console.WriteLine(IsOrdinal ? "Ordinal" : "Kardinal");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

            foreach (var attr in attributes)
            {
                Console.WriteLine($"ÖZELLİK:\t{attr.Name}");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
                Console.WriteLine($"Ağırlık:\t{attr.Weight}");
                Console.WriteLine($"q:\t\t{attr.q}");
                Console.WriteLine($"p:\t\t{attr.p}");
                Console.WriteLine($"v:\t\t{attr.v}");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            }

            Console.WriteLine("KARAR MATRİSİ");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < attributes.Count; j++)
                    Console.WriteLine($"({(attributes[j].IsPositive ? "+" : "-")}) [{alternatives[i]}, {attributes[j].Name}] = {decisionMatrix[i, j]}");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

            #region . Stage 1 .
            var outrankingIndex = new double?[alternatives.Count, alternatives.Count];
            if (IsOrdinal)
                for (int i = 0; i < alternatives.Count; i++)
                    for (int j = 0; j < alternatives.Count; j++)
                    {
                        if (i == j)
                            continue;
                        var sum = 0D;
                        for (int k = 0; k < attributes.Count; k++)
                        {
                            var diff = (decisionMatrix[i, k] - decisionMatrix[j, k]) * (attributes[k].IsPositive ? 1 : -1);
                            if (diff <= -attributes[k].p)
                                sum += 0;
                            else if (diff >= -attributes[k].q)
                                sum += 1;
                            else if (-attributes[k].p < diff - attributes[k].q && attributes[k].p >= attributes[k].q && attributes[k].q >= 0)
                            {
                                var divisor = attributes[k].p - attributes[k].q;
                                if (divisor == 0D)
                                {
                                    sum = double.NaN;
                                    break;
                                }
                                sum += (diff - attributes[k].p) / divisor;
                            }
                            else
                            {
                                sum = double.NaN;
                                break;
                            }
                        }
                        outrankingIndex[i, j] = sum;
                    }
            else
            {
            }

            Console.WriteLine("AŞAMA 1 - YEREL GEÇİŞ İNDİSİ");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 1);
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < alternatives.Count; j++)
                    Console.WriteLine($"[{alternatives[i]}, {alternatives[j]}] = {outrankingIndex[i, j]?.ToString() ?? string.Empty}");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion
        }
    }
}