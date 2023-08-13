using System;
using System.Threading;

namespace Calculadora_de_Sudoku;

class Program
{
    static void Main(string[] args)
    {
        int[,] tabela = {
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        Console.WriteLine("Para começar, insira os números presentes na tabela da seguinte forma: \n");
        
        bool inserirNovoNumero = true;

        do
        {
            string resposta;
            ImprimeTabela(tabela);

            Console.Write("\nDigite a linha do número: ");
            resposta = Console.ReadLine();

            if (!ConfereEntradaLinhaEColuna(resposta))
            {
                Thread.Sleep(2000);
                Console.Clear();
                continue;
            }

            int coordenadaY = LetraParaNumero(Char.ToUpper(Convert.ToChar(resposta)));

            Console.Write("Digite a coluna do número: ");
            resposta = Console.ReadLine();

            if (!ConfereEntradaLinhaEColuna(resposta))
            {
                Thread.Sleep(2000);
                Console.Clear();
                continue;
            }

            int coordenadaX = LetraParaNumero(Char.ToUpper(Convert.ToChar(resposta)));

            Console.Write("Digite o número: ");
            resposta = Console.ReadLine();

            if (!ConfereEntradaNumero(resposta))
            {
                Thread.Sleep(2000);
                Console.Clear();
                continue;
            }

            int numero = Convert.ToInt32(resposta);
            tabela[coordenadaY, coordenadaX] = numero;

            do
            {
                Console.Write("Deseja inserir outro número? (S/N): ");
                resposta = Console.ReadLine();

            } while (!ConfereEntradaNovoNumero(resposta));
            inserirNovoNumero = Char.ToUpper(Convert.ToChar(resposta)) == 'S';

            Console.Clear();

        }while (inserirNovoNumero);

        if (ResolveSudoku(tabela))
        {
            ImprimeTabela(tabela);
        }
        else
        {
            Console.WriteLine("Sem solução!");
        }
        
        Console.ReadKey();
    }

    static bool ConfereEntradaLinhaEColuna (string resposta)
    {
        if (string.IsNullOrEmpty(resposta) || resposta.Length != 1 || !char.IsLetter(resposta[0]))
        {
            Console.WriteLine("\nPor favor, digite um caractere.");
            return false;
        }

        char maiuscula = char.ToUpper(resposta[0]);

        if (maiuscula < 'A' || maiuscula > 'I')
        {
            Console.WriteLine("\nDigite uma letra de A a I.");
            return false;
        }

        return true;
    }

    static bool ConfereEntradaNumero (string resposta)
    {
        if (string.IsNullOrEmpty(resposta) || resposta.Length != 1 || !char.IsNumber(resposta[0]))
        {
            Console.WriteLine("\nPor favor, digite um número.");
            return false;
        }

        int numero = Convert.ToInt32(resposta);

        if (numero < 1 || numero > 9)
        {
            Console.WriteLine("\nPor favor, digite um número entre 1 e 9.");
            return false;
        }

        return true;
    }

    static bool ConfereEntradaNovoNumero (string resposta)
    {
        if (string.IsNullOrEmpty(resposta) || resposta.Length != 1 || !char.IsLetter(resposta[0]))
        {
            Console.WriteLine("\nPor favor, digite um caractere.");
            return false;
        }

        char maiuscula = char.ToUpper(resposta[0]);

        if (maiuscula != 'S' && maiuscula != 'N')
        {
            Console.WriteLine("\nDigite 'S' ou 'N'.");
            return false;
        }

        return true;
    }

    static void ImprimeTabela (int[,] tabela)
    {
        char coordenadaY = 'A';
        for (int i = 0; i < 9; i++)
        {
            if (i == 0)
            {
                Console.WriteLine("  A B C   D E F   G H I");
            }

            if (i % 3 == 0 && i != 0)
            { 
                Console.WriteLine("  ---------------------");
            }

            for (int j = 0; j < 9; j++)
            {
                if (j == 0)
                {
                    Console.Write($"{coordenadaY} ");
                    coordenadaY++;
                }

                if (j % 3 == 0 && j != 0)
                {
                    Console.Write("| ");
                }
                Console.Write($"{tabela[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    static int LetraParaNumero (char letra)
    {
        int coordenadaConvertida = 0;

        switch (letra)
        {
            case 'A':
                coordenadaConvertida = 0;
                break;
            case 'B':
                coordenadaConvertida = 1;
                break;
            case 'C':
                coordenadaConvertida = 2;
                break;
            case 'D':
                coordenadaConvertida = 3;
                break;
            case 'E':
                coordenadaConvertida = 4;
                break;
            case 'F':
                coordenadaConvertida = 5;
                break;
            case 'G':
                coordenadaConvertida = 6;
                break;
            case 'H':
                coordenadaConvertida = 7;
                break;
            case 'I':
                coordenadaConvertida = 8;
                break;
        }
        return coordenadaConvertida;
    }

    static bool ResolveSudoku (int[,] tabela)
    { 
        int linha, coluna;
        if (!TemCelulaVazia(tabela, out linha, out coluna))
        {
            return true; //todas as células foram preenchidas
        }

        for (int numero = 1; numero <= 9; numero++)
        {
            if (PodeColocarNumero(tabela, linha, coluna, numero))
            {
                tabela[linha, coluna] = numero;

                if (ResolveSudoku(tabela))
                {
                    return true;
                }
                tabela[linha, coluna] = 0; //coloca zero na célula caso chegue num beco sem saída
            }
        }
        return false; //não há soluções para o sudoku
    }

    static bool TemCelulaVazia (int[,] tabela, out int linha, out int coluna)
    {
        for (linha = 0; linha < 9; linha++)
        {
            for (coluna = 0; coluna < 9; coluna++)
            {
                if (tabela[linha, coluna] == 0)
                {
                    return true;
                }
            }
        }

        linha = -1;
        coluna = -1;
        return false;
    }

    static bool PodeColocarNumero(int[,] tabela, int linha, int coluna, int numero)
    {
        //checar a linha e coluna
        for (int i = 0; i < 9; i++)
        {
            if (tabela[linha, i] == numero || tabela[i, coluna] == numero)
            {
                return false;
            }
        }

        //checar setor 3x3
        int linhaInicial = linha - linha % 3;
        int colunaInicial = coluna - coluna % 3;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tabela[i + linhaInicial, j + colunaInicial] == numero)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
