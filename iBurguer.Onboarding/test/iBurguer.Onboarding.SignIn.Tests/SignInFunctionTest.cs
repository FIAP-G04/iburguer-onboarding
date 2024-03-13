using Xunit;
using Amazon.Lambda.TestUtilities;
using iBurguer.Onboarding.Application.SignIn;

namespace iBurguer.Onboarding.SignIn.Tests;

public class SignInFunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        var function = new Function();
        var context = new TestLambdaContext();

        var request = new SignInRequestGateway("11066001774");
        var response = function.FunctionHandler(request, context).Result;

        Assert.Equal(true, true);
    }
}
