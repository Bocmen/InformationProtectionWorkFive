using ConsoleLibrary.ConsoleExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace InformationProtectionWorkFiveShare
{
    [WorkInvoker.Attributes.LoaderWorkBase("Задание 2", "", Const.NameGroup)]
    public class WorkTwo : WorkInvoker.Abstract.WorkBase
    {
        private int n, c, e, p, q, d, el;
        public override async Task Start(CancellationToken token)
        {
            n = await Console.ReadInt("Введите N", token: token, defaultValue: 391);
            c = await Console.ReadInt("Введите C", token: token, defaultValue: 114);
            e = await Console.ReadInt("Введите E", token: token, defaultValue: 249);
            if (n <= 0) return;
            for (int i = 2; i < n; i++)
            {
                if (n % i == 0)
                {
                    p = i;
                    q = n / p;
                    break;
                }
            }
            el = (p - 1) * (q - 1);
            while (e * (++d) % el != 1) ;
            Console.StartCollectionDecorate();
            await Console.WriteLine($"Найденные p и q\np = {p}\nq = {q}");
            await Console.WriteLine($"Функция Эйлера: \\(\\varphi(n) = (p - 1) * (q - 1) = ({p} - 1) * ({q} - 1) = {el}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            await Console.WriteLine($"Найдено d которое\n\\((d * e)\\ mod\\ \\varphi(n) == 1\\)\n\\(({d} * {e}\\ mod\\ {el} == 1)\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            var dec = Decrypt(c);
            await Console.WriteLine($"Расшифрованное сообщение: \\(M = {c}^{{{d}}}\\ mod\\ {n} = {dec}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            var enc = Encrypt(dec);
            await Console.WriteLine($"Зашифрованное сообщение: \\(M = {dec}^{{{e}}}\\ mod\\ {n} = {enc}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
            while (!token.IsCancellationRequested)
            {
                Console.StartCollectionDecorate();
                if (await Console.ReadBool("Хотите зашифровать значение"))
                {
                    int val = await Console.ReadInt("Введите значение", token: token, defaultValue: 33);
                    enc = Encrypt(val);
                    await Console.WriteLine($"Зашифрованное сообщение: \\(M = {dec}^{{{e}}}\\ mod\\ {n} = {enc}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
                    await Console.WriteLine($"Исходное сообщение: \\(M = {c}^{{{d}}}\\ mod\\ {n} = {Decrypt(enc)}\\)", ConsoleIOExtension.TextStyle.UseLaTeX);
                }
                else
                    break;
            }
        }
        private int Encrypt(int val) => WorkOne.Pow(val, d, n);
        private int Decrypt(int val) => WorkOne.Pow(val, e, n);
    }
}
