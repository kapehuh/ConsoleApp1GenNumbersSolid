using System.Net.Http;

internal class Program
{
    private static void Main(string[] args)
    {
        int tryCount = 0; int diap = 0; int younum = 0; int orignum = 0;
        do
        {
            Console.Write("Задайте диапазон поиска: число от нуля до... = ");
        } while (!int.TryParse(Console.ReadLine(), out diap));
        do
        {
            Console.Write("Задайте количество попыток: ... = ");
        } while (!int.TryParse(Console.ReadLine(), out tryCount));

        //MAIN ===================================================================================
        My_IntNumGen mn = new My_IntNumGen(new MyIntNumGen());
        mn.GetMyNum(diap); orignum = mn.iinumber;
        ReturnAnswerMessageClass messageClass = new ReturnAnswerMessageClass(mn);
        // =======================================================================================

        Console.WriteLine($"Угадайте число от 0 до {diap}, у вас {tryCount} попыток:");
        for (int i = 1; i <= tryCount; i++)
        {
            while (!int.TryParse(Console.ReadLine(), out younum))
            {
                Console.WriteLine("Число не распознано, повторите попытку:");
            }
            if (younum == orignum)
            {
                Console.WriteLine($"{messageClass.ReturnAnswer()} - ты справился с {i}й попытки!"); return;
            }
            else
            {
                if (younum < orignum)
                {
                    Console.WriteLine($"Неверно, загаданное число больше, чем {younum}:");
                    Console.Write($"Следующая попытка: ");
                }
                else
                {
                    Console.WriteLine($"Неверно, загаданное число меньше, чем {younum}:");
                    Console.Write($"Следующая попытка: ");
                }
            }
        }
        Console.WriteLine($"\nВсе попытки исчерпаны, ты не справился. Загаданное число = {orignum}");
    }
}

// ========================================================================================================================
// ========================================================================================================================
// ========================================================================================================================
// ========================================================================================================================
// ========================================================================================================================

/// <summary>
/// Базовый интерфейс генерации числа.
/// </summary>
internal interface IIntNumGen
{
    int GetUncknownNum(int i);
}
/// <summary>
/// Interface Segregation principle - отделяем интерфейс, который проверяет число от интерфейса, который
/// генерирует число...
/// </summary>
internal interface IIntNumCheck
{
    bool CheckMyNum(int i);
}
/// <summary>
/// Аналогичный предыдущему интерфейс для демонстрации LSP, возращающий строку
/// </summary>
internal interface IIntNumCheckWithMessage
{
    string CheckMyNum(int i);
}



/// <summary>
/// Реализация для int_овой генерации (может в дальнейшем потребуется double)
/// </summary>
internal class MyIntNumGen : IIntNumGen
{
    public int GetUncknownNum(int i) => new Random().Next(i);
}




/// <summary>
/// Класс, который генерирует число
/// </summary>
internal class My_IntNumGen
{
    private IIntNumGen _num; //OpenClosed - My_IntNumGen "открыт для расширения, закрыт для изменения"
    public int iinumber { get; set; }
    public My_IntNumGen(IIntNumGen num)
    {
        _num = num;
    }
    public void GetMyNum(int i)
    {
        iinumber = _num.GetUncknownNum(i);
    }    
}




/// <summary>
/// для DIP - класс My_IntNumCheck наследуем от интерфейса IIntNumCheck, чтобы "детали зависели от абстракций" ?!
/// </summary>
internal class My_IntNumCheck : IIntNumCheck
{
    private My_IntNumGen myIntNumGen;
    public My_IntNumCheck(My_IntNumGen m)
    {
        myIntNumGen = m;
    }
    public bool CheckMyNum(int n)
    {
        if (myIntNumGen.iinumber == n)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/// <summary>
/// LSP - есть интерфейс IIntNumCheck, возвращающий bool, при проверке числа
/// Есть второй интерфейс IIntNumCheckWithMessage, возвращающий string, при проверке числа
/// Данный класс, наследует оба. My_IntNumCheck - только один.
/// Тут, вероятно, также прослеживается InterfaceSegregationPrinciple.
/// </summary>
internal class My_IntNumCheckWithMessage : IIntNumCheck, IIntNumCheckWithMessage
{
    private My_IntNumGen myIntNumGen;

    public My_IntNumCheckWithMessage(My_IntNumGen m)
    {
        myIntNumGen = m;
    }
    public bool CheckMyNum(int n)
    {
        if (myIntNumGen.iinumber == n)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    string IIntNumCheckWithMessage.CheckMyNum(int i)
    {
        if (myIntNumGen.iinumber == i)
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }
}






// === === === === === === === === === === === === === === === === === === === === === === === === === === === === === ===
internal class ReturnAnswerMessageClass
{
    private My_IntNumGen _MyIntNumGen;
    private My_IntNumCheck _MyIntNumCheck;
    public ReturnAnswerMessageClass(My_IntNumGen num)
    {
        _MyIntNumGen = num;
        _MyIntNumCheck = new My_IntNumCheck(_MyIntNumGen);
    }
    public string ReturnAnswer()
    {
        if (_MyIntNumCheck.CheckMyNum(_MyIntNumGen.iinumber))
        {
            return "Победа!";
        }
        else
        {
            return "loose";
        }
    }
}



