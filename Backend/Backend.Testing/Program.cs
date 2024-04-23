using Backend.Processor;

// Работая из простой библиотеки классов, весь проект питона необходимо копировать в выходной каталог
Console.WriteLine(IOProcess.Run("python ML/Testing/test.py Hello, world!").Output);

// Работая из ASP.Net достаточно указывать косвенный путь от проекта ASP.Net, не копируя в выходной каталог
//Console.WriteLine(IOProcess.Run("python ../Backend.ML/Testing/test.py Hello, web!").Output);
