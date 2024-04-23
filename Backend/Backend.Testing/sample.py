# -*- coding: cp1251 -*-
import sys

# Это список всех аргументов коммандной строки без названия файла (stdin)
args: list[str] = sys.argv[1:]

# Вывод в stdout
print("Echo: ", *args)

# Если в коде питона возникнет ошибка, она сама попадет в stderr и IOProcess.cs ее обработает

