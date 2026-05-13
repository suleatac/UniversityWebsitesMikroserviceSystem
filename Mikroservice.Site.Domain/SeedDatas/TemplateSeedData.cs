using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class TemplateSeedData
    {

        public static List<Template> Templates => new List<Template>
        {
            new Template 
              { 
                Id = 1, 
                TemplateAdi = "Default", 
                TemplateTuru = "İdari",
                FolderName = "/Default",
                LayoutPath = "/Layouts/Default",
              }
          
        };

        public static List<Template> GetTemplateSeedDatas()
        {
            return Templates;
        }

    }
}
