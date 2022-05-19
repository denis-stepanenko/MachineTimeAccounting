using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace MachineTimeAccounting
{
    class SettingsService
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetExecutingAssembly().GetName().Name + "Settings.xml");

		public void Save(Settings settings)
		{
			var serializer = new XmlSerializer(typeof(Settings));
			using (var fs = new FileStream(path, FileMode.Create))
				serializer.Serialize(fs, settings);
		}

		public Settings Load()
		{
			if (!File.Exists(path)) return new Settings();
			var serializer = new XmlSerializer(typeof(Settings));
			using (var fs = new FileStream(path, FileMode.Open))
				return (Settings)serializer.Deserialize(fs);
		}
	}
}
