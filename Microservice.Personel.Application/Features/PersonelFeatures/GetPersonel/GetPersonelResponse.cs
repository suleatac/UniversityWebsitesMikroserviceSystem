using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public record GetPersonelResponse(int id, int personid, string tcnumarasi);
}
