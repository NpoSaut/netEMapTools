using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using BlokMap.MapElements;
using BlokMap.MapElements.MapObjectElements;
using Geographics;
using GMapElements;
using MapVisualization.Elements;

namespace BlokMap
{
    public class MapPresenter
    {
        #region Цвета для сегментов

        private readonly Color[] _sectionColors =
        {
            Color.FromRgb(255, 255, 102),
            Color.FromRgb(255, 204, 102),
            Color.FromRgb(255, 204, 153),
            Color.FromRgb(255, 153, 153),
            Color.FromRgb(255, 102, 51),
            Color.FromRgb(255, 51, 51),
            Color.FromRgb(255, 102, 204),
            Color.FromRgb(153, 102, 255),
            Color.FromRgb(102, 153, 255),
            Color.FromRgb(102, 204, 204),
            Color.FromRgb(0, 255, 51)
        };

        #endregion

        public IEnumerable<MapElement> PrintPosts(GMap gMap)
        {
            var r = new Random();
            foreach (GSection section in gMap.Sections)
            {
                var sectionBrush = new SolidColorBrush(_sectionColors[r.Next(_sectionColors.Length)]);
                foreach (GPost post in section.Posts)
                    yield return new KilometerPostMapElement(post) { SectionBrush = sectionBrush };
            }
        }

        public IEnumerable<MapElement> PrintObjects(GMap gMap, int trackNumber)
        {
            foreach (GSection section in gMap.Sections)
            {
                for (int i = 0; i < section.Posts.Count; i++)
                {
                    GPost post = section.Posts[i];
                    if (i + 1 < section.Posts.Count)
                    {
                        GPost nextPost = section.Posts[i + 1];
                        double dist = post.Point.DistanceTo(nextPost.Point);

                        GTrack track = post.Tracks.FirstOrDefault(t => t.Number == trackNumber);

                        if (track != null)
                        {
                            foreach (GObject gObject in track.Objects)
                            {
                                double objectRatio = (gObject.Ordinate - post.Ordinate) / dist;
                                EarthPoint objectPosition = EarthPoint.MiddlePoint(post.Point, nextPost.Point, objectRatio);
                                MapElement objectElement;
                                switch (gObject.Type)
                                {
                                    case GObjectType.TrafficLight:
                                        objectElement = new MapTrafficLightElement(objectPosition, gObject);
                                        break;
                                    case GObjectType.Platform:
                                    case GObjectType.Station:
                                        objectElement = new MapPlatformElement(objectPosition, gObject);
                                        break;
                                    default:
                                        objectElement = new MapUnknownObjectElement(objectPosition, gObject);
                                        break;
                                }
                                yield return objectElement;
                            }
                        }
                    }
                }
            }
        }
    }
}
