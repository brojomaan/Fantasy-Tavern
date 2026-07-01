namespace Interfaces
{
    public interface IFillable
    {
        float FillLevel { get; }
        float TargetFillLevel { get; }
        float AcceptableRange { get; }
        float MaxCapacity { get; }
        bool IsOverflowing { get; }

        void Fill(float amount);
    }
}