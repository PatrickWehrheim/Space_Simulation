
public class ProxySample : IProxySubjectSample
{
    private ProxySubjectSample _subject;

    public ProxySample(ProxySubjectSample subject)
    {
        _subject = subject;
    }

    public void Request()
    {
        _subject.Request();
    }
}
