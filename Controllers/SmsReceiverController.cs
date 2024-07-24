using Microsoft.AspNetCore.Mvc;
using Azure.Communication.Sms;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;

[ApiController]
[Route("[controller]")]
public class SmsReceiverController : ControllerBase
{
    private readonly SmsClient _smsClient;
    private readonly EventGridPublisherClient _eventGridClient;

    public SmsReceiverController()
    {
        string connectionString = "endpoint=https://communicationz.unitedstates.communication.azure.com/;accesskey=2wyhil3hEwzoJMzsHC55ep2OnVyOeFyUr8pEQL9CY4yNuIVRvMJuJQQJ99AGACULyCpDv9Z9AAAAAZCSx18O";
        _smsClient = new SmsClient(connectionString);

        // Replace with your Event Grid Topic endpoint and key
        _eventGridClient = new EventGridPublisherClient(
            new Uri("https://codereceiver.eastus-1.eventgrid.azure.net/api/events"),
            new Azure.AzureKeyCredential("n57P+vFUYgik6ds8ojpr7SjXEH/poG054AZEGIll6ck=")
        );
    }

    [HttpPost("receive")]
    public IActionResult ReceiveSms([FromBody] EventGridEvent eventGridEvent)
    {
        if (eventGridEvent.EventType == EventGridEventTypes.SmsReceivedEventData)
        {
            var smsReceivedEventData = eventGridEvent.Data.ToObjectFromJson<AcsSmsReceivedEventData>();
            Console.WriteLine($"Received SMS from: {smsReceivedEventData.From}");
            Console.WriteLine($"Message: {smsReceivedEventData.Message}");
        }

        return Ok();
    }
}
