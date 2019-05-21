using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
	[Serializable]
	public class STask
	{
		public string title;
		public int seconds;
		public int minutes;
		public int hours;
		public int type;

		public STask(Task task)
		{
			this.title = task.Title;
			this.seconds = task.Seconds;
			this.minutes = task.Minutes;
			this.hours = task.Hours;
			this.type = task.Type;
		}
	}
}
