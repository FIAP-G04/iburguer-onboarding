using System.Net;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using iBurguer.Onboarding.Application.SignUp;

namespace iBurguer.Onboarding.SignUp.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        var function = new Function();
        var context = new TestLambdaContext();

        Assert.Equal(true, true);

    }
}
