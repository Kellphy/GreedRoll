using System;
using System.IO;
using System.Threading;

namespace GreedRoll
{
	class Program
	{
		public static string appVersion = " v1.05";
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
			Upper();
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
			Upper();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Amount to bet: ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			int.TryParse(Console.ReadLine(), out int bet);
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
				for (int i = 10; i >= 1; i--)
				{
					MenuCD(i);
					Thread.Sleep(500);
				}

				savedCoins++;
				coins += savedCoins;
				savedCoins = 0;
				Save();
			}
		}

		public static void Roll(int bet)
		{
			int last = bet;
			while (true)
			{
				Random rand = new Random();
				Console.Clear();
				Title();
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

		#region App Lines
		private static void Upper()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("Coins: ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write($"{coins}\n");
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("-----------------------------\n");
		}

		private static void Title()
		{
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.Write($"  Kellphy's Greed Roll{appVersion}\n");
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("-----------------------------");
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
				Console.Write(" [" + cd / 2 + "]\n");
			}
		}
		#endregion

		public static void Save()
		{
			using (StreamWriter writer = new StreamWriter(fileName))
			{
				string encryptedString = Encrypt(coins.ToString(), "H5K2o0s1bCe9");
				writer.Write(encryptedString);
			}
		}

		public static string Encrypt(string plainText, string passPhrase)
		{
			return plainText;
		}

		public static string Decrypt(string cipherText, string passPhrase)
		{
			return cipherText;
		}
	}
}
