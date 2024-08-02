using System.Collections.Generic;
using UnityEngine;


namespace SAS.Waypoints
{
    public class PingPongPath : Path
    {
        public PingPongPath(List<Vector3> points, bool isClosed) : base(points, isClosed)
        {
            if (points.Count >= 2 && isClosed)
                points.Add(points[0]);
        }

        public override IEnumerator<Vector3> GetPathEnumerator()
        {
            if (_points.Count < 2)
                yield break;
            var direction = 1;
            var index = 0;

            while (true)
            {
                yield return _points[index];

                if (index <= 0)
                    direction = 1;
                else if (index >= _points.Count - 1)
                    direction = -1;

                index += direction;
            }
        }
    }
}
