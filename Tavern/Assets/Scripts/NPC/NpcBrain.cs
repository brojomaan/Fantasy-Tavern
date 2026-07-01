using UnityEngine;

namespace NPC
{
    public class NpcBrain : MonoBehaviour
    {
        public bool Initialize()
        {
           
            return true;
        }

        public Transform FindAvailableSeat()
        {
            //TODO need to change this too look through the chairs and pick one
            return transform;
        }
    }
}