using System.Runtime.Serialization;

namespace FileProtect.Model
{
    [DataContract]
    class Meta 
    {
        [DataMember]
        private string FileExtencion { get; set; }
        [DataMember]
        public string AppVersion { get; set; }
        [DataMember]
        public string FinalPath { get; set; }

        public Meta(string fileExtencion, string finalPath, string appVersion)
        {
            this.FileExtencion = fileExtencion;
            this.FinalPath = finalPath;
            this.AppVersion = appVersion;
        }

        public string GetExtencion()
        {
            return this.FileExtencion;
        }
    }
}
