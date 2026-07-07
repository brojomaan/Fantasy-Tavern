using Components.CharacterComponents;
using UnityEngine;

namespace NPC
{
    public class NpcVisual : MonoBehaviour
    {
        [SerializeField] private AnimationComponent animationComponent;
        [SerializeField] private FaceAnimationComponent faceAnimationComponent;
        
        public AnimationComponent AnimationComponent => animationComponent;
        public FaceAnimationComponent FaceAnimationComponent => faceAnimationComponent;

        public bool Initialize()
        {
            if (animationComponent == null) 
            { 
                Debug.LogError($"NpcVisual::Initialize: Anim Comp is null");
                return false;
            }

            if (faceAnimationComponent == null)
            {
                Debug.LogError($"NpcVisual::Initialize: FaceAnimationComponent is null");
                return false;
            }

            
            //Initialize Loop
            if (!animationComponent.Initialize())
            {
                Debug.LogError($"NpcVisual::Initialize: Anim Comp Failed");
                return false;
            }

            if (!faceAnimationComponent.Initialize())
            {
                Debug.LogError($"NpcVisual::Initialize: FaceAnimationComponent Failed");
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