using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Domain.Exceptions
{
    public class OgrenciSyncException : Exception
    {
        public OgrenciSyncException(string message) : base(message)
        {
        }
    
    }
}
