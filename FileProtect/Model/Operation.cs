using System;

namespace FileProtect.Model
{
    class Operation
    {
        public string Code { get; private set; }

        public OperationType Type { get; private set; }

        public DateTime Date { get; private set; }

        public string Target { get; private set; }

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
