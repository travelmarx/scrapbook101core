---
title: Code Discussion
uid: code-discussion
---
# Code Discussion

At this point, if you followed the steps in [Get Started][gs], you might have some more questions about how the code works. In this topic, we discuss the rational behind how categories are defined in {{site.sn}} and how C# objects are used to model both category and items.

## Categories

{{site.sn}} items are described by a name/value pairs such as **title**, **id**, and **category**. There are also objects that contain child name/value pairs. For example, **assets** is an object that contains name/value pairs with the names **name** and **size**. See [item document][item] for more information.

Category values (**category** value) can be one of a fixed number of string values ("Books", "Events", etc.). Each category has a fixed number of associated fields as described in [category document][cat]. The category fields (**categoryField**) object contains different name/value pairs that depend on the selected category.

The advantages of a nested structure (top level name/value pairs and nested name/value pairs defined in an object) is that it is easier to read and allows for more targeted searching because of the controlled category field taxonomy. The disadvantage of the nested structure is that it introduces more complexity in code because additional objects such as the `Category.cs` object are needed to track the nested structure. 

An obvious question is if there another way to handle categories?  Can the JSON structure be flattened? Consider an implementation of {{site.sn}} where the category document is not used, and **category** and **categoryFields** are not controlled by a schema but are allowed to be added or not. In this case, **category** values might be "Book", "Books", "BooksRead", etc., that is, values are not governed by a fixed set of choices. This is certainly easier implementation-wise with less complicated class structures in code, but it becomes harder to have reliable search results. 

Another consideration in choosing a JSON structure - choosing a schema - is determining which name/value pairs (fields) belong at the item-level (first-level) or as category fields (nested, second-level). Here are some rough guidelines for determining what fileds belong at the category level and which belong at the item level.

* Category fields are specific to one or more categories but are not general enough to be be useful for all categories and therefore should *not* be promoted to [item level][item]. Use the same field names when possible between category data fields. For example, **synopsis**, **type**, **location**, and **year** are used in several categories. This is useful because the object representation of the categories in `Categories.cs` can be more compact and requires less property definitions.

* Item data fields are common to all (or nearly all) categories. For example, the **rating** field applies to most categories in {{site.sn}} like "Books", "Films", "Museums", and "Performances" easily and to "People" less so, it still is better suited at the item-level than duplicated at the category-level. It is not a problem to leave the **rating** field blank if it doesn't apply.

## Models

There are three model files: 

* `Item.cs` is the object representing the [item document][item].

* `Category.cs` is the object representing the [category document][cat].

* `CombinedModel.cs` is needed because each MVC view (`.cshtml` page) needs to bind to one model, however in some views both the item and category models are needed. Therefore, the combined model represents a combination of the two models plus some helper classes for dealing with how categories are displayed and how assets are tracked.

In the `Item.cs` and `Category.cs` classes, note the use of the [Newtonsoft Json.NET][newton] JsonProperty annotations that allow the use of lowercase names for data fields in JSON (e.g., "assetPath") and camelcase for data fields in code (e.g., "AssetPath").

Finally, recall how our {{site.sn}} categories can contain name/value pairs (fields) with the same name. For example, a book and film item both have a **synopsis**  field. Instead of defining multiple properties in code that have the same name "Synopsis", we instead define one property that can be used for any category that includes a **synopsis** field.  In `Category.cs` we define a `CategoryFields` class with all possible category fields, and allow indexing by name (`this`) so that just the fields for a given category are included with a {{site.sn}} are used.

```C#
public class Category
{
    [JsonProperty(PropertyName = "categories")]
    public List<CategoryItem> Categories { get; set; }
}
public class CategoryItem
{
    [JsonProperty(PropertyName = "categoryFields")]
    public CategoryFields CategoryFields { get; set; }
}
public class CategoryFields
{
    [JsonIgnore]
    public object this[string propertyName]
    {
        get { return this.GetType().GetProperty(propertyName).GetValue(this); }
        set { this.GetType().GetProperty(propertyName).SetValue(this, value); }
    }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "synopsis")]
    public string Synopsis { get; set; }
}
```



[gs]: get-started.md
[item]: item-document.md
[cat]: category-document.md
[blog]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html
[newton]: https://www.newtonsoft.com/json
