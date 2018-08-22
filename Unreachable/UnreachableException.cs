using System;

namespace System
{
    public class UnreachableException : System.Exception
    {
        public UnreachableException()
            : base("This code should not be reachable")
        {
        }
    }
}
