using UnityEngine;

namespace Gameplay.Views
{
    public class TableView : MonoBehaviour
    {
        public Transform Container;
        public Vector3 SpawnZoneSize;
        public Vector3 SpawnZoneOffset;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(SpawnZoneOffset + transform.position, SpawnZoneSize);
        }
    }
}