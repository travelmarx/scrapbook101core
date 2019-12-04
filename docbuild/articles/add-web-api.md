---
uid: add-web-api
title: Creating a Web API
---
# Add a Web API

Last update: 2019-12-04

What are we trying to do by adding a Web API? We built Scrapbook101core as a web interface and then decided we wanted to create a web API to handle HTTP requests. The web API allows for other ways to interact with Scrapbook101core beyond the browser. This page describes some of the steps we went through to add a Web API to an existing ASP.NET core web site.

## Initial steps

As usual, we started by following a tutorial: [Web API Tutorial][webapitut]. A lot of effort goes into creating this type of tutorial experience, so why not use it? And it's our old friend, the Todo List.

After running the tutorial, we realized our first decision point was whether to use the same controller classes for both views (web interface) and web APIs. We decided that we wanted to separate the functionality at the risk of repeating some code.

Another initial step was to install [Postman][postman], a collaboration platform for API development that includes an easy what to construct and send HTTP requests.

## Create the controller 

Our next step is to create a new controller called [ItemApiController](xref:Scrapbook101core.Controllers.ItemApiController). We decided to keep the controller name similar to existing [ItemController](xref:Scrapbook101core.Controllers.ItemController) but with "Api" added. We added the controller using scaffolding in Visual Studio, i.e., just added it following the menus:

1. Select the **Controllers** folder, right click and select **Add** > **Controller**.

1. Select API Controller with read/write actions. (We are not using the Entity Framework.) In the course of experimenting with adding API controllers, we ended up pulling in Microsoft.EntityFrameworkCore.SqlServer.Design and Microsoft.EntityFrameworkCore.Tools in the .csproj file

1. A new controller .cs file is generated. Note we are inheriting from ControllerBase. Here's the skeleton of the class:

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

## GET method

After creating the skeleton controller, we decided to get one method working and the simplest to get working is the GET method that returns items. This task seems easy but was a little tricky. We needed to read up on [Action return types][actionresult] and get some help on an implicit conversion error below [here][converterr] and [here][git8061]. The error was this::

*Cannot implicitly convert type 'System.Collections.Generic.IEnumerable<Scrapbook101core.Models.Item>' to 'Microsoft.AspNetCore.Mvc.ActionResult<System.Collections.Generic.IEnumerable<Scrapbook101core.Models.Item>>'	Scrapbook101core*

The solution was to simply use `.ToList` on the items returned. 

Below, we show three versions of the GET action. We started with "GET basic", then moved to the "GET with no search", and finally arrived at "GET with search".

# [GET with search](#tab/tabid-1)

```c#
/// <summary>
/// Returns Scrapbook101 items whose title or body matches filter. If filter is omitted, all items are returned.
/// </summary>
/// <remarks>
/// Specify the HTTP GET method and the URI "baseURI/api/ItemApi?filter=filter".
/// </remarks>
/// <param name="filter">Optional string to find in titles and descriptions.</param>
/// <returns>JSON representing of all matching items.</returns>
[HttpGet]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<IEnumerable<Item>>> GetAsync([FromQuery] string filter)
{
    try
    {
        if (String.IsNullOrEmpty(filter))
        {
            var items = await DocumentDBRepository<Item>
                .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
            var imagePath = HelperClasses.BuildPathList(items);
            return Ok(items.ToList());
        }
        else
        {
            var items = await DocumentDBRepository<Item>
                .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType
                    && (item.Title.Contains(filter.ToLower()) 
                    || item.Description.Contains(filter.ToLower())));
            var imagePath = HelperClasses.BuildPathList(items);
            return Ok(items.ToList());
        }
    }
    catch
    {
        return NotFound();
    }
}
```

# [GET with no search](#tab/tabid-2)

```c#
/// <summary>
/// Returns all the items in Scrapbook101.
/// </summary>
/// <remarks>
/// Specify the HTTP GET method and the URI "baseURI/api/ItemApi".
/// </remarks>
/// <returns>JSON representing of all items.</returns>
[HttpGet]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<IEnumerable<Item>>> GetAsync()
{
    try
    {
        var items = await DocumentDBRepository<Item>
            .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
        var imagePath = HelperClasses.BuildPathList(items);
        return Ok(items.ToList());
    }
    catch
    {
        return NotFound();
    }
}
```

# [GET basic](#tab/tabid-3)

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

***
<br/>

Note that in all versions of the GET action, we made the method asynchronous, and that we are using [ActionResult][actionresult] instead of IActionResult. The action's return type is inferred from the `T` in the `ActionResult<T>`.

In the version of the GET action with search, we added `ProducesResponseType` attribute for "200 OK" and "404 Not Found" responses as well as their corresponding methods `Ok()` and `NotFound()`.

Here's what we are not handling in the GET action:

* Paging. We should return a fixed number of items with a paging token to use for a subsequent request. Instead, we currently return everything.

* Security. If this were a protected Web API, we'd be looking for an authorization token in the request.

* Base URL. There is no indication of which base URL to use to access assets. We could have another API call/method whose job it is to return just that base URL or it could be returned as part of the results of the GET method.

Here's an example of using Postman to test the GET to return all items.

![Using Postman to GET all items.](../images/using-postman-to-get.jpg "Using Postman to GET all items.")


## POST method

See the <xref:Scrapbook101core.Controllers.ItemApiController.PostAsync(Scrapbook101core.Models.Item)> method for the code handling the POST method. The minimal JSON body required for POST is:

```json
{
    "category": "Museum",
    "title": "Test Museum",
    "type": "scrapbook101Item"
}
```

The <xref:Scrapbook101core.Models.Item> class defines the required attributes. If any of those attributes are not specified, then a 400 response is returned. For example, the error for not specifying the type filed is:

```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "|575802c9-4a00edbb2ae13494.",
    "errors": {
        "Type": [
            "Type is required."
        ]
    }
}
```

Note that the POST method creates an object and returns the GUID in the response.

The POST method, like the GET method, has some key functionality left out:

* Security. If this were a protected Web API, we'd be looking for an authorization token in the request.

* Assets. There is no provision for uploading or linking to a new assets. It is assumed the asset already exists in the base assets folder location.

## PUT method

See the <xref:Scrapbook101core.Controllers.ItemApiController.PutAsync(System.String,Scrapbook101core.Models.Item)> method for the code handling the PUT method. The minimal JSON body required for PUT is:

```json
{
    "category": "Museum",
    "title": "Test Museum",
    "type": "scrapbook101Item"
}
```

Here's an error example of specifying wrong field type for the rating field.

```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "|18e03c00-4d95bfbf6dc4cf23.",
    "errors": {
        "$.rating": [
            "The JSON value could not be converted to System.String. Path: $.rating | LineNumber: 4 | BytePositionInLine: 15."
        ]
    }
}
```

For item properties not specified in the JSON body of the request, the original item values are used.

## All methods

All of these methods are defined in <xref:Scrapbook101core.Controllers.ItemApiController>.

Method | Code | URI | Notes
--- | --- | --- | ---
GET | <xref:Scrapbook101core.Controllers.ItemApiController.GetAsync(System.String)> | /api/ItemApi?filter=filter | Returns items where filter whose title or description matches "filter". If filter isn't specified return maximum number of items.
GET | <xref:Scrapbook101core.Controllers.ItemApiController.DetailsAsync(System.String)> | /api/ItemApi/GUID | Returns the details for the specified item matching GUID.
DELETE | <xref:Scrapbook101core.Controllers.ItemApiController.DeleteAsync(System.String)> | /api/ItemApi/GUID | Deletes the item matching GUID.
POST | <xref:Scrapbook101core.Controllers.ItemApiController.CreateAsync(Scrapbook101core.Models.Item)> | /api/ItemApi | Creates a new item with the properties of the item in the request body as JSON.
PUT | <xref:Scrapbook101core.Controllers.ItemApiController.UpdateAsync(System.String,Scrapbook101core.Models.Item)> | /api/ItemApi/GUID | Updates an existing item with the properties of the item in the request body as JSON.

For more information about HTTP methods, see the [REST API Tutorial][resttut].


## Set up Swagger

[Swagger][swagger] generates useful documentation and help pages for Web APIs. It provides benefits such as interactive documentation, client SDK generation, and API discoverability.

Following, the [instructions][swashbuckle] for installing Swashbuckle.AspNetCore, open source project for generating Swagger documents for ASP.NET Core Web APIs.

[webapitut]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
[postman]: https://www.getpostman.com/downloads/
[actionresult]: https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-2.1#actionresultt-type
[converterr]: https://stackoverflow.com/questions/50383193/cannot-implicitly-convert-type-to-actionresultt?noredirect=1&lq=1
[git8061]: https://github.com/aspnet/Mvc/issues/8061
[resttut]: https://restfulapi.net/http-methods/
[swagger]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-3.0
[shashbuckle]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.0&tabs=visual-studio