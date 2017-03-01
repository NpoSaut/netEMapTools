using Geographics;

namespace BlokMap.Search.Interfaces
{
    public class SearchResult
    {
        public SearchResult(string Title, EarthPoint Point)
        {
            this.Title = Title;
            this.Point = Point;
        }

        public string Title { get; private set; }
        public EarthPoint Point { get; private set; }
    }
}
