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
        public string HeaderText { get; set; }
        public bool ShowHeaders { get; set; }
    }
}
