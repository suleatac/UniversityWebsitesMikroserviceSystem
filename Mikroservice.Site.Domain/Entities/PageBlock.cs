using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Domain.Entities
{
    /// <summary>
    /// PageSection icindeki dinamik container. content_type degerine gore
    /// metin, video, gorsel veya carousel olarak render edilir.
    /// ParentId null ise dogrudan section satirinda; degilse ust block'un
    /// altinda nested row olarak render edilir (Recursive row yapisi).
    /// </summary>
    public class PageBlock
    {
        public int Id { get; set; }

        public int PageSectionId { get; set; }

        /// <summary>Nested satirlar icin ust block. Top level blocklarda null.</summary>
        public int? ParentId { get; set; }

        /// <summary>BlockContentType degerlerinden biri: Text / Video / Image / Carousel.</summary>
        public string ContentType { get; set; } = BlockContentType.Text;

        /// <summary>HTML icerik (Text ve Image tipinde kullanilir).</summary>
        public string? Content { get; set; }

        public string? VideoUrl { get; set; }

        /// <summary>VideoTuru degerlerinden biri: YouTube / Local.</summary>
        public string? VideoType { get; set; }

        /// <summary>Block arka plan gorseli (video poster / image url).</summary>
        public string? BackgroundImageUrl { get; set; }

        public string? BackgroundColor { get; set; }

        /// <summary>Bootstrap kolon genisligi (1-12).</summary>
        public int ColumnSize { get; set; } = 12;

        /// <summary>Kolonun satir icindeki sirasi.</summary>
        public int RowNumber { get; set; }

        /// <summary>Animate.css animasyon adi (orn: fadeInUp).</summary>
        public string? Animation { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation
        public PageSection PageSection { get; set; } = default!;
        public PageBlock? Parent { get; set; }
        public ICollection<PageBlock> Children { get; set; } = new List<PageBlock>();
        public ICollection<PageBlockMedia> Medias { get; set; } = new List<PageBlockMedia>();
    }
}
