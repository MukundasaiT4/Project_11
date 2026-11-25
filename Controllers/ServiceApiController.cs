using System.Collections.Generic;
using System.Web.Http;
using Project_11.DAL;
using Project_11.Models;

namespace Project_11.Controllers
{
    public class ServiceApiController : ApiController
    {
        private readonly ServiceRequestDAL _dal = new ServiceRequestDAL();

        [HttpGet]
        public IEnumerable<ServiceRequest> Get()
        {
            return _dal.GetAllRequests();
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var req = _dal.GetRequestById(id);
            if (req == null) return NotFound();
            return Ok(req);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] ServiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _dal.AddRequest(request);
            return Ok(new { message = "Service request created via API.", request });
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] ServiceRequest request)
        {
            bool updated = _dal.UpdateStatus(id, request.Status);
            if (!updated) return NotFound();
            return Ok(new { message = $"Request #{id} status updated to {request.Status}." });
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool deleted = _dal.DeleteRequest(id);
            if (!deleted) return NotFound();
            return Ok(new { message = $"Request #{id} deleted via API." });
        }
    }
}