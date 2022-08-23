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
            for (bool b1 = false; !b1;)
            {
                Console.Clear();

                ConsoleHelper.WriteAppInfo();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("[1] : Kitaptaki Problem");
                Console.WriteLine("[2] : Yeni Problem Girişi");
                Console.WriteLine();
                var input = ConsoleHelper.ReadLines("Seçiminiz (1/2) >: ", maxLines: 1, validateLine: (x, i) => x.Equals("1") || x.Equals("2"), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Single();

                Console.Clear();
                ConsoleHelper.WriteAppInfo();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                switch (input)
                {
                    case "1":
                        ReadNewProblem();
                        break;
                    case "2":
                        SolveProblemFromTheBook();
                        break;
                }

                Console.Write("Hesaplama Tamamlandı. Yeni bir hesaplama için Enter'a, çıkmak için ESC'ye basın . . .");
                for (ConsoleKeyInfo cki; !(b1 = (cki = Console.ReadKey(true)).Key == ConsoleKey.Escape) && cki.Key != ConsoleKey.Enter;) ;
            }
        }

        private static void ReadNewProblem()
        {
            string tmp;
            for (var b = false; ;)
            {
                Console.Clear();

                Console.WriteLine("ALTERNATİFLER");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Alternatiflerin isimlerini giriniz.\nHer bir alternatif adını onaylamak için Enter'a, girişi tamamlamak için ESC'ye basınız.\n");
                var alternativeNames = ConsoleHelper.ReadLines(">:\t");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ÖZELLİKLER");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Özelliklerin isimlerini giriniz.\nHer bir özellik adını onaylamak için Enter'a, girişi tamamlamak için ESC'ye basınız.\n");
                var attributeNames = ConsoleHelper.ReadLines(">:\t");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ÖZELLİK AĞIRLIKLARI");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Özelliklerin ağırlık değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var weights = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} için w >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("ORDİNALLİK / KARDİNALLİK");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Özelliklerin ordinallik - kardinallik durumlarını belirtiniz.\nOrdinal özellikler için O, kardinal özellikler için K giriniz.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                for (; !(tmp = ConsoleHelper.ReadLine("O/K >: ").ToUpperInvariant()).Equals("O") && !tmp.Equals("K");)
                    Console.WriteLine("\tHATALI GİRİŞ!");
                var isOrdinal = tmp.Equals("O");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("KAYITSIZLIK EŞİĞİ PARAMETRELERİ");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Her bir özellik için Kayıtsızlık Eşiği Parametrelerinin (q) değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var q = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn q >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("TERCİH EŞİĞİ PARAMETRELERİ");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Her bir özellik için Tercih Eşiği Parametrelerinin (p) değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var p = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn p >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("REDDETME EŞİĞİ PARAMETRELERİ");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Her bir özellik için Reddetme Eşiği Parametrelerinin (v) değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                var v = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn v >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out _), invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                double[] y;
                if (isOrdinal)
                    y = null;
                else
                {
                    Console.WriteLine("ÖLÇÜM BİRİMİ SEVİYELERİNİN NUMARALARI");
                    ConsoleHelper.WriteHorizontalSeperator();
                    Console.WriteLine("Her bir özellik için Ölçüm Birimi Seviyelerinin Numaralarını (y) giriniz.\nDeğerler nümerik ve 3'ten büyük olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                    y = ConsoleHelper.ReadLines(i => $"{attributeNames[i]} içn y >: ", maxLines: attributeNames.Count, validateLine: (x, i) => double.TryParse(x, out var x_) && x_ > 3, invalidLineMessage: (x, i) => "\tHATALI GİRİŞ!").Select(x => double.Parse(x)).ToArray();
                    ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
                }

                var decisionMatrix = new double[alternativeNames.Count, attributeNames.Count];
                Console.WriteLine("KARAR MATRİSİ");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine("Karar Matrisi'nin değerlerini giriniz.\nDeğerler nümerik olmalıdır.\nHer bir değeri onaylamak için Enter'a basınız.\n");
                for (int i = 0; i < alternativeNames.Count; i++)
                    for (int j = 0; j < attributeNames.Count; j++)
                        while (!double.TryParse(ConsoleHelper.ReadLine($"{alternativeNames[i]} ve {attributeNames[j]} için değer >: "), out decisionMatrix[i, j]))
                            Console.WriteLine("\tHATALI GİRİŞ!");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                Console.WriteLine("KARAR MATRİSİ İÇİN +/- DEĞERLERİ");
                ConsoleHelper.WriteHorizontalSeperator();
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

                ConsoleHelper.WriteAppInfo();
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

                CalculateWithPAMSSEM(alternativeNames, isOrdinal, Enumerable.Range(0, attributeNames.Count).Select(x => (attributeNames[x], weights[x], q[x], p[x], v[x], y?[x], isPositive[x])).ToArray(), decisionMatrix);
            }
        }
        private static void SolveProblemFromTheBook()
        {
            var alternatives = new[] { "A1", "A2", "A3" };
            var isOrdinal = true;
            var attributes = new List<(string Name, double Weight, double q, double p, double v, double? y, bool IsPositive)> {
                ("C1", 1D / 3D, 5D, 12D, 18D, null, false),
                ("C2", 1D / 3D, 15D, 25D, 32D, null, true),
                ("C3", 1D / 3D, 1D, 2D, 3D, null, true)
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
        private static void CalculateWithPAMSSEM(IList<string> alternatives, bool IsOrdinal, IList<(string Name, double Weight, double q, double p, double v, double? y, bool IsPositive)> attributes, double[,] decisionMatrix)
        {
            if ((alternatives?.Count ?? 0) == 0 || (attributes?.Count ?? 0) == 0 || decisionMatrix == null || decisionMatrix.GetLength(0) != alternatives.Count || decisionMatrix.GetLength(1) != attributes.Count || alternatives.Any(x => string.IsNullOrWhiteSpace(x)) || attributes.Any(x => string.IsNullOrWhiteSpace(x.Name) || x.y.HasValue == IsOrdinal || (x.y ?? 3) < 3))
                throw new ArgumentException();

            #region . Abstract .
            Console.WriteLine("ALTERNATİFLER");
            ConsoleHelper.WriteHorizontalSeperator();
            Console.WriteLine(string.Join("\n", alternatives));
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

            Console.WriteLine("ORDİNALLİK");
            ConsoleHelper.WriteHorizontalSeperator();
            Console.WriteLine(IsOrdinal ? "Ordinal" : "Kardinal");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);

            foreach (var attr in attributes)
            {
                Console.WriteLine($"ÖZELLİK:\t{attr.Name}");
                ConsoleHelper.WriteHorizontalSeperator();
                Console.WriteLine($"Ağırlık:\t{attr.Weight}");
                Console.WriteLine($"q:\t\t{attr.q}");
                Console.WriteLine($"p:\t\t{attr.p}");
                Console.WriteLine($"v:\t\t{attr.v}");
                ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            }

            Console.WriteLine("KARAR MATRİSİ");
            ConsoleHelper.WriteHorizontalSeperator();
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < attributes.Count; j++)
                    Console.WriteLine($"({(attributes[j].IsPositive ? "+" : "-")}) [{alternatives[i]}, {attributes[j].Name}] = {decisionMatrix[i, j]}");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            var differences = new double[alternatives.Count, alternatives.Count, attributes.Count];
            var indices = new double[alternatives.Count, alternatives.Count, attributes.Count];
            var Ds = new double[alternatives.Count, alternatives.Count, attributes.Count];
            var outrankingIndices = new double?[alternatives.Count, alternatives.Count];
            var concordanceIndices = new double?[alternatives.Count, alternatives.Count];
            var localDiscordanceIndices = new double?[alternatives.Count, alternatives.Count];
            var outrankingDegrees = new double?[alternatives.Count, alternatives.Count];
            var enteringFlows = new double[alternatives.Count];
            var leavingFlows = new double[alternatives.Count];
            var netFlows = new double[alternatives.Count];

            #region . Stage 1 .
            Console.WriteLine("AŞAMA 1 - YEREL GEÇİŞ İNDİSİ");
            ConsoleHelper.WriteHorizontalSeperator();
            if (IsOrdinal)
                for (int i = 0; i < alternatives.Count; i++)
                    for (int j = 0; j < alternatives.Count; j++)
                    {
                        Console.Write($"[{alternatives[i]}, {alternatives[j]}] = ");
                        if (i == j)
                        {
                            Console.WriteLine();
                            continue;
                        }
                        var sum = 0D;
                        for (int k = 0; k < attributes.Count; k++)
                        {
                            differences[i, j, k] = (decisionMatrix[i, k] - decisionMatrix[j, k]) * (attributes[k].IsPositive ? 1D : -1D);
                            if (differences[i, j, k] <= -attributes[k].p)
                                sum += indices[i, j, k] = 0D;
                            else if (differences[i, j, k] >= -attributes[k].q)
                                sum += indices[i, j, k] = 1D;
                            else if (-attributes[k].p < differences[i, j, k] - attributes[k].q && attributes[k].p >= attributes[k].q && attributes[k].q >= 0D)
                                sum += indices[i, j, k] = (differences[i, j, k] - attributes[k].p) / (attributes[k].p - attributes[k].q);
                            else
                            {
                                sum = indices[i, j, k] = double.NaN;
                                break;
                            }
                        }
                        Console.WriteLine(outrankingIndices[i, j] = sum);
                    }
            else
                for (int i = 0; i < alternatives.Count; i++)
                    for (int j = 0; j < alternatives.Count; j++)
                    {
                        Console.Write($"[{alternatives[i]}, {alternatives[j]}] = ");
                        if (i == j)
                        {
                            Console.WriteLine();
                            continue;
                        }
                        var sum = 0D;
                        for (int k = 0; k < attributes.Count; k++)
                        {
                            var diff = (decisionMatrix[i, k] - decisionMatrix[j, k]) * (attributes[k].IsPositive ? 1D : -1D);
                            if (diff >= 0D)
                                sum += indices[i, j, k] = 1D;
                            else if (diff >= -1D)
                                sum += indices[i, j, k] = .5;
                        }
                        Console.WriteLine(outrankingIndices[i, j] = sum);
                    }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Stage 2 .
            Console.WriteLine("AŞAMA 2 - AHENK İNDİSİ");
            ConsoleHelper.WriteHorizontalSeperator();
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < alternatives.Count; j++)
                {
                    Console.Write($"[{alternatives[i]}, {alternatives[j]}] = ");
                    if (!outrankingIndices[i, j].HasValue)
                    {
                        Console.WriteLine();
                        continue;
                    }
                    var sum = 0D;
                    for (int k = 0; k < attributes.Count; k++)
                        sum += attributes[k].Weight * indices[i, j, k];
                    Console.WriteLine(concordanceIndices[i, j] = sum);
                }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Stage 3 .
            Console.WriteLine("AŞAMA 3 - YEREL UYUMSUZLUK İNDİSİ");
            ConsoleHelper.WriteHorizontalSeperator();
            if (IsOrdinal)
                for (int i = 0; i < alternatives.Count; i++)
                    for (int j = 0; j < alternatives.Count; j++)
                    {
                        Console.Write($"[{alternatives[i]}, {alternatives[j]}] = ");
                        if (!outrankingIndices[i, j].HasValue)
                        {
                            Console.WriteLine();
                            continue;
                        }
                        var sum = 0D;
                        for (int k = 0; k < attributes.Count; k++)
                        {
                            if (differences[i, j, k] <= -attributes[k].v)
                                sum += Ds[i, j, k] = 1D;
                            else if (differences[i, j, k] >= -attributes[k].p)
                                sum += Ds[i, j, k] = 0D;
                            else
                                sum += Ds[i, j, k] = -(differences[i, j, k] + attributes[k].p) / (attributes[k].v - attributes[k].p);
                        }
                        Console.WriteLine(localDiscordanceIndices[i, j] = sum);
                    }
            else
                for (int i = 0; i < alternatives.Count; i++)
                    for (int j = 0; j < alternatives.Count; j++)
                    {
                        Console.Write($"[{alternatives[i]}, {alternatives[j]}] = ");
                        if (!outrankingIndices[i, j].HasValue)
                        {
                            Console.WriteLine();
                            continue;
                        }
                        var sum = 0D;
                        for (int k = 0; k < attributes.Count; k++)
                        {
                            var xiW = .2D * (1D + attributes[k].Weight / 2D);
                            var y = (attributes[k].y.Value + 1D) / 2D;
                            sum += Ds[i, j, k] = differences[i, j, k] < -y ? Math.Min(1D, xiW * differences[i, j, k] + y) : 0D;
                        }
                        Console.WriteLine(localDiscordanceIndices[i, j] = sum);
                    }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Stage 4 .
            Console.WriteLine("AŞAMA 4 - GEÇİŞ DERECESİ");
            ConsoleHelper.WriteHorizontalSeperator();
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < alternatives.Count; j++)
                {
                    Console.Write($"[{alternatives[i]}, {alternatives[j]}] = ");
                    if (!outrankingIndices[i, j].HasValue)
                    {
                        Console.WriteLine();
                        continue;
                    }
                    var product = concordanceIndices[i, j];
                    for (int k = 0; k < attributes.Count; k++)
                        product *= 1D - Math.Pow(Ds[i, j, k], 3D);
                    Console.WriteLine(outrankingDegrees[i, j] = product >= 0D && product <= 1D ? product : double.NaN);
                }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Stage 5 .
            Console.WriteLine("AŞAMA 5 - GİRİŞ VE ÇIKIŞ AKIŞLARI");
            ConsoleHelper.WriteHorizontalSeperator();
            for (int i = 0; i < alternatives.Count; i++)
            {
                double sum1 = 0D, sum2 = 0D;
                for (int j = 0; j < alternatives.Count; j++)
                {
                    sum1 += outrankingDegrees[i, j] ?? 0D;
                    sum2 += outrankingDegrees[j, i] ?? 0D;
                }
                Console.WriteLine($"+[{alternatives[i]}] = {enteringFlows[i] = sum1}");
                Console.WriteLine($"-[{alternatives[i]}] = {leavingFlows[i] = sum2}");
            }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Stage 6 .
            Console.WriteLine("AŞAMA 6 - NET AKIŞLAR");
            ConsoleHelper.WriteHorizontalSeperator();
            for (int i = 0; i < alternatives.Count; i++)
                Console.WriteLine($"[{alternatives[i]}] = {netFlows[i] = enteringFlows[i] - leavingFlows[i]}");
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Final Ranking: PAMSSEM I .
            Console.WriteLine("NİHAİ SIRALAMA: PAMSSEM I");
            ConsoleHelper.WriteHorizontalSeperator();
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = i; j < alternatives.Count; j++)
                {
                    bool? isP = null;
                    if (enteringFlows[i] == enteringFlows[j] && leavingFlows[i] == leavingFlows[j])
                        isP = false;
                    else if (
                        (enteringFlows[i] > enteringFlows[j] && leavingFlows[i] <= leavingFlows[j]) ||
                        (enteringFlows[i] == enteringFlows[j] && leavingFlows[i] < leavingFlows[j]))
                        isP = true;
                    Console.WriteLine("[{0} , {1}] : {2}", alternatives[i], alternatives[j], isP.HasValue ? (isP.Value ? "P" : "I") : "-");
                }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion

            #region . Final Ranking: PAMSSEM II .
            Console.WriteLine("NİHAİ SIRALAMA: PAMSSEM II");
            ConsoleHelper.WriteHorizontalSeperator();
            var netFlows_ = netFlows.Select((x, i) => (Index: i, Value: x));
            var dist = netFlows.Distinct().ToArray();
            if (dist.Length == 1)
                Console.WriteLine(string.Join(" I ", netFlows_.OrderBy(x => x.Index).Select(x => alternatives[x.Index])));
            else if (dist.Length == netFlows.Length)
                Console.WriteLine(string.Join(" > ", netFlows_.OrderByDescending(x => x.Value).Select(x => alternatives[x.Index])));
            else
            {
                var arr = netFlows_.ToArray();
                for (int i = 0; i < alternatives.Count; i++)
                {
                    var dic = arr.Where(x => x.Index > i).ToLookup(x => x.Value == arr[i].Value).ToDictionary(x => x.Key, x => x.ToArray());
                    if (dic.TryGetValue(true, out var I))
                        Console.WriteLine($"{alternatives[i]} I {string.Join(" I ", I.Select(x => alternatives[x.Index]))}");
                    if (dic.TryGetValue(false, out var P1))
                        Console.WriteLine(string.Join(" P ", P1.Concat(new[] { arr[i] }).OrderByDescending(x => x.Value).Select(x => alternatives[x.Index])));
                }
            }
            ConsoleHelper.WriteHorizontalSeperator(linesAfter: 3);
            #endregion
        }
    }
}