using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Persistence.Settings
{
    public class ExternalOgrenciApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string BasicAuthToken { get; set; } = string.Empty;
    }
}
