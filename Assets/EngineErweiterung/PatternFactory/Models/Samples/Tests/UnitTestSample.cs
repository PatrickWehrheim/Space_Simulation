
// To get NUnit in Unity, open the Test Runner Window, Create a Test Folder and put this file in the Test Folder
using NUnit.Framework;

public class UnitTestSample
{
    [Test]
    public void sample_test()
    {
        // ARRANGE
        int a = 1;
        int b = 2;

        // ACT
        int c = a + b;

        // ASSERT
        Assert.AreEqual(c, 3);
    }
}
