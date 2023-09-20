using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Options;
using GymAppInfrastructure.Services.InternalManagement;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace GymAppInfrastructure.Services;

internal class PaymentService : IPaymentService
{
    private readonly string _stripeSecretKey;
    private readonly PaymentMessagesService _paymentMessagesService;
    private readonly IUserContextService _userContextService;

    public PaymentService(IOptionsMonitor<StripeOptions> options, IUserContextService userContextService, PaymentMessagesService paymentMessagesService)
    {
        _stripeSecretKey = options.CurrentValue.SecretKey;
        _userContextService = userContextService;
        _paymentMessagesService = paymentMessagesService;
    }

    public async Task<Session> CreateRedirectToPayment(PaymentRequestModel paymentRequest)
    {
        var customerOptions = new CustomerCreateOptions
        {
            Email = _userContextService.Email
        };

        var customerService = new CustomerService();
        var customer = await customerService.CreateAsync(customerOptions);

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
        var session = await sessionService.CreateAsync(options);

        return session;
    }

    public async Task WebhookStripePayments()
    {
        var json = await new StreamReader(_userContextService.HttpContent.Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(json, _userContextService.HttpContent.Request.Headers["Stripe-Signature"], _stripeSecretKey);
        if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        {
            if (stripeEvent.Data.Object is not Session session)
                throw new InvalidProgramException("Session payment error");
            
            var paymentIntentId = session.PaymentIntent.Id;
            var amount = session.AmountTotal ?? throw new InvalidProgramException("Stripe webhook error - Amount is null");
            var email = session.Customer.Email;

            var paymentMessage = new PaymentMessage()
            {
                PaymentIntentId = paymentIntentId,
                Amount = amount,
                Email = email
            };

            await _paymentMessagesService.Add(paymentMessage);
            return;
        }

        throw new InvalidProgramException("Stripe webhook error");
    }
}