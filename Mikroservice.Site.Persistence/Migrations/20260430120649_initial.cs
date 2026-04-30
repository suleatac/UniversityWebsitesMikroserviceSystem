using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Birimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birimler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Birimler_Birimler_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InternationalAd = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Kod = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hedefler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedefler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonelTipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelTipleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SikcaSorulanSoruKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SikcaSorulanSoruKategorileri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templateler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TemplateTuru = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FolderName = table.Column<string>(type: "text", nullable: true),
                    LayoutPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templateler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoneticiTipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoneticiTipleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YonetimDuyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YonetimDuyurular", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unvanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipId = table.Column<int>(type: "integer", nullable: false),
                    Ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    KisaAd = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unvanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unvanlar_PersonelTipleri_TipId",
                        column: x => x.TipId,
                        principalTable: "PersonelTipleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Siteler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SiteAdiEng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SiteUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BirimId = table.Column<int>(type: "integer", nullable: false),
                    SiteAlanAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SiteEPostaSifre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SiteEPostaHost = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SiteEPostaPort = table.Column<int>(type: "integer", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SiteEPosta = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siteler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Siteler_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Siteler_Templateler_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templateler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BandLogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    Ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ImgUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EklenmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandLogos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandLogos_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BandLogos_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Icerik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    HedefId = table.Column<int>(type: "integer", nullable: true),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    KisaAciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IcerikMetni = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResimUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    GosterimSayisi = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    YayimTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EklemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    BaslamaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SeoUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    Tip = table.Column<int>(type: "integer", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: true),
                    TamEkranMi = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    GosterimSuresiSaniye = table.Column<int>(type: "integer", nullable: true, defaultValue: 5),
                    CookieIleTekrarGosterme = table.Column<bool>(type: "boolean", nullable: true),
                    VideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icerik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Icerik_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Icerik_Hedefler_HedefId",
                        column: x => x.HedefId,
                        principalTable: "Hedefler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Icerik_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    Path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Url = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaFile_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaFile_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    HedefId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IconUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Icerik = table.Column<string>(type: "text", nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    MegaMenu = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menuler_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menuler_Hedefler_HedefId",
                        column: x => x.HedefId,
                        principalTable: "Hedefler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Menuler_Menuler_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menuler_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SikcaSorulanSorular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    KategoriId = table.Column<int>(type: "integer", nullable: false),
                    Soru = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Cevap = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SeoUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SikcaSorulanSorular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SikcaSorulanSorular_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SikcaSorulanSorular_SikcaSorulanSoruKategorileri_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "SikcaSorulanSoruKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SikcaSorulanSorular_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteOzellikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    SiteAdress = table.Column<string>(type: "text", nullable: false),
                    SiteAdressEng = table.Column<string>(type: "text", nullable: false),
                    SiteBaslangicHakkimizda = table.Column<string>(type: "text", nullable: false),
                    SiteBaslangicHakkimizdaEng = table.Column<string>(type: "text", nullable: false),
                    SiteTelNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SiteFaxNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SiteFacebookAdress = table.Column<string>(type: "text", nullable: false),
                    SiteTwitterAdress = table.Column<string>(type: "text", nullable: false),
                    SiteInstagramAdress = table.Column<string>(type: "text", nullable: false),
                    SiteYoutubeAdress = table.Column<string>(type: "text", nullable: false),
                    SiteHaritaAdress = table.Column<string>(type: "text", nullable: false),
                    SiteBaslangicVideoLink = table.Column<string>(type: "text", nullable: false),
                    SiteBaslangicVideoResimAdress = table.Column<string>(type: "text", nullable: false),
                    SiteWatsappAdress = table.Column<string>(type: "text", nullable: false),
                    SiteLinkedinAdress = table.Column<string>(type: "text", nullable: false),
                    SiteHakkindaLink = table.Column<string>(type: "text", nullable: false),
                    SiteVideoType = table.Column<string>(type: "text", nullable: false),
                    SiteHakkindaResim = table.Column<string>(type: "text", nullable: false),
                    SiteFooterLogo = table.Column<string>(type: "text", nullable: false),
                    SiteTopbarLogo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteOzellikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteOzellikleri_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SitePersonelleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    UnvanId = table.Column<int>(type: "integer", nullable: false),
                    PersonelTipId = table.Column<int>(type: "integer", nullable: false),
                    ResimUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IlgiAlanlari = table.Column<string>(type: "text", nullable: false),
                    BlogAdress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TwitterAdress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    FacebookAdress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    InstagramAdress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    GoogleplusAdress = table.Column<string>(type: "text", nullable: false),
                    Hakkinda = table.Column<string>(type: "text", nullable: false),
                    DeneyimVeCalismalari = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SitePersonelleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SitePersonelleri_PersonelTipleri_PersonelTipId",
                        column: x => x.PersonelTipId,
                        principalTable: "PersonelTipleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SitePersonelleri_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SitePersonelleri_Unvanlar_UnvanId",
                        column: x => x.UnvanId,
                        principalTable: "Unvanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YoneticiSiteler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KeycloakUserId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    YoneticiTipiId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoneticiSiteler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoneticiSiteler_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YoneticiSiteler_YoneticiTipleri_YoneticiTipiId",
                        column: x => x.YoneticiTipiId,
                        principalTable: "YoneticiTipleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonelTelefonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SitePersonelId = table.Column<int>(type: "integer", nullable: false),
                    TelefonNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelTelefonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelTelefonlar_SitePersonelleri_SitePersonelId",
                        column: x => x.SitePersonelId,
                        principalTable: "SitePersonelleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandLogos_DilId",
                table: "BandLogos",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_BandLogos_SiteId",
                table: "BandLogos",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Birimler_Ad",
                table: "Birimler",
                column: "Ad");

            migrationBuilder.CreateIndex(
                name: "IX_Birimler_ParentId",
                table: "Birimler",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Birimler_Sira",
                table: "Birimler",
                column: "Sira");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_DilId",
                table: "Icerik",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_HedefId",
                table: "Icerik",
                column: "HedefId");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_DilId",
                table: "Icerik",
                columns: new[] { "SiteId", "DilId" });

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_SeoUrl",
                table: "Icerik",
                columns: new[] { "SiteId", "SeoUrl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_Sira",
                table: "Icerik",
                columns: new[] { "SiteId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFile_DilId",
                table: "MediaFile",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFile_SiteId",
                table: "MediaFile",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_DilId",
                table: "Menuler",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_HedefId",
                table: "Menuler",
                column: "HedefId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_ParentId_Sira",
                table: "Menuler",
                columns: new[] { "ParentId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_SiteId_DilId",
                table: "Menuler",
                columns: new[] { "SiteId", "DilId" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelTelefonlar_SitePersonelId",
                table: "PersonelTelefonlar",
                column: "SitePersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelTipleri_Ad",
                table: "PersonelTipleri",
                column: "Ad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSoruKategorileri_Ad",
                table: "SikcaSorulanSoruKategorileri",
                column: "Ad");

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSoruKategorileri_Sira",
                table: "SikcaSorulanSoruKategorileri",
                column: "Sira");

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSorular_DilId",
                table: "SikcaSorulanSorular",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSorular_KategoriId_Sira",
                table: "SikcaSorulanSorular",
                columns: new[] { "KategoriId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSorular_SeoUrl",
                table: "SikcaSorulanSorular",
                column: "SeoUrl");

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSorular_SiteId_DilId",
                table: "SikcaSorulanSorular",
                columns: new[] { "SiteId", "DilId" });

            migrationBuilder.CreateIndex(
                name: "IX_Siteler_BirimId",
                table: "Siteler",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Siteler_TemplateId",
                table: "Siteler",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteOzellikleri_SiteId",
                table: "SiteOzellikleri",
                column: "SiteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SitePersonelleri_PersonelTipId",
                table: "SitePersonelleri",
                column: "PersonelTipId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePersonelleri_SiteId_PersonelId",
                table: "SitePersonelleri",
                columns: new[] { "SiteId", "PersonelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SitePersonelleri_UnvanId",
                table: "SitePersonelleri",
                column: "UnvanId");

            migrationBuilder.CreateIndex(
                name: "IX_Unvanlar_TipId",
                table: "Unvanlar",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_YoneticiSiteler_SiteId_KeycloakUserId",
                table: "YoneticiSiteler",
                columns: new[] { "SiteId", "KeycloakUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_YoneticiSiteler_YoneticiTipiId",
                table: "YoneticiSiteler",
                column: "YoneticiTipiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandLogos");

            migrationBuilder.DropTable(
                name: "Icerik");

            migrationBuilder.DropTable(
                name: "MediaFile");

            migrationBuilder.DropTable(
                name: "Menuler");

            migrationBuilder.DropTable(
                name: "PersonelTelefonlar");

            migrationBuilder.DropTable(
                name: "SikcaSorulanSorular");

            migrationBuilder.DropTable(
                name: "SiteOzellikleri");

            migrationBuilder.DropTable(
                name: "YoneticiSiteler");

            migrationBuilder.DropTable(
                name: "YonetimDuyurular");

            migrationBuilder.DropTable(
                name: "Hedefler");

            migrationBuilder.DropTable(
                name: "SitePersonelleri");

            migrationBuilder.DropTable(
                name: "Diller");

            migrationBuilder.DropTable(
                name: "SikcaSorulanSoruKategorileri");

            migrationBuilder.DropTable(
                name: "YoneticiTipleri");

            migrationBuilder.DropTable(
                name: "Siteler");

            migrationBuilder.DropTable(
                name: "Unvanlar");

            migrationBuilder.DropTable(
                name: "Birimler");

            migrationBuilder.DropTable(
                name: "Templateler");

            migrationBuilder.DropTable(
                name: "PersonelTipleri");
        }
    }
}
