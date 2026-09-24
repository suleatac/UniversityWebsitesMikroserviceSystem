using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class TemplateSeedData
    {

        public static List<Template> Templates => new List<Template>
        {
            new Template
              {
                TemplateAdi = "Template_1",
                TemplateTuru = "İdari",

              },
               new Template
              {
                TemplateAdi = "Template_2",
                TemplateTuru = "İdari",

              },

        };

        public static List<Template> GetTemplateSeedDatas()
        {
            return Templates;
        }

    }
}
