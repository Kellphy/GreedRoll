using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace GreedRoll
{
    class Program
    {
        public static string kVersion = " v1.04";
        public static int coins;
        public static string fileName;
        public static bool inMenu;
        public static int savedCoins;
        public static void Main()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(30, 20);
            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GreedRoll.save");
            using (StreamWriter w = File.AppendText(fileName)) { }

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                if ((line = sr.ReadLine()) != null)
                {
                        coins = Int32.Parse(Decrypt(line, "H5K2o0s1bCe9"));
                }
                else
                {
                    coins = 0;
                }
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                AddCoins();
            }).Start();

            Menu();
        }

        public static void Menu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("  Kellphy's Greed Roll" + kVersion + "\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-----------------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Coins: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(coins+"\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-----------------------------\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[Esc]");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" to Exit!\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(">>>>>>>>>>> PLAY! <<<<<<<<<<<\n");
            Console.BackgroundColor = ConsoleColor.Black;
            inMenu = true;
            ConsoleKey keyDec = Console.ReadKey().Key; ;
            inMenu = false;
            if (keyDec == ConsoleKey.Escape)
            {
                Save();
                Environment.Exit(1);
            }
            Play();
        }

        public static void Play()
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("  Kellphy's Greed Roll"+kVersion+"\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-----------------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Coins: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(coins + "\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-----------------------------\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Amount to bet: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            int bet = 0;
            Int32.TryParse(Console.ReadLine(), out bet);
            Console.CursorVisible = false;
            if (bet > coins || bet <= 1)
            {
                Menu();
            }
            coins -= bet;
            Save();
            Console.ForegroundColor = ConsoleColor.Blue;
            Roll(bet);
            Menu();

        }

        public static void AddCoins()
        {
            Thread.Sleep(500);
            while (true)
            {
                MenuCD(10);
                Thread.Sleep(500);
                MenuCD(9);
                Thread.Sleep(500);
                MenuCD(8);
                Thread.Sleep(500);
                MenuCD(7);
                Thread.Sleep(500);
                MenuCD(6);
                Thread.Sleep(500);
                MenuCD(5);
                Thread.Sleep(500);
                MenuCD(4);
                Thread.Sleep(500);
                MenuCD(3);
                Thread.Sleep(500);
                MenuCD(2);
                Thread.Sleep(500);
                MenuCD(1);
                Thread.Sleep(500);

                savedCoins++;
                coins += savedCoins;
                savedCoins = 0;
                Save();
            }
        }

        public static void MenuCD(int cd)
        {
            if (inMenu)
            {
                Console.SetCursorPosition(0, 2);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Coins: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(coins);
                Console.ForegroundColor = ConsoleColor.Blue;
                for (int i = 0; i < cd; i++)
                {
                    Console.Write(" ");
                }
                for (int j = cd; j < 10; j++)
                {
                    Console.Write("<");
                }
                Console.Write(" [" + cd/2 + "]\n");
            }
        }


        public static void Roll(int bet)
        {
            int last = bet;
            while (true)
            {
                Random rand = new Random();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("  Kellphy's Greed Roll" + kVersion + "\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("-----------------------------");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Bet: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(bet);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(" | ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Current Roll: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(last + "\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("-----------------------------\n");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("[Esc]");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" to Give up & Keep: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(last);
                Console.Write("\n\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write(">>>>>>>>>>> ROLL! <<<<<<<<<<<\n\n");
                Console.BackgroundColor = ConsoleColor.Black;

                ConsoleKey keyDec = Console.ReadKey().Key; ;
                if (keyDec == ConsoleKey.Escape)
                {
                    coins += last;
                    Save();
                    Menu();
                }

                int random = rand.Next(1, last + 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You rolled: " + random + " (" + random + "/" + last + ")\n");
                last = random;
                if (last <= 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("<<<<<<<<< YOU LOST! >>>>>>>>>");
                    Console.WriteLine("-----------------------------\n");
                    Save();
                    break;
                }
                else
                {
                    random = rand.Next(1, last + 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("RNG rolled: " + random + " (" + random + "/" + last + ")\n");
                    last = random;
                    if (last <= 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("-----------------------------");
                        Console.WriteLine(">>>>>>>>>> YOU WON <<<<<<<<<<");
                        Console.WriteLine("-----------------------------\n");
                        coins += bet * 2;
                        Save();
                        break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue!");
                Console.ReadKey();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to go to Menu!");
            Console.ReadKey();
        }

        public static void Save()
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                string encryptedString = Encrypt(coins.ToString(), "H5K2o0s1bCe9");
                writer.Write(encryptedString);
            }
        }

        private const int Keysize = 256;
        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            byte[] saltStringBytes = Generate256BitsOfRandomEntropy();
            byte[] ivStringBytes = Generate256BitsOfRandomEntropy();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                byte[] cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            byte[] saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            byte[] ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            byte[] cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                Int32 decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            byte[] randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
