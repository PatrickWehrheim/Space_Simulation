
public class MVCControllerSample
{
    private IMVCViewSample _view;

    public MVCControllerSample(IMVCViewSample view)
    {
        _view = view;
        _view.SetController(this);
    }
}
