using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Personel.Domain.Exceptions
{
    public class PersonelSyncException : Exception
    {
        public PersonelSyncException(string message) : base(message)
        {
        }
    
    }
}
