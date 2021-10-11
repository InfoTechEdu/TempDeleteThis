public interface IController
{
    public void OnInitialize(GameDataStorage gameDataStorage);

    public void OnActivate();

    public void OnDiactivate();
}
