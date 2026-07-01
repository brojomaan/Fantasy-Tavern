using UnityEngine;

namespace Components.SharedComponents
{
    public enum VFXMode
    {
        OneShot,
        Loop,
        AlwaysOn
    }

    public class VFXComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private VFXMode mode;

        public bool Initialize()
        {
            if (particles)if (particles == null) { Debug.LogError("VFXComponent::Initialize(): particles is null."); return false; }

            var main = particles.main;
            switch (mode)
            {
                case VFXMode.OneShot:
                    main.loop = false;
                    break;
                case VFXMode.Loop:
                    main.loop = true;
                    break;
                case VFXMode.AlwaysOn:
                    main.loop = true;
                    particles.Play();
                    break;
            }

            return true;
        }

        public void Play()
        {
            if (mode == VFXMode.AlwaysOn) return;
            particles.Play();
        }

        public void Stop()
        {
            if (mode == VFXMode.AlwaysOn) return;
            particles.Stop();
        }

        public bool IsPlaying() => particles.isPlaying;
    }
}