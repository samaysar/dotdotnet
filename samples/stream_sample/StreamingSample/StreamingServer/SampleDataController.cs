using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace StreamingServer
{
    [RoutePrefix("sample")]
    public class SampleDataController : ApiController
    {
        [HttpGet]
        [Route("sample_array")]
        public HttpResponseMessage GetData([FromUri]int count)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Tuple<string, int>("count", count),
                new MediaTypeHeaderValue("application/json"));
        }
    }
}