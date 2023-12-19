using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimizingComplexStatements_Lab3
{
    internal class Program
    {
        static string[,] GlueForm(ref string[,] array)
        {
            string[,] newGluing = new string[512, array.GetLength(1) - 1];
            int g = 0;
            List<int> indexes = new List<int> { };
            for (int i = 0; i < array.GetLength(0) - 1; i++)
            {
                string[] temp = new string[4];
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    temp[j] = array[i, j];
                }
                for (int k = i + 1; k < array.GetLength(0); k++)
                {
                    int countNotEquls = 0, countSymmetry = 0;
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (temp[j] != array[k, j])
                        {
                            countNotEquls++;
                            if (temp[j] == "-" + array[k, j] || "-" + temp[j] == array[k, j])
                                countSymmetry++;
                        }
                    }
                    if (countNotEquls == 1 && countSymmetry == 1)
                    {
                        if (!indexes.Contains(i))
                            indexes.Add(i);
                        if (!indexes.Contains(k))
                            indexes.Add(k);
                        int symbol = 0;
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            if (temp[j] == array[k, j])
                            {
                                newGluing[g, symbol] = temp[j];
                                symbol++;
                            }
                        }
                        g++;
                    }
                }
            }
            for (int i = 0; i < array.GetLength(0); ++i)
            {
                if (indexes.Contains(i))
                {
                    for (int j = 0; j < array.GetLength(1); ++j)
                    {
                        array[i, j] = "";
                    }
                }
            }
            DeleteEmptyStrings(ref array);
            return newGluing;
        }
        static void PrintArray(string[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                    if (array[i, j] != "")
                        Console.Write(array[i, j]);
                Console.WriteLine();
            }
        }
        static void DeleteEmptyStrings(ref string[,] array)
        {
            List<int> indexes = new List<int> { };
            for (int i = array.GetLength(0) - 1; i >= 0; i--)
            {
                if (array[i, 0] == "" || array[i, 0] == null)
                {
                    indexes.Add(i);
                }
            }
            if (indexes.Count != 1)
            {
                string[,] newArray = new string[array.GetLength(0) - indexes.Count, array.GetLength(1)];
                int k = 0;
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    if (!indexes.Contains(i))
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            newArray[k, j] = array[i, j];
                        }
                        k++;
                    }
                }
                array = newArray;
            }
        }
        static void DeleteEqualStrings(ref string[,] array)
        {
            List<int> indexes = new List<int> { };
            for (int i = 0; i < array.GetLength(0) - 1; ++i)
            {
                string tempString1 = "";
                for (int j = 0; j < array.GetLength(1); ++j)
                {
                    tempString1 += array[i, j];
                }
                for (int k = i + 1; k < array.GetLength(0); ++k)
                {
                    string tempString2 = "";
                    for (int l = 0; l < array.GetLength(1); ++l)
                    {
                        tempString2 += array[k, l];
                    }
                    if (tempString1 == tempString2)
                    {
                        if (!indexes.Contains(k))
                        {
                            indexes.Add(k);
                        }
                    }
                }
            }
            string[,] newArray = new string[array.GetLength(0) - indexes.Count, array.GetLength(1)];
            int newI = 0;
            for (int i = 0; i < array.GetLength(0); ++i)
            {
                if (!indexes.Contains(i))
                {
                    for (int j = 0; j < array.GetLength(1); ++j)
                    {
                        newArray[newI, j] = array[i, j];
                    }
                    ++newI;
                }
            }
            DeleteEmptyStrings(ref newArray);
            array = newArray;
        }
        static void PrintIntersections(string[,] mainArray, string[,] lessArray)
        {
            for (int i = 0; i < lessArray.GetLength(0); ++i)
            {
                int len = 0;
                for (int j = 0; j < lessArray.GetLength(1); ++j)
                {
                    Console.Write(lessArray[i, j]);
                    len += lessArray[i, j].Length;
                }
                for (int k = 0; k < mainArray.GetLength(0); ++k)
                {
                    if (len < 8 || k != 0)
                        Console.Write("\t");
                    string mainString = "";
                    bool IsConsists = true;
                    for (int l = 0; l < mainArray.GetLength(1); ++l)
                    {
                        mainString += mainArray[k, l];
                    }
                    for (int p = 0; p < lessArray.GetLength(1); ++p)
                    {
                        if (!mainString.Contains(lessArray[i, p]) || mainString.Contains('-' + lessArray[i, p]))
                            IsConsists = false;
                    }
                    if (IsConsists)
                    {
                        Console.Write("|   +");
                    }
                    else
                        Console.Write('|');
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите вектор значений xyzw (16 значений):");
            string msg = Console.ReadLine();
            int[] func = new int[16];
            int count1 = 0;
            for (int i = 0; i < msg.Length && i < 16; i++)
            {
                if (msg[i] == '0')
                    func[i] = 0;
                else
                    if (msg[i] == '1')
                {
                    func[i] = 1;
                    ++count1;
                }
            }

            // Таблица истинности
            Console.WriteLine("\tx\ty\tz\tw\tf");
            for (int i = 0; i < func.Length; i++)
            {
                int j = i;
                int i8 = j / 8;
                if (i8 == 1)
                    j -= 8;
                int i4 = j / 4;
                if (i4 == 1)
                    j -= 4;
                int i2 = j / 2;
                if (i2 == 1)
                    j -= 2;
                int i1 = j % 2;
                Console.WriteLine($"{i}\t{i8}\t{i4}\t{i2}\t{i1}\t{func[i]}");
            }

            string PDNF = ""; // СДНФ
            string[,] gluing4 = new string[32, 4];
            string[,] copyGluing4 = new string[32, 4];
            int g4 = 0;
            for (int i = 0; i < func.Length; i++)
            {
                if (func[i] == 1)
                {
                    if (PDNF != "")
                        PDNF += " v ";
                    string x, y, z, w;
                    int j = i;
                    int i8 = j / 8;
                    if (i8 == 1)
                    {
                        j -= 8;
                        x = "X";
                    }
                    else
                        x = "-X";
                    int i4 = j / 4;
                    if (i4 == 1)
                    {
                        j -= 4;
                        y = "Y";
                    }
                    else
                        y = "-Y";
                    int i2 = j / 2;
                    if (i2 == 1)
                    {
                        j -= 2;
                        z = "Z";
                    }
                    else
                        z = "-Z";
                    int i1 = j % 2;
                    if (i1 == 1)
                        w = "W";
                    else
                        w = "-W";
                    PDNF += x + y + z + w;
                    gluing4[g4, 0] = x;
                    gluing4[g4, 1] = y;
                    gluing4[g4, 2] = z;
                    gluing4[g4, 3] = w;
                    copyGluing4[g4, 0] = x;
                    copyGluing4[g4, 1] = y;
                    copyGluing4[g4, 2] = z;
                    copyGluing4[g4, 3] = w;
                    ++g4;
                }
            }
            if (func.Sum() != 0)
            {
                Console.WriteLine($"\nСДНФ\n{PDNF}");
                DeleteEmptyStrings(ref gluing4);
                DeleteEmptyStrings(ref copyGluing4);
            }
            else
            {
                Console.WriteLine("СДНФ не существует!");
                return;
            }

            string[,] gluing3 = GlueForm(ref copyGluing4);
            DeleteEmptyStrings(ref gluing3);
            if (gluing3.Length != 0)
            {
                Console.WriteLine($"\nСклейка\n");
                PrintArray(gluing3);
                Console.WriteLine();
            }
            string[,] gluing2 = GlueForm(ref gluing3);
            DeleteEmptyStrings(ref gluing2);
            if (gluing2.Length != 0)
            {
                PrintArray(gluing2);
                Console.WriteLine();
            }
            string[,] gluing1 = GlueForm(ref gluing2);
            DeleteEmptyStrings(ref gluing1);
            if (gluing1.Length != 0)
            {
                PrintArray(gluing1);
                Console.WriteLine();
            }

            if (gluing3.Length != 0)
            {
                Console.WriteLine($"\nУдаление дубликатов\n");
                DeleteEqualStrings(ref gluing3);
                PrintArray(gluing3);
                Console.WriteLine();
            }
            if (gluing2.Length != 0)
            {
                DeleteEqualStrings(ref gluing2);
                PrintArray(gluing2);
                Console.WriteLine();
            }
            if (gluing1.Length != 0)
            {
                DeleteEqualStrings(ref gluing1);
                PrintArray(gluing1);
                Console.WriteLine();
            }
            if (gluing3.Length + copyGluing4.Length + gluing2.Length + gluing1.Length != 0)
            {
                Console.Write($"\nИмпликантная матрица\n\t");
                for (int i = 0; i < gluing4.GetLength(0); ++i)
                {
                    int len = 0;
                    for (int j = 0; j < gluing4.GetLength(1); ++j)
                    {
                        Console.Write(gluing4[i, j]);
                        len += gluing4[i, j].Length;
                    }
                    if (len < 8)
                        Console.Write("\t");
                }
                Console.WriteLine();
                PrintIntersections(gluing4, copyGluing4);
                PrintIntersections(gluing4, gluing3);
                PrintIntersections(gluing4, gluing2);
                PrintIntersections(gluing4, gluing1);
            }
        }
    }
}
