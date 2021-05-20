using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proteo5.HL.Test
{
    [TestClass]
    public class ResultTest
    {
        [TestMethod]
        public void CastTest()
        {
            try
            {
                //Test 1 Cast from Result to Result<T>
                Result result = new(ResultsStates.success) { Message = "Test", ValidationResults = new() { IsValid = false } };
                Result<dynamic> resultGeneric = result.GetResult;
                Assert.IsTrue(true, "Result to Result<T> Pass");

                //Test 2 Cast from Result to Result<T>
                Result<dynamic> resultGeneric2 = new(ResultsStates.unsuccess) { Message = "Test2", ValidationResults = new() { IsValid = true } };
                Result result2 = resultGeneric2.GetResult;
                Assert.IsTrue(true, "Result<T> to Result Pass");
            }
            catch (System.Exception)
            {
                Assert.Fail("Cast Test Fail");
            }
        }
    }
}
