---
uid: item-document
title: Scrapbook101core - Item Document
---
# Item Document

In the [technology overview][tn] section, we briefly discussed the ideas behind a document-oriented database and how it is used in {{site.sn}}. In this section we describe one of the key document types stored in the document-oriented database, the item document.

Our design approach is model-first approach meaning we start with a prototype item document in JSON that represents one {{site.sn}} item. From this prototype document, the code class `Item.cs` is defined in our Visual Studio project representing this item document.


## Overview

Below is the prototype item document that describes one {{site.sn}} item in JSON. The document data store will contain one document that looks like this for every {{site.sn}} item. This item document is one of the two document types (distinguished by the `type` field) stored in the document datastore. The other document type is the [Category Document][cat]. 

The values of the fields in the key-value pairs aren't important below, only the structure is. Following the JSON are descriptions of each field.

```json
{
    "assets": [
        {
            "name": "name",
            "size": 10000
        }
    ],
    "assetPath": "path to assets",
    "category": "category",
    "categoryFields": {
    },
    "dateAdded": "timestamp",
    "dateUpdated": "timestamp",
    "description": "description",
    "geoLocation": {
        "type": "Point",
        "coordinates": [
            43.5290298461914, 
            12.1621837615967
        ]
    },
    "id": "GUID",
    "location": "location name",
    "rating": "rating",
    "title": "title",
    "type": "scrapbook101Item",
    "updatedBy": "identifier, email or name"
}
```

## Description of fields

Fields not marked as <u>Required</u> are not required.

<dl class="deflist">
    <dt>assets</dt>
    <dd>A list of assets associated with the {{site.sn}} item. Can be <i>null</i>, meaning no assets are associated with the item.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A JSON array of objects containing two strings and a number:
    </p>
    <dl class="subdeflist">
        <dt>name</dt>
        <dd>The name of the asset.
        <p>
            <strong>Format</strong>: A string, e.g., "Folder.jpg".
        </p>
        </dd>
        <dt>size</dt>
        <dd>The size in bytes of the asset.
        <p>
            <strong>Format</strong>: A number, e.g., 20056.
        </p>
        </dd>
    </dl>
    </dd>
    <dt>assetPath</dt>
    <dd>This is the path to any digital assets associated with the item. For example, there might be images, PDFs or other documents that give context about the item. Associating assets is not necessary if you plan to implement {{site.sn}} without any assets. Part of the spirit behind {{site.sn}} however is to provide strong visual cues for remembering an item as well as archiving data associated with it, so at least one image for each {{site.sn}} is recommended.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A string, e.g., "year2018/jan-10" or "assets/2018/2018-12-10".
    </p>
    </dd>
    <dt>category</dt>
    <dd>One of the category values described in <a href="/category-document">Category Document</a>. A {{site.sn}} item must
    be assigned to a category.
    <p class="inset">
        <u>Required</u>
        <br/>
        <u>Format</u>: A string, e.g. "Books".
    </p>
    </dd>
    <dt>categoryFields</dt>
    <dd>Fields specific to the category selected for the {{site.sn}} item. See the <a href="/category-document">Category Document</a> for more information. Each
    category has a fixed set of fields.
    <p class="inset">
        <u>Default</u>: Depends on the category chosen.
        <br/>
        <u>Format</u>: An object containing key-value pairs. 
    </p>
    <p>For example, for a {{site.sn}} book item, the <strong>categoryField</strong> object would look like the following:</p>
    <pre>
"categoryFields": {
    "author": "author name",
    "genre": "genre",
    "year": "year",
    "synopsis": "synopsis",
    "theme": "theme"
}</pre>
    </dd>
    <dt>dateAdded</dt>
    <dd>This value is automatically inserted by code upon initial creation of an item and then never changed.
    <p class="inset">
        <u>Format</u>: A string that is an ISO 8601 date and time, e.g., "2018-06-07T00:00:00".
    </p>
    </dd>
    <dt>dateUpdated</dt>
    <dd>This value is automatically inserted by code. On initial creation of an {{site.sn}} item, <strong>dateUpdated</strong> equals <strong>dateAdded</strong>. When an item is edited, <strong>dateUpdated</strong> is updated with the correct timestamp.
    <p class="inset">
        <u>Default</u>: Initially equal to <strong>dateAdded</strong>.
        <br/>
        <u>Format</u>: A string that is an ISO 8601 date and time, e.g., "2018-06-07T00:00:00".
    </p>
    </dd>
    <dt>description</dt>
    <dd>Information about the {{site.sn}} item that isn't already captured in other fields. This field provides the context of why the item is important. Descriptions longer than a couple hundred words should be added as assets.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A string, e.g. "The best book I've read in 2018...".
    </p>
    </dd>
    <dt>geoLocation</dt>
    <dd>If you have a <a href="https://www.microsoft.com/en-us/maps/create-a-bing-maps-key">Bing Maps Key</a> and it is specified in the <code>web.config</code> file, then <strong>location</strong> values are converted into latitude and longitude coordinates (geocoded) to allow for more flexible location searches.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: An object that contains a string and an array:
        <dl class="subdeflist">
            <dt>type</dt>
            <dd>The type of geocode. {{site.sn}} stores points, so this value is always "Point".</dd>
            <dt>coordinates</dt>
            <dd>An array of two numbers, the first is longitude and the second is latitude. For exmple,
            [43.5290298461914, 12.1621837615967].</dd>
        </dl>
    </p>
    </dd>
    <dt>id</dt>
    <dd>This is a GUID auto-generated upon insertion into the document store.
    <p class="inset">
        <u>Required</u>
        <br/>
        <u>Format</u>: A GUID, e.g., "d26f6d16-591e-4ae2-9072-d7ba113ec454".
    </p>
    </dd>
    <dt>location</dt>
    <dd>This field is a friendly name of the location that is relevant for the {{site.sn}} item. It is not required but is helpful. It is important to be consistent with how you enter values to make searching easier.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A string, e.g. "Seattle, WA" or "101 Main St., Seattle 98103, USA".
    </p>
    </dd>
    <dt>rating</dt>
    <dd>A rating of the {{site.sn}} item, if applicable. The rating system will be unique for each implementation. The test data provided with {{site.sn}} uses a rating of 1 - 5.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A number, e.g. 3.
    </p>
    </dd>
    <dt>title</dt>
    <dd>A descriptive title for the {{site.sn}} item. For example, if item is a book, use all or part of the book title.
    <p class="inset">
        <u>Required</u>
        <br/>
        <u>Format</u>: A string, e.g. "Brief Answers to the Big Questions".
    </p>
    </dd>
    <dt>type</dt>
    <dd>Describes the type of record. If your documents are stored in a data store with other documents from other applications using an <strong>id</strong> field, then <strong>type</strong> helps distinguish {{site.sn}} records uniquely. There are two types of documents, item documents and <a href="/category-document">category documents</a>.
    <p class="inset">
        <u>Required</u>
        <br/>
        <u>Format</u>: A string equal to "scrapbook101Item".
    </p>
    </dd>
    <dt>updatedBy</dt>
    <dd>This could be a user name or ID. For example, if you implement a authentication scheme based on Windows Live, Facebook, or Google, you can use
    the user's email or ID in this field.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A string, e.g. "somename@someemail.com".
    </p>
    </dd>
</dl>

## JSON schema

*Optional.*

The description of the fields above is usually sufficient for programmers looking for an overview of the item document's structure, its fields and their values. Another way to approach "documenting" the {{site.sn}} item document is with a [JSON schema][json-schema], which would provide a more precise description of the JSON. Besides describing the JSON item document, the schema could also be used to validate any new block JSON as representing a valid item document or not. 

In the {{site.sn}} application, validation is done implicitly as the JSON is serialized from a class object and deserialized into a class object. In other words, documents in our data store are valid JSON because they are created and edited via a fixed class (see for example the `Item.cs` class). In this situation, a JSON schema isn't as critical because we won't be encountering JSON that isn't in the context of this application. 

Consider the following simplified item document:

```json
{
    "category": "category",
    "description": "description",
    "id": "GUID",
    "rating": "rating",
    "title": "book title",
    "type": "scrapbook101Item"
}
```

Using a schema generator like [JSONschema.net][json-schema-gen], a possible schema describing the simplified item document might look like the following (truncated to show just the initial part of the schema):

```json
{
   "definitions": {},
   "$schema": "http://json-schema.org/draft-07/schema#",
   "$id": "http://example.com/root.json",
   "type": "object",
   "title": "The Root Schema",
   "required": [
     "item"
   ],
   "properties": {
     "item": {
       "$id": "#/properties/item",
       "type": "object",
       "title": "The Item Schema",
       "required": [
         "category",
         "id",
         "title",
         "type"
       ],
       "properties": {
         "category": {
           "$id": "#/properties/item/properties/category",
           "type": "string",
           "title": "The Category Schema",
           "default": "",
           "examples": [
             "category"
           ],
           "pattern": "^(.*)$"
         },
...
}
```



[cat]: category-document.md
[tn]: technology-overview.md
[docdb]: https://en.wikipedia.org/wiki/Document-oriented_database
[json-schema]: https://json-schema.org/
[json-schema-gen]: https://www.jsonschema.net/