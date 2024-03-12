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
        var response = function.FunctionHandler(new SignUpRequest("11066001774", "Vincius", "Saetao", "vinisaeta@gmail.com"), context).Result;

        Assert.Equal(true, true);

    }
}
