using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace GymAppApi.Controllers.v1;

[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly string _stripePublicKey;
    private readonly IUserContextService _userContextService;

    public PaymentsController(IOptions<StripeOptions> options, IUserContextService userContextService)
    {
        _stripePublicKey = options.Value.PublicKey;
        StripeConfiguration.ApiKey = options.Value.SecretKey;
        _userContextService = userContextService;
    }

    [HttpPost(ApiRoutes.Payments.RedirectToPayment)]
    public IActionResult RedirectToPayment([FromBody] PaymentRequest paymentRequest)
    {
        var customerOptions = new CustomerCreateOptions
        {
            Email = _userContextService.Email
        };

        var customerService = new CustomerService();
        var customer = customerService.Create(customerOptions);

        var options = new SessionCreateOptions
        {
            Customer = customer.Id,
            PaymentMethodTypes = new List<string>
            {
                "card",
                "blik",
                "paypal"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = paymentRequest.Amount,
                        Currency = paymentRequest.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "application developer support :D",
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "https://adres-twojej-strony.com/success",
            CancelUrl = "https://adres-twojej-strony.com/cancel",
        };

        var sessionService = new SessionService();
        var session = sessionService.Create(options);
        return Ok(new { SessionId = session.Id, PublishableKey = _stripePublicKey });
    }

    [HttpPost(ApiRoutes.Payments.Webhook)]
    public Task StripeWebhook()
    {
        var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();
        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "Foo");
        if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        {
            if (stripeEvent.Data.Object is not Session session)
                throw new InvalidProgramException("Session payment error");
            
            var paymentIntentId = session.PaymentIntent.Id;
            var amount = session.AmountTotal;
            var email = session.Customer.Email;
            
            // mongo db message
        }

        return Task.CompletedTask;
    }
}

public class PaymentRequest
{
    public int Amount { get; set; }
    public string Currency { get; set; }
}