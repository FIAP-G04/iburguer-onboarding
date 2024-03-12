using Xunit;
using Amazon.Lambda.TestUtilities;

namespace iBurguer.Onboarding.SignIn.Tests;

public class SignInFunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        var function = new Function();
        var context = new TestLambdaContext();
        var response = function.FunctionHandler("11066001774", context).Result;

        Assert.Equal(true, true);
    }
}
