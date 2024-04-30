# Rospred

### Наши направления
- [Backend](#Backend)
- [Frontend](#Frontend)

## Backend

Весь бэк реализован в проектах [WebApi](#WebApi), [Python processor](#Python-Processor) и [ML](#ML) с использованием следующих технологий:

### WebApi
[Код](Backend/Backend.Web)

Бэк веба реализован на ASP.Net с базой данных SQLite.

Вот некоторые запросы к серверу

![image](https://github.com/aexra/Rospred/assets/121866384/488bff53-be34-4bf7-95d9-18d373ca03f7)

### Python processor
[Код](Backend/Backend.Processor)

Т.к. мы взяли за серверный бэк ASP.Net на C#, то надо как-то запускать из него ML на Python.

Было решено сделать это при помощи процессов, обернутых в класс IOProcess (ниже) для удобства использования stdin и stdout:
вызывается командная строка с аргументами для запуска файла python с дополнительными аргументами

Метод вызова процесса
```cs
public static class IOProcess
{
    public static IOProcessResult Run(string cmdpromt)
    {
        var proc = new Process();

        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = "/c" + cmdpromt;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;

        proc.Start();

        var output = proc.StandardOutput.ReadToEnd();
        var error = proc.StandardError.ReadToEnd();

        proc.WaitForExit();

        return new() { ExitCode = proc.ExitCode, Output = proc.ExitCode == 0? output : "Subprocess error: \t" + error };
    }
}
```

Пример вызова процесса Python
```cs
using Backend.Processor;
Console.WriteLine(IOProcess.Run("python ../Backend.ML/Testing/test.py Hello, web!").Output);
```

### ML

Большая часть работы с ML была проведена в Google.Colab, выгруженные оттуда ноутбуки можно просмотреть [здесь](Backend/Backend.ML/Jupyther)

[Код](Backend/Backend.ML)


## Frontend

### Дизайн

![Landing (3)](https://github.com/aexra/Rospred/assets/121866384/b1bc29f2-3f9f-471f-891a-619c9abe1ee7)
