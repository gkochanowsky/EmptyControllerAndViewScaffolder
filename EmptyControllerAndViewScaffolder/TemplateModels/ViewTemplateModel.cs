namespace EmptyControllerAndViewScaffolder.TemplateModels
{
    public class ViewTemplateModel
    {
        public string ViewName { get; set; }
        public string LayoutPageFile { get; set; }
        public bool IsPartialView { get; set; }
        public bool IsLayoutPageSelected { get; set; }
    }
}