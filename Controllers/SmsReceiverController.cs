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
        string connectionString = "<YOUR_CONNECTION_STRING>";
        _smsClient = new SmsClient(connectionString);

        // Replace with your Event Grid Topic endpoint and key
        _eventGridClient = new EventGridPublisherClient(
            new Uri("<YOUR_EVENT_GRID_TOPIC_ENDPOINT>"),
            new Azure.AzureKeyCredential("<YOUR_EVENT_GRID_TOPIC_KEY>")
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
