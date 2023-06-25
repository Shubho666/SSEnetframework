using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System;
using System.Web.Http;

public class SseListenerController : System.Web.Http.ApiController
{
    [HttpGet]
    public System.Net.Http.HttpResponseMessage SubscribeToListener(HttpRequestMessage request)
    {
        System.Net.Http.HttpResponseMessage response = request.CreateResponse();
        response.Content = new System.Net.Http.PushStreamContent((Action<Stream, HttpContent, TransportContext>)WriteToStream, new MediaTypeHeaderValue("text/event-stream"));
        return response;
    }

    [HttpPost]
    public void Post(string thing)
    {
        MessageCallback(thing);
    }

    private static readonly ConcurrentDictionary<StreamWriter, StreamWriter> _streammessage = new ConcurrentDictionary<StreamWriter, StreamWriter>();
    private void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
    {
        StreamWriter streamwriter = new StreamWriter(outputStream);
        _streammessage.TryAdd(streamwriter, streamwriter);
    }

    public static void MessageCallback(string thing)
    {
        foreach (var subscriber in _streammessage.ToArray())
        {
            try
            {
                subscriber.Value.WriteLine(string.Format("id: {0}\n", thing));
                subscriber.Value.WriteLine("data: " + JsonConvert.SerializeObject(thing) + "\n\n");
                subscriber.Value.Flush();

                //subscriber.Value.WriteLine(string.Format("id: {0}\n", thing));
                //subscriber.Value.WriteLine("data: " + JsonConvert.SerializeObject(thing) + "\n\n");
                //subscriber.Value.Flush();
            }
            catch
            {
                StreamWriter streamWriter;
                _streammessage.TryRemove(subscriber.Value, out streamWriter);
            }
        }
    }
}