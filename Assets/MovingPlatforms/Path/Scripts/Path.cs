using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.Waypoints
{
    interface IPath
    {
        IEnumerator<Vector3> GetPathEnumerator();
    }

    public abstract class Path : IPath
    {
        protected List<Vector3> _points;
        protected bool _isClosed;

        public Path(List<Vector3> points, bool isClosed)
        {
            _points = points;
            _isClosed = isClosed;
        }

        public abstract IEnumerator<Vector3> GetPathEnumerator();
    }
}
