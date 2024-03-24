using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InlyITWPF.Models
{
    public class LocalSettings
    {
        public string LastActive { get; set; }

        public LocalSettings()
        {
            LastActive = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void DateWriter(LocalSettings obj)
        {
            File.Create("testLastUsing.json").Close();
            string ser = JsonConvert.SerializeObject(obj);
            File.WriteAllText("testLastUsing.json", ser, Encoding.UTF8);
        }

        public string DateSetter()
        {
            try
            {
                string jsonString = File.ReadAllText("testLastUsing.json");
                var a = JsonConvert.DeserializeObject<LocalSettings>(jsonString);
                return "Последняя активность: " + a.LastActive.ToString();
            }
            catch
            {
                return "Последней активности еще не было";
            }
        }

    }
}
