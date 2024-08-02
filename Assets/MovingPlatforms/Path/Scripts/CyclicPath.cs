using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SAS.Waypoints
{
    public class CyclicPath : Path
    {
        public CyclicPath(List<Vector3> points, bool isClosed) : base(points, isClosed)
        {
           
        }

        public override IEnumerator<Vector3> GetPathEnumerator()
        {
            if (_points.Count < 2)
                yield break;

            var index = 0;

            while (true)
            {
                yield return _points[index];

                if (index == _points.Count - 1)
                {
                    index = -1;
                    if(!_isClosed)
                        break;
                }

                ++index;
            }
        }
    }
}
