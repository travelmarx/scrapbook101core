---
uid: category-document
title: Category Document
---
# Category Document

In the [technology overview][tn] section, we briefly discussed the ideas behind a document-oriented database and how it is used in {{productName}}. In this section we describe one of the two key document types stored in the document-oriented database, the category document.

Our design approach is model-first approach meaning we start with a prototype category document in JSON that represents {{productName}} categories. From this prototype document, a the code class `Category.cs` is defined in our Visual Studio project representing this category document.

## Overview

This {{productName}} category document defines the types of categories that items can belong to (via the **category** field). It is one of the two document types (distinguished by the **type** field) stored in the document-oriented datastore. The other is the [Item Document][item]. The data store will contain many item documents and one category document.

The categories used in {{productName}} are: Book, Event, Film, Museum, People, and Performance. This reduced number of categories was selected to make the code easier to work with and describe. In the version of Scrapbook described in the related [blog post][blog] over 20 categories are used. 

Each category has a **name**, **description**, and one or more associated **categoryFields**. 

```json
{
    "id": "GUID",
    "type": "scrapbook101Categories",
    "categories": [
        {
            "name": "Book",
            "description": "Book",
            "categoryFields": {
                "author": "Author name.",
                "genre": "Genre.",
                "pubYear": "Year published.",
                "synopsis": "Short description.",
                "theme": "Topic or themes."
            }
        },
        {
            "name": "Event",
            "description": "A gathering, party, celebration.",
            "categoryFields": {
                "location": "Joe Smith's house, restaurant name.",
                "people": "A list of people who were there.",
                "type": "[dinner, lunch, party, wedding, festival]"
            }
        },
        {
            "name": "Film",
            "description": "Movie, documentary, series.",
            "categoryFields": {
                "director": "Director name.",
                "genre": "Genre",
                "releaseYear": "Release year.",
                "synopsis": "Short description."
            }
        },
        {
            "name": "Museum",
            "description": "A musuem, site.",
            "categoryFields": {
                "highlight": "Highlight."
            }
        },
        {
            "name": "People",
            "description": "Friend, family, contact, group.",
            "categoryFields": {
                "birthDate": "yyyy-mm-dd",
                "location:": "Where the person lives.",
                "type": "[family, friend, contact, group]"
            }
        },
        {
            "name": "Performance",
            "description": "Theater, concert, play, live performance.",
            "categoryFields": {
                "artist": "Composer, author, or artist.",
                "location": "Venue, theater name.",
                "synopsis": "Brief one sentence overview.",
                "type": "[theater, theatre, concert, play, ballet, opera, dance]"
            }
        }
    ]
}
```

## Description of fields

Fields not marked as <u>Required</u> are not required.

<dl class="deflist">
    <dt>categoryFields</dt>
    <dd>Data fields specific to the category. In this implementation of {{productName}} item, the following keys are supported
    <strong>artist</strong>, <strong>author</strong>, <strong>birthDate</strong>, <strong>director</strong>, 
    <strong>genre</strong>, <strong>highlight</strong>, <strong>location</strong>, 
    <strong>synopsis</strong>, <strong>theme</strong>, <strong>type</strong> <strong>who</strong>, and <strong>year</strong>. The keys are not required. If specified, their values are strings.
    <br/><br/>
    Which keys are used depends on the category chosen for the {{productName}} item. If <i>null</i>, no keys are stored for the item.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A JSON object containing key-value pairs. 
    </p>
    </dd>
    <dt>description</dt>
    <dd>Describes the category. This field is used for information in dropdowns or tooltips.
    <p class="inset">
        <u>Default</u>: <i>null</i>
        <br/>
        <u>Format</u>: A string.
    </p>
    </dd>
    <dt>id</dt>
    <dd>This is an auto-generated GUID representing the item. The value can be auto-generated or assigned. 
    The <strong>id</strong> is used to easily reference the category document.
    <p class="inset">
        <u>Required</u>
        <br/>
        <u>Format</u>: A GUID, e.g., "49916d87-e565-4220-8806-b9e60c867ae6".
    </p>
    </dd>
    <dt>type</dt>
    <dd>Describes the type of record. If your records are stored in a data store with other records with
        an <strong>id</strong> field, then <strong>type</strong> helps distinguish {{productName}} records uniquely.
        There are two types of documents, <a href="/item-document">item documents</a> and category documents.
    <p class="inset">
        <u>Required</u>
        <br/>
        <u>Format</u>: A string equal to "scrapbook101Categories".
    </p>
    </dd>
</dl>

[tn]: technology-overview.md
[item]: item-document.md
[blog]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html