namespace AbakTools.Web.Models.Shared
{
    public class BreadcrumbsItemModel
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public BreadcrumbsItemModel(string name)
        {
            Name = name;
        }

        public BreadcrumbsItemModel(string name, string url)
            : this(name)
        {
            Url = url;
        }
    }
}
