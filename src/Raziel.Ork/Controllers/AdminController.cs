using Microsoft.AspNetCore.Mvc;

// This controller is used strictly for account creation and should be disabled once all accounts are established for the hack
namespace Raziel.Ork.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase {
        //private readonly IAdminTideAuthentication _tideAuthentication;

        //public AdminController(IAdminTideAuthentication tideAuthentication) {
        //    _tideAuthentication = tideAuthentication;
        //}

        //[HttpGet("/CreateAccount")]
        //public IActionResult CreateBlockchainAccount(string publicKey, string username) {
        //    return new JsonResult(_tideAuthentication.CreateAccount(publicKey, username, false));
        //}

        //[HttpPost("/PushFragment")]
        //public IActionResult PushFragment(AuthenticationModel model) {
        //    model.SiteUrl = $"{Request.Scheme}://{Request.Host}";
        //    return new JsonResult(_tideAuthentication.AddFragment(model));
        //}
    }
}