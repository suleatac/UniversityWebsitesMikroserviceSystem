using Microservice.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.MenuFeatures.UpdateMenu
{
    public record UpdateMenuCommand : IRequestByServiceResult
    {
        public int Id { get; set; }
        public int SiteId { get; init; }

        public int DilId { get; init; }

        public int HedefId { get; init; }

        public string Ad { get; init; } = default!;

        public string Link { get; init; } = default!;

        public string? IconUrl { get; init; }

        public string? Icerik { get; init; }

        public int Sira { get; init; }

        public bool MegaMenu { get; init; }

        public int? ParentId { get; init; }
    }
}
