using Interactables.WorldInteractable;

namespace Interfaces
{
    public interface IFillable
    {
        float FillLevel { get; }
        float TargetFillLevel { get; }
        float AcceptableRange { get; }
        float MaxCapacity { get; }
        bool IsOverflowing { get; }
        LiquidMixer GetLiquidMixer();
        void Fill(float amount, string liquidId);
    }
}