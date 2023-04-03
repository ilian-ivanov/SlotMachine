namespace SlotMachine.Models
{
    public interface ISlotMachine
    {
        decimal CurrentBalance { get; set; }
        SpinResult Play(decimal stake);
    }
}