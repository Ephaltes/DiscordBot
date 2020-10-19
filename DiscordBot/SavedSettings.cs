using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DiscordBot
{
    [Serializable]
    class SavedSettings
    {
        private Dictionary<ulong,ulong> UploadOnlyList;

        private static SavedSettings _settings = null;

        private static object lockobject = new object();

        public static SavedSettings settings
        {
            //Prüft ob es null ist wenn null dann leg für _settings eine neue AppSettings an
            get
            {
                lock (lockobject)
                {
                    return _settings ??= new SavedSettings();
                }
            }
        }

        private SavedSettings()
        {

            UploadOnlyList = new Dictionary<ulong, ulong>();
        }

        public bool Load()
        {
            try
            {
                using (Stream stream = File.Open("channels.save", FileMode.Open))
                {
                    var bformatter = new BinaryFormatter();

                    _settings = (SavedSettings)bformatter.Deserialize(stream);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                
            }

        }


        public Dictionary<ulong, ulong> GetUploadOnlyList()
        {
            return UploadOnlyList;
        }

        public bool AddToUploadOnlyList(ulong key,ulong value)
        {
            if (!UploadOnlyList.ContainsKey(key))
            {
                UploadOnlyList.Add(key,value);
                try
                {
                    lock (lockobject)
                    {
                        using (Stream stream = File.Open("channels.save", FileMode.Create))
                        {
                            var bformatter = new BinaryFormatter();

                            bformatter.Serialize(stream, this);

                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return false;
        }

    }
}
