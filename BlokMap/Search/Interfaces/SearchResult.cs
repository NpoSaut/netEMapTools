using Geographics;

namespace BlokMap.Search.Interfaces
{
    public class SearchResult
    {
        public SearchResult(string Title, string Description, EarthPoint Point)
        {
            this.Title = Title;
            this.Description = Description;
            this.Point = Point;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public EarthPoint Point { get; private set; }
    }
}
