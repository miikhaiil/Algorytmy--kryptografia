using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kryptografia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public string railFanceCipher(string text, int level)
        {
            int n = level;
            int direction = 1;

            int count = 0;
            char[,] tab = new char[n, text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                if (count + direction == n)
                {
                    direction = -1;
                }
                else if (count + direction == -1)
                {
                    direction = 1;
                }
                tab[count, i] = text[i];
                count = count + direction;
            }

            string cipher = "";
            foreach (char a in tab)
            {
                if (isCorrect(a))
                {
                    cipher = cipher + a;
                }
            }
            return cipher;
        }

        public string decipherRailFence(string word, int level)
        {
            int count = 0;
            int direction = 1;
            int n = level;
            char[,] tab = new char[n, word.Length];
            string decipher ="";
            for (int i = 0; i < word.Length; i++)
            {
                if (count + direction == n)
                {
                    direction = -1;
                }
                else if (count + direction == -1)
                {
                    direction = 1;
                }
                tab[count, i] = '*';
                count = count + direction;
            }
            count = 0;
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if(tab[j, i] == '*')
                    {
                        tab[j, i] = word[count++];
                    }
                }
            }
            count =0;
            for (int i = 0; i < word.Length; i++)
            {
                if (count + direction == n)
                {
                    direction = -1;
                }
                else if (count + direction == -1)
                {
                    direction = 1;
                }
                decipher += tab[count, i];
                count = count + direction;
            }
            return decipher;
        }

        public bool isCorrect(char a)
        {
            if (a >= 'a' && a <= 'z' || a >= 'A' && a <= 'Z')
            {
                return true;
            }
            else
                return false;
        }

        public string matrixCipherA(string text)
        {
            int[] code = { 3, 4, 1, 5, 2 };
         
            string cipher = "";
            int n;
            if (text.Length % 5 == 0)
            {
                n = text.Length / 5;
            }
            else
            {
                n = (text.Length / 5) + 1;
            }
            char[,] tab = new char[n, 5];
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    tab[i, j] = text[count++];
                    if (count > text.Length-1) 
                        break;
                }
            }
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < code.Length; j++)
                {
                    if (isCorrect(tab[i, code[j]-1]))
                    {
                        cipher += tab[i, code[j] - 1];
                    }
                }
            }
            return cipher;
        }

        public string decipherMatrixA(string word)
        {
            string decipher = "";
            int[] code = { 3, 4, 1, 5, 2 };
            int n;
            if (word.Length % 5 == 0)
            {
                n = word.Length / code.Length;
            }
            else
            {
                n = (word.Length / code.Length) + 1;
            }
            char[,] tab = new char[n, code.Length];
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < code.Length; j++)
                {
                    tab[i, j] = '*';
                    count++;
                    if (count > word.Length - 1)
                        break;
                }
            }
            count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < code.Length; j++)
                {
                    if(tab[i, code[j] - 1] == '*')
                    {
                        tab[i, code[j] - 1] = word[count++];
                    }
                    if (count > word.Length - 1)
                        break;
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < code.Length; j++)
                {
                    if (isCorrect(tab[i, j]))
                    {
                        decipher += tab[i, j];
                    }
                }
            }
            return decipher;
        }

        public int[] getKeyMatrixCipherB(string text)
        {
            int[] map = new int['Z' - 'A' + 1];
            int[] sum = new int['Z' - 'A' + 1];
            int[] mapCount = new int['Z' - 'A' + 1];
            int[] key = new int[text.Length];
            foreach (var a in text)
            {
                map[a - 'A'] += 1;
            }
            sum[0] = map[0];
            for (int i = 1; i < 'Z' - 'A' + 1; i++)
            {
                sum[i] = sum[i - 1] + map[i];
            }

            for (int i = 0; i < text.Length; i++)
            {
                key[i] = (text[i] == 'A')? mapCount[text[i] - 'A'] : sum[text[i] - 'A' - 1] + mapCount[text[i] - 'A'];
                mapCount[text[i] - 'A']++;
            }
            return key;
        }

        public string matrixCipherB(string text, string code)
        {
            string cipher = "";
            var key = getKeyMatrixCipherB(code);
            int[] indexes = new int[key.Length];
            for(int i = 0; i < key.Length; i++)
            {
                indexes[key[i]] = i;
            }
            int n;
            if (text.Length % code.Length == 0)
            {
                n = text.Length / key.Length;
            }
            else
            {
                n = (text.Length / key.Length) + 1;
            }
            char[,] tab = new char[n, key.Length];
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    tab[i, j] = text[count++];
                    if (count > text.Length - 1)
                        break;
                }
            }
            for (int j = 0; j < indexes.Length; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    {
                        if (isCorrect(tab[i, indexes[j]]))
                        {
                            cipher += tab[i, indexes[j]];
                        }
                    }
                }
            }
            return cipher;
        }

        public string matrixDecipherB(string text, string code)
        {
            string decipher = "";
            var key = getKeyMatrixCipherB(code);
            int[] indexes = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                indexes[key[i]] = i;
            }
            int n;
            if (text.Length % code.Length == 0)
            {
                n = text.Length / key.Length;
            }
            else
            {
                n = (text.Length / key.Length) + 1;
            }
            char[,] tab = new char[n, key.Length];
            int counter = 0;
            for(int i = 0; i < n; i++ )
            {
                for (int j = 0; j < key.Length; j++)
                {
                    tab[i,j] = '*';
                    counter++;
                    if (counter > text.Length - 1)
                        break;
                }
            }
            counter = 0;
            for (int j = 0; j < key.Length ; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (tab[i, indexes[j]] == '*')
                    {
                        tab[i, indexes[j]] = text[counter++];

                    }
                    if (counter > text.Length - 1)
                        break;
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (isCorrect(tab[i, j]))
                    {
                        decipher += tab[i, j];
                    }
                }
            }
            return decipher;
        }

        public string matrixCipherC(string text, string code)
        {
            string cipher = "";
            var key = getKeyMatrixCipherB(code);
            int[] indexes = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                indexes[key[i]] = i;
            }
            int n = key.Length;
            char[,] tab = new char[n, key.Length];
            // WPISYWANIE DO TABLICY
            int count = 0;

            for (int i = 0; i < n; i++)
            {
                int counter = 0;
                for (int j = 0; j < key.Length; j++)
                {
                    tab[i, j] = text[count++];
                    counter++;
                    if (counter > indexes[i] || count > text.Length - 1)
                        break;
                }
                if (count > text.Length - 1)
                    break;
            }
            //
            for (int j = 0; j < indexes.Length; j++)
            {
                for (int i = 0; i < n; i++)
                {

                    if (isCorrect(tab[i, indexes[j]]))
                    {
                        cipher += tab[i, indexes[j]];
                    }

                }
            }
            return cipher;
        }

        public string matrixDecipherC(string text, string code)
        {
            string decipher = "";
            var key = getKeyMatrixCipherB(code);
            int[] indexes = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                indexes[key[i]] = i;
            }
            int n = key.Length;
            char[,] tab = new char[n, key.Length];
            int count = 0;
            int counter;
            for (int i = 0; i < n; i++)
            {
                counter = 0;
                for (int j = 0; j < key.Length; j++)
                {
                    tab[i, j] = '*';
                    count++;
                    counter++;
                    if (counter > indexes[i] || count > text.Length - 1)
                        break;
                }
                if (count > text.Length - 1)
                    break;
            }
            counter = 0;
            for (int j = 0; j < key.Length; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (tab[i, indexes[j]] == '*')
                    {
                        tab[i, indexes[j]] = text[counter++];
                    }
                    if (counter > text.Length - 1)
                        break;
                }
                if (counter > text.Length - 1)
                    break;
            }
            counter = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < indexes.Length; j++)
                {
                    if (isCorrect(tab[i, j]))
                    {
                        decipher += tab[i, j];
                        counter++;
                    }
                    if (counter > text.Length - 1)
                        break;
                }
                if (counter > text.Length - 1)
                    break;
            }
            return decipher;
        }


        public string CeasarCipher(string text, int a, int b)
        {
            string cipher = "";
       

            for (int i = 0; i < text.Length; i++)
            {
                cipher += (char)((((text[i] -'A') * a + b) % 26) +'A');
            }
            return cipher;
        }

        public string DecipherCeasar(string text, int a, int b)
        {
            string decipher = "";
            int euler = 12;
            for (int i = 0; i < text.Length; i++)
            {
                decipher += (char)(((((text[i] - 'A') + (26 - b)) * BigInteger.Pow(a, euler - 1)) % 26) + 'A');
            }
            return decipher;
        }


        public string CipherVigenere(string text, string code)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string cipher = "";
            char[,] arr = new char[26, 26];

            for(int i = 0; i < 26; i++)
            {
                for(int j = 0; j < 26; j++ )
                {
                    arr[i,j] = letters[(i + j) % 26];
                }
            }
            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
                cipher += arr[code[index % code.Length] - 'A', text[i] - 'A'];
                index++;
            }


            return cipher;
        }

        public string DecipherVigenere(string text, string code)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string decipher = "";
            char[,] arr = new char[26, 26];

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    arr[i, j] = letters[(i + j) % 26];
                }
            }
            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (text[i] == arr[code[index % code.Length]- 'A',j])
                    {
                        decipher += letters[j];
                        index++;
                        break;
                    }
                }    
            }
            return decipher;
        }

        private bool checkData()
        {
            if (Text.Text == null)
                return false;
            if (Text.Text.Any(char.IsDigit))
                return false;
            if (MatrixKeyB.Text == null)
                return false;
            if (MatrixKeyC.Text == null)
                return false;
            if (MatrixKeyB.Text == null)
                return false;
            if (!Int32.TryParse(A.Text, out _) || !Int32.TryParse(B.Text, out _))
                return false;
            if (!Int32.TryParse(Level.Text, out _))
                return false;
            if (VigenereKey.Text == null)
                return false;

            return true;
        }

        private void onCipherClick(object sender, RoutedEventArgs e)
        {
            
            
            if (checkData())
            {
                string text = Text.Text.ToUpper();
                string matrixCipherBKey = MatrixKeyB.Text.ToUpper();
                string matrixCipherCKey = MatrixKeyC.Text.ToUpper();
                string vigenereKey = VigenereKey.Text.ToUpper();
                int levels = Int32.Parse(Level.Text);
                int a = Int32.Parse(A.Text);
                int b = Int32.Parse(B.Text);

                RailFenceCipher.Text = railFanceCipher(text, levels);
                RailFenceDecipher.Text = decipherRailFence(RailFenceCipher.Text, levels);

                MatrixACipher.Text = matrixCipherA(text);
                MatrixADecipher.Text = decipherMatrixA(MatrixACipher.Text);

                MatrixBCipher.Text = matrixCipherB(text, matrixCipherBKey);
                MatrixBDecipher.Text = matrixDecipherB(MatrixBCipher.Text, matrixCipherBKey);

                MatrixCCipher.Text = matrixCipherC(text, matrixCipherCKey);
                MatrixCDecipher.Text = matrixDecipherC(MatrixCCipher.Text, matrixCipherCKey);

                Ceasar.Text = CeasarCipher(text, a, b);
                CeasarDecipher.Text = DecipherCeasar(Ceasar.Text, a, b);

                VigenereCipher.Text = CipherVigenere(text, vigenereKey);
                VigenereDecipher.Text = DecipherVigenere(VigenereCipher.Text, vigenereKey);
            }
            else
                MessageBox.Show("Podano złe dane");
        }
    }
}