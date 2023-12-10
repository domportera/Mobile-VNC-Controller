namespace CustomDotNetExtensions
{
  /*  public static class Mathy
    {
        private static Circle MinimumBoundingCircle(List<Vector2> points, List<Vector2> boundary)
        {
            if (points.Count == 0 || boundary.Count == 3)
            {
                return boundary.ToCircle();
            }
            var p = points.Last();
            points.RemoveAt(points.Count - 1);
            var c = MinimumBoundingCircle(points, boundary);
            if (c.Contains(p))
            {
                return c;
            }
            boundary.Add(p);
            return MinimumBoundingCircle(points, boundary);
        }

        private static Circle ToCircle(this List<Vector2> points)
        {
            if (points.Count == 0) return new Circle(Vector2.zero, 0);
            if (points.Count == 1) return new Circle(points[0], 0);
            if (points.Count == 2) return new Circle(points[0], points[1]);
            return new Circle(points[0], points[1], points[2]);
        }
        
        // Note that this implementation assumes that the input list contains distinct points.
        // If there are duplicate points, you may need to remove them before running the algorithm
        // to avoid degenerate cases.
        public static List<Vector2> ConvexHull(List<Vector2> points)
        {
            var hull = new List<Vector2>();
            if (points.Count < 3)
            {
                hull.AddRange(points);
                return hull;
            }

            // Find the point with the minimum x-coordinate
            var start = points[0];
            foreach (var point in points)
            {
                if (point.x < start.x || (point.x == start.x && point.y < start.y))
                {
                    start = point;
                }
            }

            // Add the starting point to the hull
            hull.Add(start);

            // Add the next point to the hull until we return to the starting point
            var current = start;
            do
            {
                var next = points[0];
                foreach (var point in points)
                {
                    if (point == current)
                    {
                        continue;
                    }
                    var cross = (point - current).Cross(next - current);
                    if (cross > 0 || (cross == 0 && (point - current).magnitude > (next - current).magnitude))
                    {
                        next = point;
                    }
                }
                current = next;
                hull.Add(current);
            } while (current != start);

            return hull;
        }
    }*/
}