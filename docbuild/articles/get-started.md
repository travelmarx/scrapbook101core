---
uid: get-started
title: Scrapbook101core - Get Started
---
# Get Started

To build the {{site_sn}} app on Windows use the following:

* [Visual Studio Community Edition][vsdown]  
* [Azure Cosmos DB Local Emulator][emul]
* [Bing Maps Basic Key][bingmap] (optional)

Install these components on your development or local server. This server will be your local run/test environment as well as the publish point to push changes live, if you choose to do so.

Visual Studio has different workloads that can be installed. If you are running {{site.sn}} on ASP.NET for Windows, then you need the *ASP.NET and web development* workload. If you are running {{site.sn}} on ASP.NET Core, then you need the *.NET Cross-platform development workload*. For more information on workloads, see [Selecting Workloads in Visual Studio 2017][work].

After installing the Cosmos DB Emulator, confirm that you can run Cosmos DB Local Emulator. You should see a screen like the following image. 

![Getting samples in Cosmos DB Local Emulator](../images/where-to-get-samples.jpg "Getting samples in Cosmos DB Local Emulator")

## Run local

Running {{site.sn}} locally, you can try out your ideas at no cost or risk.

<ul class="nav nav-tabs" role="tablist">
  <li class="nav-item">
    <a class="nav-link active" href="#runlocal1" role="tab"
    data-toggle="tab">ASP.NET MVC</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#runlocal2" role="tab"
    data-toggle="tab">ASP.NET Core</a>
  </li>
</ul>

<div class="tab-content">
  <div role="tabpanel" class="tab-pane aspnetmvc active" id="runlocal1">
  <ol><li>Clone (<a href="https://github.com/travelmarx/scrapbook101.git">https://github.com/travelmarx/scrapbook101.git</a>) or download the {{site.sn}} code and open it as a solution in Visual Studio. The source code in the <code>src</code> folder and these docs in the <code>docs</code> folder.</li>
<li>Open the solution in Visual Studio. For example, open the solution file <code>Scrapbook101.sln</code>.
   </li><li>Customize the code as follows in the <code>web.config</code> file.
   <ul><li>Enter the correct value for <strong>authKey</strong>, which you can get from the local emulator home page as show above.</li><li>Enter a Bing Maps Key for <strong>bingMapKey</strong> if you have one; if blank, then geocoding is skipped.</li><li>Set the value for <strong>addTestAssets</strong> to <code>true</code> to write test assets or <code>false</code> not to write them.</li></ul>
   There are other <code>web.config</code> settings you can change, but the the three above are the enough to get started.
   </li></ol>
</div>
  <div role="tabpanel" class="tab-pane aspnetcore" id="runlocal2">
  <ol><li>Clone (<a href="https://github.com/travelmarx/scrapbook101core.git">https://github.com/travelmarx/scrapbook101core.git</a>) or download the {{site.sn}} code and open it as a solution in Visual Studio. The source code in the <code>
Scrapbook101core</code> folder. </li>
  <li>Open the solution in Visual Studio. For example, open the solution file <code>Scrapbook101core.sln</code>.
   </li><li>Customize the code as follows in the <code>appsettings.json</code> file.
   <ul><li>Enter the correct value for <strong>AuthKey</strong>, which you can get from the local emulator home page as show above.</li><li>Enter a Bing Maps Key for <strong>BingMapKey</strong> if you have one; if blank, then geocoding is skipped.</li><li>Set the value for <strong>AddTestAssets</strong> to <code>true</code> to write test assets or <code>false</code> not to write them.</li></ul>
   There are other <code>appsettings.json</code> settings you can change, but the the three above are the enough to get started.
   </li></ol>
  </div>
</div>

With the Cosmos DB Local Emulator running, run the solution (Visual Studio F5) and view {{site.sn}} in a browser, e.g. https://localhost:port#/. You should see something ressembling the following screenshot:

![Scrapbook101Core home page](../images/scrapbook101-running.png "Scrapbook101 Home Page")


What happened on startup:

<ul class="nav nav-tabs" role="tablist">
  <li class="nav-item">
    <a class="nav-link active" href="#explanation1" role="tab"
    data-toggle="tab">ASP.NET MVC</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#explanation2" role="tab"
    data-toggle="tab">ASP.NET Core</a>
  </li>
</ul>

<div class="tab-content">
  <div role="tabpanel" class="tab-pane aspnetmvc active" id="explanation1">
    <ol>
    <li>In the document store, a database named <strong>Scrapbook101</strong> was created with a collection named <strong>Items</strong>.</li>
    <li>A <a href="category-document.md">category document</a> was created and stored in the document store. The category information was read from the file <code>Assets/categories-document.json</code>.</li>
    <li>If the configuration option <strong>addTestAssets</strong> to create test assets was set to `true` in 
    the <code>web.config</code> file, then test items were added to the document store as well. They were read from the file <code>App_data/categories-document.json</code>.</li>
    </ol>
  </div>
  <div role="tabpanel" class="tab-pane aspnetcore" id="explanation2">
    <ol>
    <li>In the document store, a database named <strong>Scrapbook101</strong> was created with a collection named <strong>Items</strong>.</li>
    <li>A <a href="category-document.md">category document</a> was created and stored in the document store. The category information was read from the file <code>Assets/categories-document.json</code>.</li>
    <li>If the configuration option <strong>AddTestAssets</strong> to create test assets was set to `true` in 
    <code>appsettings.json</code>, then test items were added to the document store as well. They were read from the file <code>Assets/categories-document.json</code>.</li>
    </ol>
  </div>
</div>

## Create an item

At this point, you can start working with {{site.sn}} items with CRUD (create, read, update, delete) operations. The home page is https://localhost:port#/ or https://localhost:port#/Item/Index.

To create a new {{site.sn}} item.

1. On the home page, select **Create**.
2. Choose a category in the **Category** dropdown.
3. Fill in the **Title** field.
4. Select **Save** or continue to fill in fields.

## Run live

After running {{site.sn}} locally, you can take optional next step and run {{site.sn}} as a web service. To do this you need to go live, which means publishing your site live with the following services:

- [Azure Cosmos DB Service][cosmos] - This works the same as local emulator and you can copy any documents created locally to the live service. 
 
* [Azure Application Service][azapp] - You can publish your site directly from Visual Studio to the Azure Application service.

Using Azure Cosmos DB Service you will eventually incur charges, but be sure to take advantage of any limited, free use offers. 

There are a couple of other considerations when going live. In the least, you should consider:

* Authentication and authorization.
* Transfering any documents from local emulator to live service.
* How to deal with asset storage.

These and other topics are discussed in [Next Steps][next-steps] and [Handling Assets][handling-assets].

[next-steps]: next-steps.md
[handling-assets]: handling-assets.md
[item]: item-document.md
[cat]: category-document.md
[vsdown]: https://visualstudio.microsoft.com/downloads/
[emul]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
[cosmos]: https://azure.microsoft.com/en-us/services/cosmos-db/
[azapp]: https://azure.microsoft.com/en-us/services/app-service/
[bingmap]: https://www.microsoft.com/en-us/maps/create-a-bing-maps-key
[azblob]: https://azure.microsoft.com/en-us/services/storage/blobs/
[work]: https://visualstudio.microsoft.com/vs/support/selecting-workloads-visual-studio-2017/