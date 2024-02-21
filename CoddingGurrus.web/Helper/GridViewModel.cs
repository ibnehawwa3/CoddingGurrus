using System.Linq;
using System.Reflection;

namespace CoddingGurrus.web.Helper
{
    public class GridViewModel<T>
    {
        public List<T> Data { get; set; }
        public GridConfiguration Configuration { get; set; }
    }

    public class GridConfiguration
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public int NoOfPages { get; set; }
        public string HeaderText { get; set; }
        public string CreateAction{ get; set; }
        public string UpdateAction { get; set; }
        public string DeleteAction { get; set; }
        public string CreateButtonText { get; set; }
        public bool ShowHeaders { get; set; }
        public string ControllerName { get; set; }
        public List<string> DisplayFields { get; set; }
    }

    public static class DisplayFieldsHelper
    {
        public static List<string> GetDisplayFields<T>(Func<PropertyInfo, bool> criteria)
        {
            return typeof(T)
                .GetProperties()
                .Where(criteria)
                .Select(property => property.Name)
                .ToList();
        }
    }
}
