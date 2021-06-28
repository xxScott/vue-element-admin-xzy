using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DAL
{
    [Serializable]
    public class CustomException : Exception, ISerializable
    {
        public CustomException() : base() { }
        public CustomException(string message) : base(message) { }
        public CustomException(string message, Exception inner) : base(message, inner) { }

        private Enum_StatusCode statuscode;
        public Enum_StatusCode StatusCode { get { return statuscode; } }

        private CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.statuscode = (Enum_StatusCode)info.GetInt16("StatusCode");

        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StatusCode", statuscode);
            base.GetObjectData(info, context);
        }

        public CustomException(string message, Enum_StatusCode statusCode)
            : this(message)
        {
            this.statuscode = statusCode;
        }

        public CustomException(string message, Enum_StatusCode statusCode,Exception inner)
            : this(message,inner)
        {
            this.statuscode = statusCode;
        }
    }
}