using System;

namespace MTUnity.Utils
{
	
	public class TimeUtil
	{
		
		/// <summary>
		/// 格式化时间。“01:09:40” "09:40" "00:40"
		/// </summary>
		/// <returns>The time.</returns>
		/// <param name="time">Time.</param>
		public static string FormatTime (int time)
		{
			string formatTime = "";
			int hour = 0;
			int minute = 0;
			int second = 0;
			if (time >= 3600) {
				hour = (int)Math.Floor ((double)(time / 3600));
				time = (int)Math.Floor ((double)(time % 3600));
			}
			if (time >= 60) {
				minute = (int)Math.Floor ((double)(time / 60));
				time = (int)Math.Floor ((double)(time % 60));
			}
			second = time;
			if (hour > 9) {
				formatTime += hour + ":";
			} else if (hour > 0) {
				formatTime += "0" + hour + ":";
			}
			if (minute > 9) {
				formatTime += minute + ":";
			} else {
				formatTime += "0" + minute + ":";
			}
			if (second > 9) {
				formatTime += second;
			} else {
				formatTime += "0" + second;
			}
			return formatTime;
		}

		/// <summary>
		/// 获取当前时间戳
		/// </summary>
		/// <returns>The time stamp.</returns>
		/// <param name="second">为真时获取10位时间戳(秒),为假时获取13位时间戳(毫秒)</param>
		public static long GetTimeStamp (bool second = false)
		{  
			TimeSpan ts = DateTime.Now - DateTime.Parse ("1970-1-1");  
			if (second)
				return Convert.ToInt64 (ts.TotalSeconds);
			else
				return Convert.ToInt64 (ts.TotalMilliseconds);

		}

		public static DateTime UTC_BASE = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		/**
		 * Get UTC timestamp
		 */
		public static double GetUTCTimestamp() {
			return DateTime.UtcNow.Subtract (UTC_BASE).TotalSeconds;
		}

		public static double GetUTCTimestamp(DateTime dt) {
			dt = dt.ToUniversalTime ();
			return (dt.Subtract (UTC_BASE)).TotalSeconds;
		}

		public static DateTime FromUTCTimestamp(double timestamp) {
			TimeSpan span = TimeSpan.FromSeconds (timestamp);
			return UTC_BASE.Add (span);
		}

		public static DateTime GetDayStart(DateTime dataTime)
		{
			return new DateTime (dataTime.Year, dataTime.Month, dataTime.Day, 0, 0, 0, dataTime.Kind);
		}

		public static DateTime GetDayEnd(DateTime dataTime)
		{
			return new DateTime (dataTime.Year, dataTime.Month, dataTime.Day, 23, 59, 59, dataTime.Kind);
		}

		public static bool IsDayEnd(DateTime dataTime)
		{
			return dataTime.Hour == 23 && dataTime.Minute == 59 && dataTime.Second == 59;
		}
	}
	
}