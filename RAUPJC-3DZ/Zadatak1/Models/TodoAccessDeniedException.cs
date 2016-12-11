using System;

namespace Zadatak1.Models
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException()
        {
        }

        public TodoAccessDeniedException(string message)
            : base(message)
        {
        }

        public TodoAccessDeniedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
