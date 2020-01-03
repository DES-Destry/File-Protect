using System;
using System.Runtime.Serialization;

namespace FileProtect.Model
{
    [DataContract]
    class Operation
    {
        [DataMember]
        public string Code { get; private set; }
        [DataMember]
        public OperationType Type { get; private set; }
        [DataMember]
        public DateTime Date { get; private set; }
        [DataMember]
        public string Target { get; private set; }
        [DataMember]
        public string Result { get; private set; }

        public Operation(string code, OperationType type, DateTime time, string target, string result)
        {
            this.Code = code;
            this.Type = type;
            this.Date = time;
            this.Target = target;
            this.Result = result;
        }
    }
}
