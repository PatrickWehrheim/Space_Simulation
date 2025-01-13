
public class MVCModelSample
{
	private string _sampleText;
    public string SampleText { get => _sampleText; set => _sampleText = value; }

	public MVCModelSample(string sampleText)
	{
		_sampleText = sampleText;
	}
}