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

![image](https://github.com/aexra/Rospred/assets/121866384/457e3be2-cc36-4c69-9369-14675511af31)

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

Скрипт python используемый для получения прогноза на обученных моделях
```py
import sys
import pickle
import normalizer

args = sys.argv[1:]

label = " ".join(args[:-2])
model_name = args[-2]
prediction_horizon = int(args[-1])

with open(f'../Backend.ML/Models/trained_{model_name}_models.pkl', 'rb') as f:
  models = pickle.load(f)
  model = models[label]
  forecast = model.forecast(steps=prediction_horizon)
  forecasted_values = forecast.values.tolist()
  denormalized = normalizer.denormalize(labels.index(label), forecasted_values, 331)
  print(denormalized)
```

### ML

Большая часть работы с ML была проведена в Google.Colab, выгруженные оттуда ноутбуки можно просмотреть [здесь](Backend/Backend.ML/Jupyther)

[Код](Backend/Backend.ML)


## Frontend

### Дизайн

![Landing - Administrator](https://github.com/aexra/Rospred/assets/121866384/059f87f3-02f4-4873-8b80-3da76e75868c)

![ЦУРы - Administrator](https://github.com/aexra/Rospred/assets/121866384/98e693a8-a6f4-44f8-bbfe-c3f5549cd73a)

![Control Panel - Administrator](https://github.com/aexra/Rospred/assets/121866384/40283549-53dd-4344-b0ec-b5dd35d813d7)
