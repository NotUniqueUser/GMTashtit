using System;

namespace HELPER
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("שגיאת קלט")
        {
        }

        public ValidationException(ErrorStatus status, string description)
        {
            if (status == ErrorStatus.EMPTY)
                Message = "יש למלא " + description;
            else if (status == ErrorStatus.ERROR)
                Message = description + " שגוי";
        }

        public ValidationException(string description)
        {
            Message = description;
        }

        public override string Message { get; }
    }
}