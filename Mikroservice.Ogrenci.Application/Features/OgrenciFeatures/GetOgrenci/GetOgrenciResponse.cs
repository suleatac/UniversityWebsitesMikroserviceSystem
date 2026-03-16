using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public record GetOgrenciResponse(int id, int personid, string tcnumarasi);
}
