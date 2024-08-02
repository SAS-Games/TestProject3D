using System.Collections.Generic;
using UnityEngine;

namespace SAS.Waypoints
{
    public enum WaypointPathType
    {
        Closed,
        Open
    }

    public enum WaypointBehaviorType
    {
        Loop,
        PingPong
    }

    enum FollowType
    {
        Lerp,
        MoveTowards
    }

    public class PlatformController : MonoBehaviour
    {
        //[HideInInspector]
        public List<Vector3> waypoints = new List<Vector3>();

        [Header("Editor Settings")]
        public float handleRadius = .5f;
        public Vector2 snappingSettings = new Vector2(.1f, .1f);
        public Color gizmoDeselectedColor = Color.blue;

        public bool editing = false;


        public WaypointPathType pathType = WaypointPathType.Closed;
        [SerializeField] private WaypointBehaviorType m_BehaviorType = WaypointBehaviorType.Loop;
        [SerializeField] private FollowType m_FollowType = default;


        [SerializeField] private float m_Speed = 3f; // Speed of movement
        [SerializeField] private float m_MaxDistanceToGoal = 0.1f;
        [SerializeField] private bool m_MoveTowardsPath = true;
        [SerializeField] private Transform m_Platform;
        [SerializeField] private bool m_LocalSpace = true;


        private IEnumerator<Vector3> _currentPoint;
        private float _squaredMaxDistanceToGoal => m_MaxDistanceToGoal * m_MaxDistanceToGoal;
        private Vector3 _position;
        private Path _path = default;


        private void Start()
        {
            if (waypoints.Count < 2)
            {
                Debug.LogError($"Path should contains minimum 2 points");
                return;
            }
            CreatePath();
            if (m_MoveTowardsPath)
            {
                if (m_LocalSpace)
                    _position = m_Platform.localPosition;
                else
                    _position = m_Platform.position;
            }
        }

        private void Update()
        {
            if (_currentPoint == null || _currentPoint.Current == null)
                return;

            if (m_LocalSpace)
                m_Platform.localPosition = _position;
            else
                m_Platform.position = _position;

            if (m_FollowType == FollowType.MoveTowards)
                _position = Vector3.MoveTowards(_position, _currentPoint.Current, Time.deltaTime * m_Speed);
            else
                _position = Vector3.Lerp(_position, _currentPoint.Current, Time.deltaTime * m_Speed);

            var distanceSquared = (_position - _currentPoint.Current).sqrMagnitude;
            if (distanceSquared < _squaredMaxDistanceToGoal)
            {
                if (!_currentPoint.MoveNext())
                    CreatePath();
            }
        }

        private void CreatePath()
        {
            if (m_BehaviorType is WaypointBehaviorType.Loop)
                _path = new CyclicPath(waypoints, pathType == WaypointPathType.Closed);
            else
                _path = new PingPongPath(waypoints, pathType == WaypointPathType.Closed);

            _currentPoint = _path.GetPathEnumerator();
            _currentPoint.MoveNext();
            _position = _currentPoint.Current;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (IsSelected() && editing)
                return;

            List<Vector3> waypoints = new List<Vector3>(this.waypoints);
            if (m_LocalSpace)
                waypoints = ConvertToLocalSpace(this.waypoints);

            if (pathType == WaypointPathType.Closed)
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    Gizmos.color = gizmoDeselectedColor;

                    Vector3 nextPoint = waypoints[(i + 1) % waypoints.Count];
                    Gizmos.DrawLine(waypoints[i], nextPoint);

                    Gizmos.DrawSphere(waypoints[i], handleRadius / 2);
                }
            }
            else
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    Gizmos.color = gizmoDeselectedColor;

                    Vector3 nextPoint = waypoints[(i + 1) % waypoints.Count];
                    if (i != waypoints.Count - 1)
                        Gizmos.DrawLine(waypoints[i], nextPoint);

                    Gizmos.DrawSphere(waypoints[i], handleRadius / 2);
                }
            }
        }

        public List<Vector3> ConvertToLocalSpace(List<Vector3> worldPoints)
        {
            List<Vector3> localPoints = new List<Vector3>();

            foreach (Vector3 worldPoint in worldPoints)
            {
                Vector3 localPoint = transform.position + worldPoint;
                localPoints.Add(localPoint);
            }

            return localPoints;
        }


        private bool IsSelected()
        {
            return UnityEditor.Selection.activeGameObject == transform.gameObject;
        }
#endif
    }
}
