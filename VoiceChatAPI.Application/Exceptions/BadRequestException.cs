using System;

namespace VoiceChatAPI.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {

        }

        public BadRequestException(AggregateException ex) : base(ex.Message, ex)
        {

        }
    }
    public class RemovalException : BadRequestException
    {
        public RemovalException(string msg) : base("Удаление невозможно. " + msg) { }
    }
}
