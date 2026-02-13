using System.Text.RegularExpressions;

namespace BusinessLogic.Services
{
    public class TemplateDefinition
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> Placeholders { get; set; } = new();
    }

    public interface ITemplateService
    {
        List<TemplateDefinition> GetTemplates();
        Task<string> GetTemplateContentAsync(string templateId);
        Task<byte[]> ExportAsync(string templateId, string html, Dictionary<string, string> placeholders);
    }

    public class TemplateService : ITemplateService
    {
        private readonly string _templatesPath;

        private static readonly Dictionary<string, TemplateDefinition> Templates = new()
        {
            ["zgoda_wycieczka"] = new TemplateDefinition
            {
                Id = "zgoda_wycieczka",
                Name = "Zgoda na wycieczkę",
                Placeholders = new() { "{{imie_i_nazwisko}}", "{{klasa}}", "{{data_wycieczki}}", "{{koszt}}", "{{termin_wplat}}", "{{numer_konta}}", "{{data}}" }
            },
            ["zgoda_obiady"] = new TemplateDefinition
            {
                Id = "zgoda_obiady",
                Name = "Zgoda na obiady szkolne",
                Placeholders = new() { "{{imie_i_nazwisko}}", "{{klasa}}", "{{rok_szkolny}}", "{{semestr}}", "{{data}}" }
            },
            ["zagrozenie_ocena"] = new TemplateDefinition
            {
                Id = "zagrozenie_ocena",
                Name = "Zagrożenie oceną niedostateczną",
                Placeholders = new() { "{{imie_i_nazwisko}}", "{{klasa}}", "{{przedmiot}}", "{{data}}" }
            }
        };

        private static readonly Dictionary<string, string> QuillAlignments = new()
        {
            ["ql-align-center"] = "text-align:center",
            ["ql-align-right"] = "text-align:right",
            ["ql-align-justify"] = "text-align:justify"
        };

        private static readonly Dictionary<string, string> QuillSizes = new()
        {
            ["ql-size-small"] = "font-size:0.75em",
            ["ql-size-large"] = "font-size:1.5em",
            ["ql-size-huge"] = "font-size:2.5em"
        };

        public TemplateService(string templatesPath)
        {
            _templatesPath = templatesPath;
        }

        public List<TemplateDefinition> GetTemplates() => Templates.Values.ToList();

        public async Task<string> GetTemplateContentAsync(string templateId)
        {
            if (!Templates.ContainsKey(templateId))
                throw new KeyNotFoundException("Szablon nie istnieje");

            var filePath = Path.Combine(_templatesPath, $"{templateId}.html");
            return await File.ReadAllTextAsync(filePath);
        }

        private static string NormalizeQuillHtml(string html)
        {
            var result = html;

            foreach (var (cssClass, style) in QuillAlignments)
            {
                result = Regex.Replace(result,
                    $@"class\s*=\s*""[^""]*{cssClass}[^""]*""",
                    m =>
                    {
                        var cleaned = Regex.Replace(m.Value, $@"\b{cssClass}\b", "").Replace("  ", " ");
                        return cleaned.Replace(@"""  """, @""" """) + $@" style=""{style}""";
                    });
            }

            foreach (var (cssClass, style) in QuillSizes)
            {
                result = result.Replace($@"class=""{cssClass}""", $@"style=""{style}""");
            }

            return result;
        }

        public async Task<byte[]> ExportAsync(string templateId, string html, Dictionary<string, string> placeholders)
        {
            if (!Templates.ContainsKey(templateId))
                throw new KeyNotFoundException("Szablon nie istnieje");

            var processed = NormalizeQuillHtml(html);

            placeholders["{{data}}"] = DateTime.Now.ToString("dd.MM.yyyy");

            foreach (var (key, value) in placeholders)
                processed = processed.Replace(key, value);

            using var ms = new MemoryStream();
            using (var doc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(
                    new DocumentFormat.OpenXml.Wordprocessing.Body()
                );

                var converter = new HtmlToOpenXml.HtmlConverter(mainPart);
                var paragraphs = converter.Parse(processed);

                foreach (var p in paragraphs)
                    mainPart.Document.Body!.Append(p);
            }

            return ms.ToArray();
        }
    }
}
