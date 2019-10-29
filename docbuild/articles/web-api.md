---
uid: web-api
title: Web API
---
# Web API (in progress)

Last update: 2019-10-20

What are we trying to do? We build Scrapbook101core as a web interface and then decided we wanted to create a web API to handle HTTP requests. The web API allows for other use cases and ways to interact with Scrapbook101core beyond the browser.

## First steps

As usual, we started by following the [Web API Tutorial][webpaitut]. A lot of effort is put into creating this tutorial experiences so why not use them? And it's our old friend, the Todo List.

After running the tutorial, we realized our first decision point was whether to use the same controller classes for both views (web interface) and web APIs. We decided that we wanted to separate the functionality at the risk of repeating some code.

Install [Postman][postman], a collaboration platform for API development that includes an easy what to construct and send HTTP requests.

## Detailed steps

1. Create a new ItemApiController. Keep the names similar to existing [ItemController](xref:Scrapbook101core.Controllers.ItemController) but with "Api" added.

2. We already have an [Item](xref:Scrapbook101core.Models.Item) class defined, so no changes necessary there. 

3. TBD: Any changes in appsettings.json?

4. TBD: Any changes in Program.cs or Startup.cs?

Adding scaffolding item. Easier in Visual Studio? First time through, might be easier to use Visual Studio.

1. Add new controller.

2. Select API Controller with read/write actions. We are not using the Entity Framework.

3. A new controller is generated. Note we are inheriting from ControllerBase.

```c#
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Scrapbook101core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemApiController : ControllerBase
    {
        // GET: api/ItemApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET: api/ItemApi/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        // POST: api/ItemApi
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        // PUT: api/ItemApi/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
```

[webapitut]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
[postman]: https://www.getpostman.com/downloads/
