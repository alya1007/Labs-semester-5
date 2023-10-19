using System;
using System.Text;
using System.Linq;
using lab3;

class Program
{
    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;
        bool exit = false;
        while (!exit)
        {

            Console.WriteLine("Vigenere Cipher (Romanian Alphabet)");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("e - Encrypt\nd - Decrypt\nq - Quit");

            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choice)
            {
                case 'e':
                    EncryptText();
                    break;
                case 'd':
                    DecryptText();
                    break;
                case 'q':
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Use 'e' for encryption or 'd' for decryption.");
                    break;
            }
        }
    }

    static void EncryptText()
    {
        Console.Write("Enter the text to be encrypted (only letters): ");
        string? plaintext = GetValidInput();
        if (plaintext == null) return;

        Console.Write($"Enter the key (word or sentence, length 6 to {plaintext.Length}): ");
        string? key = GetValidInput();
        if (key == null || key.Length < 6 || key.Length > plaintext.Length) return;

        string ciphertext = VigenereCipher.Encrypt(plaintext, key);
        Console.WriteLine("Encrypted text: " + ciphertext);
    }

    static void DecryptText()
    {
        Console.Write("Enter the text to be decrypted (only letters): ");
        string? ciphertext = GetValidInput();
        if (ciphertext == null) return;

        Console.Write($"Enter the key (word or sentence, length 6 to {ciphertext.Length}): ");
        string? key = GetValidKey();
        if (key == null || key.Length < 6 || key.Length > ciphertext.Length) return;

        string plaintext = VigenereCipher.Decrypt(ciphertext, key);
        Console.WriteLine("Decrypted text: " + plaintext);
    }

    static string? GetValidInput()
    {
        string? input = Console.ReadLine();
        if (input is null) return null;
        else
            input = input.ToUpper().Replace(" ", ""); ;
        if (!IsAllLetters(input))
        {
            Console.WriteLine("Invalid input. Text should only contain letters.");
            return null;
        }
        return input;
    }

    static string? GetValidKey()
    {
        string? key = Console.ReadLine();
        if (key is null) return null;
        else
            key = key.ToUpper().Replace(" ", ""); ;
        if (!IsAllLetters(key))
        {
            Console.WriteLine("Invalid input. Key should only contain letters.");
            return null;
        }
        return key;
    }

    static bool IsAllLetters(string text)
    {
        return text.All(c => VigenereCipher.RomanianAlphabet.Contains(c));
    }
}
