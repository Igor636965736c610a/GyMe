using GyMeApplication.IServices;
using GyMeApplication.Models.InternalManagement;
using GyMeApplication.Options;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly string _stripePublicKey;
    private readonly IUserContextService _userContextService;
    private readonly IPaymentService _paymentService;

    public PaymentsController(IOptions<StripeOptions> options, IUserContextService userContextService, IPaymentService paymentService)
    {
        _stripePublicKey = options.Value.PublicKey;
        StripeConfiguration.ApiKey = options.Value.SecretKey;
        _userContextService = userContextService;
        _paymentService = paymentService;
    }

    [HttpPost(ApiRoutes.Payments.RedirectToPayment)]
    public async Task<IActionResult> RedirectToPayment([FromBody] PaymentRequestModel paymentRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data");

        if (!_userContextService.EmailConfirmed)
            return UnprocessableEntity("Email address is not confirmed. Please, confirm your email! ;)");

        var session = await _paymentService.CreateRedirectToPayment(paymentRequest);
        
        return Ok(new { SessionId = session.Id, PublishableKey = _stripePublicKey });
    }

    [HttpPost(ApiRoutes.Payments.Webhook)]
    public async Task<IActionResult> StripeWebhook()
    {
        await _paymentService.WebhookStripePayments();
        
        return Ok("Webhook processed successfully");
    }
}