namespace Multigroup.DomainModel.Menu
{
    public class MenuComponentPage
    {
        public int MenuComponentPageId { get; set; }
        public string Name { get; set; }
        public string HtmlContent { get; set; }
        public int UserId { get; set; }
        public int MenuComponentId { get; set; }
        public MenuComponent MenuComponent { get; set; }
    }
}
