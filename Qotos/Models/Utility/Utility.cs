

namespace Qotos.Models.Utility
{
	public class Utility
	{
		public static string GenerateString()
		{
			Random rand = new Random();
			int stringlen = rand.Next(4, 10);
			int randValue;
			string str = "";
			char letter;
			for (int j = 0; j < stringlen; j++)
			{
				randValue = rand.Next(0, 26);
				letter = Convert.ToChar(randValue + 65);
				str = str + letter;
			}
			return str;
		}
	}
}
