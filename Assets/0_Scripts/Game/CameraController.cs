using Common;
using UnityEngine;

namespace Game
{
    public class CameraController : SingletonMonoBehaviour<CameraController>
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;

        public bool IsVisible(Vector3 position, Vector3 boundSize)
        {
            var bounds = new Bounds(position, boundSize);
            var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }

        public Bounds GetBounds()
        {
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = _camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                _camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }
    }
}
