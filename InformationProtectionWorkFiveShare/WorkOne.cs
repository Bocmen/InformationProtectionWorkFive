using ConsoleLibrary.ConsoleExtensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InformationProtectionWorkFiveShare
{
    [WorkInvoker.Attributes.LoaderWorkBase("Задание 1", "", Const.NameGroup)]
    public class WorkOne : WorkInvoker.Abstract.WorkBase
    {
        public override async Task Start(CancellationToken token)
        {
            int p = await Console.ReadInt("Введите P", token: token, defaultValue: 37);
            int a = GetAlpha(p, token);
            await Console.WriteLine($"Найден следующий первый примитивный элемент {a}");
            int secA = await Console.ReadInt("Секретный ключ участника A", defaultValue: 15, token: token);
            int secB = await Console.ReadInt("Секретный ключ участника B", defaultValue: 20, token: token);

            int keyA = Pow(a, secA, p);
            int keyB = Pow(a, secB, p);

            int keyShareA = Pow(keyB, secA, p);
            int keyShareB = Pow(keyA, secB, p);


            Console.StartCollectionDecorate();
            await Console.WriteLine("Открытые ключи участников", ConsoleIOExtension.TextStyle.IsTitle);
            await Console.WriteLine($"\\(K_a = \\alpha^{{X_A}} = \\alpha^{{{secA}}} = {a}^{{{secA}}} mod({p}) = {keyA}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            await Console.WriteLine($"\\(K_a = \\alpha^{{X_B}} = \\alpha^{{{secB}}} = {a}^{{{secB}}} mod({p}) = {keyB}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            Console.StartCollectionDecorate();
            await Console.WriteLine("Обменные ключи", ConsoleIOExtension.TextStyle.IsTitle);
            await Console.WriteLine($"\\(K = K^{{X_A}}_{{B}} mod \\alpha = {keyB}^{{{secA}}} mod({p}) = {keyShareA}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            await Console.WriteLine($"\\(K = K^{{X_B}}_{{A}} mod \\alpha = {keyA}^{{{secB}}} mod({p}) = {keyShareB}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            await Console.WriteLine($"Значения обменного ключа для A и B {(keyShareA == keyShareB ? string.Empty : "не")} совпадают", keyShareA == keyShareB ? ConsoleIOExtension.TextStyle.None : ConsoleIOExtension.TextStyle.IsError);
        }
        public static int Pow(int basis, int degree, int mod)
        {
            int result = basis;
            for (int i = 1; i < degree; i++)
                result = result * basis % mod;
            return result;
        }

        private int GetAlpha(int p, CancellationToken token)
        {
            for (int i = 2; i < p; i++)
            {
                if (IsAlpha(i, p, token))
                    return i;
            }
            throw new Exception("Не удалось найти примитивный элемент поля");
        }
        private bool IsAlpha(int a, int p, CancellationToken token)
        {
            HashSet<int> foundElems = new HashSet<int>();
            int lastVal = a;
            foundElems.Add(lastVal);
            for (int i = 2; i < p && !token.IsCancellationRequested; i++)
            {
                lastVal = lastVal * a % p;
                if (!foundElems.Add(lastVal))
                    return false;
            }
            return true;
        }
    }
}
