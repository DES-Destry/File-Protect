using System.Runtime.Serialization;

namespace FileProtect.Model
{
    [DataContract]
    public class Settings 
    {
        [DataMember]
        private string RRHBN { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public bool CheckUpdates { get; set; }
        [DataMember]
        public bool DelDataBefOperation { get; set; }
        [DataMember]
        public bool WriteLogs { get; set; }
        [DataMember]
        public bool SaveCache { get; set; }
        [DataMember]
        public bool WarningMessageShow { get; set; }
        [DataMember]
        public bool WriteErrorFiles { get; set; }

        public Settings(string aaa, string ver, bool checkUpdates, bool deleteData, bool writeLogs, bool saveCache, bool warning, bool errors)
        {
            this.RRHBN = aaa;
            this.Version = ver;
            this.CheckUpdates = checkUpdates;
            this.DelDataBefOperation = deleteData;
            this.WriteLogs = writeLogs;
            this.SaveCache = saveCache;
            this.WarningMessageShow = warning;
            this.WriteErrorFiles = errors;
        }

        public string ASSKOP()
        {
            return RRHBN;
        }
    }
}
