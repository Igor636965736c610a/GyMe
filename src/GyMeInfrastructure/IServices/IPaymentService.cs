using GymAppInfrastructure.Models.Payment;
using Stripe.Checkout;

namespace GymAppInfrastructure.IServices;

public interface IPaymentService
{
    Task<Session> CreateRedirectToPayment(PaymentRequestModel paymentRequest);
    Task WebhookStripePayments();
}