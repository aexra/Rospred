# -*- coding: cp1251 -*-
import sys

# ��� ������ ���� ���������� ���������� ������ ��� �������� ����� (stdin)
args: list[str] = sys.argv[1:]

# ����� � stdout
print("Echo: ", *args)

# ���� � ���� ������ ��������� ������, ��� ���� ������� � stderr � IOProcess.cs �� ����������

