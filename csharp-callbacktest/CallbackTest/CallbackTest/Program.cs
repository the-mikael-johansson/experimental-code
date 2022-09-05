// See https://aka.ms/new-console-template for more information

using System.Web.Mvc;
using Mimiware.ServiceResult;
using static System.Console;

namespace CallbackTest;

public static class Program
{
    public static void Main()
    {
        var controller = new MyController();
        var x = controller.GetUsers();
        WriteLine($"x={x.GetType().FullName}");
    }

    public class MyController
    {
        private readonly TestService _service;

        public MyController()
        {
            _service = new TestService();
        }

        public ActionResult GetUsers()
        {
            return GetResponse(() => _service.GetUsers());
        }

        private static ActionResult GetResponse<T>(Func<IServiceResult<T>> function)
        {
            // Check if valid Model

            // Call method
            var serviceResult = function();

            // Return rest response
            return RestResponse(serviceResult);
        }
    }

    public interface ITestService
    {
        IServiceResult<string> GetUsers();
    }

    public class TestService : ITestService
    {
        public IServiceResult<string> GetUsers()
        {
            var result = new ServiceResult<string>();
            return result.Ok("OK");
        }
    }



    static HttpStatusCodeResult RestResponse(IServiceResult result)
    {
        return result.IsSuccessCode
            ? new HttpStatusCodeResult(result.Code)
            : new HttpStatusCodeResult(result.Code, result.ErrorMessage.ErrorMessage);
    }

    static ActionResult RestResponse<T>(IServiceResult<T> result, JsonRequestBehavior requestBehavior = JsonRequestBehavior.DenyGet)
    {
        if (!result.IsSuccessCode)
        {
            return new HttpStatusCodeResult(result.Code, result.ErrorMessage.ErrorMessage);
        }

        return Json(result.Data, requestBehavior);
    }

    // Mock
    static ActionResult Json(object a, object b)
    {
        return new A();
    }

    public class A : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            
        }
    }
}