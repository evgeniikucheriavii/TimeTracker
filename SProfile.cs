using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
	[Serializable]
	public class SProfile
	{
		public string name;
		public List<STask> tasks;

		public SProfile(Profile profile)
		{
			this.name = profile.Name;
			tasks = new List<STask>();

			foreach (Task task in profile.Tasks)
			{
				tasks.Add(new STask(task));
			}

		}
	}
}
