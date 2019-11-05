---
uid: add-web-api
title: Creating a Web API
---
# Adding a Web API (in progress)

Last update: 2019-10-20

What are we trying to do? We build Scrapbook101core as a web interface and then decided we wanted to create a web API to handle HTTP requests. The web API allows for other use cases and ways to interact with Scrapbook101core beyond the browser.

## Initial steps

As usual, we started by following the [Web API Tutorial][webapitut]. A lot of effort goes into creating this type of tutorial experience, so why not use it? And it's our old friend, the Todo List.

After running the tutorial, we realized our first decision point was whether to use the same controller classes for both views (web interface) and web APIs. We decided that we wanted to separate the functionality at the risk of repeating some code.

Install [Postman][postman], a collaboration platform for API development that includes an easy what to construct and send HTTP requests.

## Create the controller 

Our first step is to create a new ItemApiController. We decided to keep the controller similar to existing [ItemController](xref:Scrapbook101core.Controllers.ItemController) but with "Api" added. We added the scaffolding item using Visual Studio.

1. Add a new controller to the project.

1. Select API Controller with read/write actions. (We are not using the Entity Framework.) In the course of experimenting with adding API controllers, we ended up pulling in Microsoft.EntityFrameworkCore.SqlServer.Design and Microsoft.EntityFrameworkCore.Tools in the .csproj file

1. A new controller is generated. Note we are inheriting from ControllerBase. Here's the skeleton of the class:

We already have an [Item](xref:Scrapbook101core.Models.Item) class defined, so no changes necessary there. And there were no initial changes needed in appsettings.json, Program.cs or Startup.cs.

The controller after running the scaffolding looks like this:

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

## GET api
The next step was to get the GET action to work. This step seemed easy but was a little tricky. We needed to read up on [Action return types][actionresult] and get some help on an implicit conversion error below [here][converterr] and [here][git8061]. The error was this::

*Cannot implicitly convert type 'System.Collections.Generic.IEnumerable<Scrapbook101core.Models.Item>' to 'Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.IEnumerable<Scrapbook101core.Models.Item>>'	Scrapbook101core*

The solution was to simply use `.ToList` so that our simple GET method then became this:

```c#
// GET: api/ItemApi
[HttpGet]
public async Task<ActionResult<IEnumerable<Item>>> GetAsync()
{
    var items = await DocumentDBRepository<Item>
        .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
    var imagePath = HelperClasses.BuildPathList(items);
    return items.ToList();
}
```

Here's an example of using Postman to test the GET to return all items.

![Using Postman to GET all items.](../images/using-postman-to-get.jpg "Using Postman to GET all items.")

## Code the rest

Method | URI | Notes
--- | --- | ---
GET | /api/ItemApi | Returns all items.
GET | /api/ItemApi/GUID | Returns the details for the specified item matching the GUID.
DELET
E | /api/ItemApi/GUID | Deletes the item matching the GUID.


[webapitut]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
[postman]: https://www.getpostman.com/downloads/
[actionresult]: https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-2.1#actionresultt-type
[converterr]: https://stackoverflow.com/questions/50383193/cannot-implicitly-convert-type-to-actionresultt?noredirect=1&lq=1
[git8061]: https://github.com/aspnet/Mvc/issues/8061

