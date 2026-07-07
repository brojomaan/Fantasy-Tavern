using UnityEngine;

namespace Components.CharacterComponents
{
    public class MicrophoneComponent : MonoBehaviour
    {
        [SerializeField] private float talkingThreshold = 0.01f;
        [SerializeField] private float smoothing = 10f;
        [SerializeField] private bool microphoneEnabled = true;

        private AudioClip microphoneClip;
        private string microphoneDevice;
        private float currentAmplitude;


        public bool IsTalking => currentAmplitude > talkingThreshold;
        public float Amplitude => currentAmplitude;
        
        
        public bool Initialize()
        {
            if (!microphoneEnabled) return true;
            if (Microphone.devices.Length == 0)
            {
                Debug.LogError($"MicrophoneComponent::Intialize(): No microphone Device Found");
                return false;
            }
            microphoneDevice = Microphone.devices[0];
            microphoneClip = Microphone.Start(microphoneDevice, true, 1, AudioSettings.outputSampleRate);
            return true;

        }

        public void OnUpdate()
        {
            float target = GetMicrophoneAmplitude();
            currentAmplitude = Mathf.Lerp(currentAmplitude, target, Time.deltaTime * smoothing);
        }

        private float GetMicrophoneAmplitude()
        {
            if (microphoneClip == null) return 0f;

            int position = Microphone.GetPosition(microphoneDevice);
            if (position <= 0) return 0f;

            float[] samples = new float[256];
            microphoneClip.GetData(samples, Mathf.Max(0, position - 256));

            float sum = 0f;
            foreach (float sample in samples)
            {
                sum += Mathf.Abs(sample);
            }

            return sum / samples.Length;
        }

        private void OnDestroy()
        {
            if (Microphone.IsRecording(microphoneDevice))
            {
                Microphone.End(microphoneDevice);
            }
        }
    }
}