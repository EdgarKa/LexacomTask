using System.Text.RegularExpressions;

namespace LexacomTask
{
	internal class Program
	{
		static string firstString = "Today we saw 3 patients[[new-line]]The first was Name:Michael Michaelson NHS Number:333444[[New-Line]]the second was Name:Jane Bridge NHS NUmber:55666 with her son Name:David Bridge NHS Number:a44t55[[new-line]]We then saw[[new-line]]NHS Number:999 James McDonald[[new-line]][[new-line]]NHS Number:444 ";
		static string secondString = "{[{\"Name\":\"James Jamerson\",\"NHSNumber\":12345},{\"Name\":\"Bob Sinclair\",\"NHSNumber\":5555},{\"Name\":\"Sally Jamerson\",\"NHSNumber\":66554},{\"Name\":\"Michael Myers\",\"NHSNumber\":6666},{\"Name\":\"James Jamerson\",\"NHSNumber\":12345}]}";
		static List<string> errors = new List<string>();

		static void Main(string[] args)
		{
			Console.WriteLine("Enter a string:");
			string input = Console.ReadLine();
			ProcessData(input);
		}

		private static void ProcessData(string input)
		{
			var jsonRes = ToJson(input);
			if (jsonRes != null)
			{
				OutputData(jsonRes);
			}
			else
			{
				Data data = ProcessVoiceInput(input);
				OutputData(data);
			}
		}

		private static Data ProcessVoiceInput(string input)
		{
			List<PatientsDatum> patientsData = new List<PatientsDatum>();
			string name = "";

			input = input.Replace("[[new-line]]", "\n").Replace("[[New-Line]]", "\n");
			Console.WriteLine(input);

			Regex re = new Regex("[^a-zA-Z0-9 -]");
			List<string> words = re.Replace(input, " ").Split(" ").ToList(); // eliminate symbols in the string and convert to list
			words = words.Where(x => x.Length > 1 && !string.IsNullOrWhiteSpace(x)).ToList(); // remove single letter s and white spaces
			for (int i = 0; i < words.Count - 1; i++)
			{
				// Get First and Last Names for the specific string
				if (char.IsUpper(words[i][0]) && char.IsUpper(words[i + 1][0]) && !words[i].ToUpper().Contains("NHS") && !words[i + 1].ToUpper().Contains("NHS")) 
					name = $"{words[i]} {words[i + 1]}";

				if (words[i].ToLower().Contains("number"))
				{
					int.TryParse(words[i + 1], out int NHSNum);
					if (NHSNum == 0)
					{
						errors.Add($"Name {name} appears to have invalid NHS Number \"{words[i + 1]}\"");
					}
					patientsData.Add(new PatientsDatum(name, NHSNum));
					name = "";
				}
			}
			Data dt = new Data();
			dt.PatientsData = patientsData;
			return dt;
		}

		private static void OutputData(Data test)
		{
			Console.WriteLine("---------------------------------");
			Console.WriteLine(String.Format("|{0,1}{1,17}{2,7}|", "Name", "|", "NHS Number"));
			
			Console.WriteLine("|--------------------|----------|");
			foreach (var t in test.PatientsData)
			{
				Console.WriteLine(String.Format("|{0,-20}|{1,-10:D}|", t.Name, t.NHSNumber));
			}
			Console.WriteLine("|_______________________________|");
			if (errors.Count > 0)
				foreach (var er in errors)
					Console.WriteLine(er);
		}

		private static Data ToJson(string stringToCheck)
		{
			Data myDeserializedClass;
			try
			{
				stringToCheck = stringToCheck.Replace("{[", "{\"PatientsData\":[");
				myDeserializedClass = System.Text.Json.JsonSerializer.Deserialize<Data>(stringToCheck);
			}
			catch
			{
				return null;
			}
			return myDeserializedClass;
		}
	}
}