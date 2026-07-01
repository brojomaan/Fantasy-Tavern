using Components.CharacterComponents;
using UnityEngine;

namespace NPC
{
    public class NpcVisual : MonoBehaviour
    {
        [SerializeField] private AnimationComponent animationComponent;
        
        public AnimationComponent AnimationComponent => animationComponent;

        public bool Initialize()
        {
            if (animationComponent == null) 
            { 
                Debug.LogError($"NpcVisual::Initialize: Anim Comp is null");
                return false;
            }

            if (!animationComponent.Initialize())
            {
                Debug.LogError($"NpcVisual::Initialize: Anim Comp Failed");
                return false;
            }

            return true;
        }
        

        public void OnUpdate(Vector2 moveInput, float speed, bool isGrounded)
        {
            animationComponent.SetWalking(moveInput);
            animationComponent.SetSpeed(speed);
            animationComponent.SetGrounded(isGrounded);
        }
    }
}