using Interactables.WorldInteractable;
using Liquids;

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
        void Fill(float amount, LiquidData liquidId);
    }
}