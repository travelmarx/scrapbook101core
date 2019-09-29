using System.Linq;
using Scrapbook101core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace Scrapbook101core.Controllers
{
    /// <summary>
    /// In the Model-View-Controller pattern, the controller processes the input. In this case, it is all the input involving
    /// category operations.
    /// </summary>
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET for partial view on create page to get category fields for a chosen category
        [HttpGet]
        [ActionName("GetCategoryFields")]
        public string GetCategoryFieldsAsync(string category)
        {
            System.Text.StringBuilder buildString = new System.Text.StringBuilder();
            var categories = AppVariables.CategoryFieldMappingList;
            var foundCategory = categories.First(x => string.Compare(x.Name, category, true) == 0) as CategoryFieldMapping;

            foreach (string field in foundCategory.ActiveFields)
            {
                var builder1 = new TagBuilder("label");
                builder1.MergeAttribute("for", "Item_CategoryFields_" + field + "_");
                builder1.MergeAttribute("style", "color: blue");
                builder1.InnerHtml.Append(field);
                var builder2 = new TagBuilder("input");
                builder2.MergeAttribute("name", "Item.CategoryFields[" + field + "]");
                builder2.MergeAttribute("id", "Item_CategoryFields_" + field + "_");
                builder2.MergeAttribute("type", "text");
                builder2.MergeAttribute("value", "");
                builder2.MergeAttribute("class", "form-control");

                buildString.Append(GetString(builder1) + GetString(builder2) + "<br/>");
            }
            return buildString.ToString();
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

    }
}