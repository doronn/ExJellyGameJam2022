namespace AiBrain
{
    public interface IBrainInputListener
    {
        void RegisterInput(int inputType, float inputValue);
        void ExecuteInputs();
        void InputReset();
        bool DidReceiveBadInputs();
        int InputAmount { get; }
    }
}