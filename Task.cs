using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
	public class Task : INotifyPropertyChanged
	{
		private string title;
		private DateTime start;
		//private int milliseconds = 0;
		private int seconds = 0;
		private int minutes = 0;
		private int hours = 0;
		private int type = 0;

		//private Dictionary<DateTime, TimeSpan> time = new Dictionary<DateTime, TimeSpan>();
		

		private bool isStarted = false;

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public Task()
		{

		}

		public Task(string title, int type)
		{
			this.title = title.Trim();
			this.type = type;
		}

		public Task(STask sTask)
		{
			this.title = sTask.title;
			this.seconds = sTask.seconds;
			this.minutes = sTask.minutes;
			this.hours = sTask.hours;
			this.type = sTask.type;
		}

		public void Update()
		{
			NotifyPropertyChanged();
		}

		private void Check()
		{
			if (this.seconds >= 60)
			{
				this.minutes += this.seconds / 60;
				this.seconds = (int)(this.seconds % 60);
			}

			if (this.minutes >= 60)
			{
				this.hours += this.minutes / 60;
				this.minutes = (int)(this.minutes % 60);
			}

			NotifyPropertyChanged();
		}

		public void Toggle()
		{
			if (!string.IsNullOrEmpty(this.title))
			{
				if (this.isStarted)
				{
					TimeSpan ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

					TimeSpan past = ts.Subtract(new TimeSpan(this.start.Hour, this.start.Minute, this.start.Second));

					this.Add(past.Seconds, past.Minutes, past.Hours);

					this.start = new DateTime();
					this.isStarted = false;
				}
				else
				{
					this.start = DateTime.Now;
					this.isStarted = true;
				}

				NotifyPropertyChanged();
			}
		}

		public void Add(int seconds)
		{
			this.seconds += seconds;

			this.Check();
		}

		public void Add(int seconds, int minutes)
		{
			this.seconds += seconds;
			this.minutes += minutes;

			this.Check();
		}

		public void Add(int seconds, int minutes, int hours)
		{
			this.seconds += seconds;
			this.minutes += minutes;
			this.hours += hours;

			this.Check();
		}


		#region GettersSetters

		public string Title
		{
			get
			{
				return this.title;
			}
		}

		public int Seconds
		{
			get
			{
				return this.seconds;
			}
		}

		public int Minutes
		{
			get
			{
				return this.minutes;
			}
		}

		public int Hours
		{
			get
			{
				return this.hours;
			}
		}

		public bool IsStarted
		{
			get
			{
				return this.isStarted;
			}
		}

		public string AllTime
		{
			get
			{
				string text = !this.isStarted ? $"{this.hours}:{this.minutes}:{this.seconds}" : "In progress...";

				return !string.IsNullOrEmpty(this.title) ? text : "";
			}
		}

		public string IsStartedText
		{
			get
			{
				return this.isStarted ? "Stop" : "Start";
			}
		}

		public int Type
		{
			get
			{
				return this.type;
			}
		}

		public string Image
		{
			get
			{
				string source = "";

				switch (this.type)
				{
					case 0:
						source = @"Images\file.png";
						break;
					case 1:
						source = @"Images\folder.png";
						break;
				}

				return source;
			}
		}

		#endregion


	}
}
