namespace lab3;

public static class VigenereCipher
{
    public static string RomanianAlphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ";
    public static string Encrypt(string text, string key)
    {
        string result = "";
        int textLength = text.Length;
        int keyLength = key.Length;

        for (int i = 0; i < textLength; i++)
        {
            char textChar = text[i];
            if (RomanianAlphabet.Contains(textChar))
            {
                char keyChar = key[i % keyLength];
                int shift = (RomanianAlphabet.IndexOf(textChar) + RomanianAlphabet.IndexOf(keyChar)) % RomanianAlphabet.Length;
                result += RomanianAlphabet[shift];
            }
            else
            {
                result += textChar;
            }
        }

        return result;
    }

    public static string Decrypt(string text, string key)
    {
        string result = "";
        int textLength = text.Length;
        int keyLength = key.Length;

        for (int i = 0; i < textLength; i++)
        {
            char textChar = text[i];
            if (RomanianAlphabet.Contains(textChar))
            {
                char keyChar = key[i % keyLength];
                int shift = (RomanianAlphabet.IndexOf(textChar) - RomanianAlphabet.IndexOf(keyChar) + RomanianAlphabet.Length) % RomanianAlphabet.Length;
                result += RomanianAlphabet[shift];
            }
            else
            {
                result += textChar;
            }
        }

        return result;
    }
}