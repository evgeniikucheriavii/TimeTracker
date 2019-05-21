using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TimeTracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private Profile profile = new Profile();

		private List<Border> borders = new List<Border>();
		private DispatcherTimer timer = new DispatcherTimer();

		

		public MainWindow()
		{
			InitializeComponent();

			


			//profile.CurrTask = profile.Tasks[0];

			

			timer.Tick += new EventHandler(timer_Tick);

			timer.Interval = new TimeSpan(0, 0, 0, 0, 100);


			borders = GetBorders();

			Load();

			this.DataContext = profile;
		}



		private void timer_Tick(object sender, EventArgs e)
		{
			TimeSpan timerNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			profile.TimerPast = timerNow.Subtract(profile.TimerStart);
		}

		private void Save()
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "timetracker.profile");

			BinaryFormatter formatter = new BinaryFormatter();

			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				try
				{
					formatter.Serialize(fs, new SProfile(profile));
				}
				catch (Exception exc)
				{
					MessageBox.Show(exc.StackTrace);
				}
			}

		}

		private void Load()
		{

			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "timetracker.profile");

			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();

				using(FileStream fs = new FileStream(path, FileMode.Open))
				{
					try
					{
						SProfile sProfile = formatter.Deserialize(fs) as SProfile;

						profile = new Profile(sProfile);
					}
					catch (Exception exc)
					{
						MessageBox.Show(exc.StackTrace);
					}


				}
			}
		}

		private List<Border> GetBorders()
		{
			List<Border> borders = new List<Border>();

			borders.Add(AddBorder);
			borders.Add(TasksBorder);

			return borders;
		}

		private void Show(Border border)
		{
			foreach (Border b in borders)
			{
				b.Visibility = Visibility.Hidden;
			}

			border.Visibility = Visibility.Visible;


		}

		private void NewTaskButton_Click(object sender, RoutedEventArgs e)
		{
			Show(AddBorder);
		}

		private void HeaderButton_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(profile.CurrTask.Title))
			{
				profile.CurrTask.Toggle();

				Save();

				if (profile.CurrTask.IsStarted)
				{
					profile.TimerStart = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
					timer.Start();
				}
				else
				{
					profile.TimerStart = new TimeSpan();
					timer.Stop();
				}
			}
			else
			{
				MessageBox.Show("Select the task first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void TasksListBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (profile.CurrTask.IsStarted)
			{
				MessageBox.Show("Stop your task first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				int index = TasksListBox.SelectedIndex;
				if (index != -1)
				{
					profile.CurrTask = profile.Tasks[index];

					//TasksListBox.UnselectAll();
				}
			}

		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			string text = TitleTextBox.Text.Trim();
			int type = 0;
			try
			{
				type = TypeComboBox.SelectedIndex;
			}
			catch (Exception exc)
			{

			}

			if (!string.IsNullOrEmpty(text))
			{
				TitleTextBox.Text = "";

				profile.Tasks.Add(new Task(text, type));
				profile.Update();

				Save();

				Show(TasksBorder);
			}
			else
			{
				MessageBox.Show("Title field must be filled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
			}

			
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			TitleTextBox.Text = "";
			profile.Update();
			Show(TasksBorder);
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(profile.CurrTask.Title))
			{
				MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete task", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					List<Task> tasks = new List<Task>();

					foreach (Task task in profile.Tasks)
					{
						if (!task.Equals(profile.CurrTask))
						{
							tasks.Add(task);
						}
					}

					profile.Tasks = tasks;
					profile.CurrTask = new Task();
					Save();
				}
			}
			else
			{
				MessageBox.Show("Select the task first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
			}

		}
	}
}
