
public class MVCViewSample : IMVCViewSample
{
    public string SampleText { get; set; }

    private MVCControllerSample _controller;

    public void SetController(MVCControllerSample controller)
    {
        _controller = controller;
    }
}
