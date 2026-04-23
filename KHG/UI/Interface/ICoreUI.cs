using KHG.UIs;
using Assets._00.Work.YHB.Scripts.Core;

public interface ICoreUI
{
    void Initialize(ICoreUIContext coreUIContext);
}

public interface ICoreUIContext
{
    InputSO Input { get; }
    void RequestGameStart();
}
