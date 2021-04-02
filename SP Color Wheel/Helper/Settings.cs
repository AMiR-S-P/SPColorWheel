using Newtonsoft.Json;
using SP_Color_Wheel.Models.Settings;
using SP_Color_Wheel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.Helper
{
    public static class Settings
    {
        static object _lock=new object();



        public static string Path { set; get; }

        [JsonProperty]
        public static WheelSettingsModel WheelSettings { set; get; } 


        public static Task Save()
        {
            lock (_lock)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Wheel = WheelSettings
                },
                Formatting.Indented);

                using (var file = File.CreateText(Path))
                {
                    try{
                        file.Write(json);
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            return Task.CompletedTask;
        }

        public static Task Load()
        {
            try
            {
                lock (_lock)
                {
                    string json = "";
                    using (var file = File.OpenText(Path))
                    {
                        json = file.ReadToEnd();
                    }

                    var anonymous = new { Wheel = new WheelSettingsModel() };


                    var deserial = JsonConvert.DeserializeAnonymousType(json, anonymous);

                    WheelSettings = new WheelSettingsModel();
                    if (deserial.Wheel != null)
                    {
                        WheelSettings = deserial.Wheel;
                    }
                }
            }
            catch (Exception ex) 
            { }
            return Task.CompletedTask;
        }
    }
}
