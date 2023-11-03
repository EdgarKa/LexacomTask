using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LexacomTask
{
	// Was Data
	public class Data
	{
		public List<PatientsDatum> PatientsData { get; set; }
	}

	// Was PatientsData
	public class PatientsDatum
	{
		[JsonConstructor]
		public PatientsDatum(string name, int nHSNumber)
		{
			Name = name;
			NHSNumber = nHSNumber;
		}

		public PatientsDatum(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
        public int NHSNumber { get; set; }
    }
}
