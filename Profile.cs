using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
	public class Profile : INotifyPropertyChanged
	{

		private string name = "";
		private List<Task> tasks = new List<Task>();
		private Task currTask = new Task();

		private TimeSpan timerStart = new TimeSpan();
		private TimeSpan timerPast = new TimeSpan();

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public Profile()
		{

		}

		public Profile(string name)
		{
			this.name = name;
		}

		public Profile(SProfile profile)
		{
			this.name = profile.name;
			foreach (STask sTask in profile.tasks)
			{
				this.tasks.Add(new Task(sTask));
			}
		}

		public void Update()
		{
			NotifyPropertyChanged();
		}

		#region GettersSetters
		
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public Task CurrTask
		{
			get
			{
				return this.currTask;
			}
			set
			{
				this.currTask = value;
				NotifyPropertyChanged();
			}
		}

		public List<Task> Tasks
		{
			get
			{
				return this.tasks;
			}

			set
			{
				this.tasks = value;
				NotifyPropertyChanged();
			}
		}

		public TimeSpan TimerStart
		{
			get
			{
				return this.timerStart;
			}

			set
			{
				this.timerStart = value;
				NotifyPropertyChanged();
			}
		}

		public TimeSpan TimerPast
		{
			get
			{
				return this.timerPast;
			}

			set
			{
				this.timerPast = value;
				NotifyPropertyChanged();
			}
		}



		#endregion

	}
}
