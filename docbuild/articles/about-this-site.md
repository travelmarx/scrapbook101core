---
uid: about
title: Scrapbook101core - About this site
---
# About This Site

## Overview

This site was created with [DocFx][docfx]. How is this site different from our [GitHub pages site][site1] for Scrapbook101? 

* This site parses comments from code (under **\Scrapbook101core**) and creates API documentation together with conceptual content (under **\articles**). The conceptual documentation from the GitHub pages site was reused here with few changes. 

* In the previous [site][site1], we used Jekyll to create the website, which contained only conceptual content. We used Jekyll to run local builds and then pushed the .md files to GitHub into a **\docs** folder, which we defined as a GitHub pages website. There, the Jekyll process would kick off automatically and create the HTML for the [site][site1].

* This site relies only on DocFx. We still work offline (currently), run DocFx to produce the website with both conceptual and API documentation, and then push to Github **\docs** folder. The difference with our Jekyll site is that here we are pushing the HTML files and are not relying on Jekyll to build HTML for us.


## Initial steps

We started by running `docfx init` to create a simple documentation website. We played around with that for a bit, following the [walkthroughs][walk].

After that initial simple documentation website, we started reading up on using [C# XML documentation][ref3]. 

Then, we merged the simple documentation website structure with Scrapbook101core. Specifically, we created a **\docs** and **\docbuild** folder parallel to code folder. In the **\docbuild** folder, we run `docfx` to gather metadata and build HTML. Then as a final step, we copy the **\doc\build\ _site content** over to **\docs**.

Our local process:

1. Start in root folder.
    * In this case, it is the directory we cloned from GitHub, "scrapbook101core".
1. Change directory to **\docbuild**.
    * There should be a docfx.json file in the root of this folder. We copied the filed from the walkthrough.
    * If you didn't start with a docfx walkthrough, you can use the `docfx init` subcommand to generate an initial docfx.json file.
1. Run `docfx metadata`.
    * This generates YAML files from source code.
1. Run `docfx build`.
    * This generates client-only website combining API in YAML files and conceptual files.
1. Run `docfx --serve`
    * This command executes the subcommands `metadata` and `build` and then creates a local web server to host the pages. So the previous two steps can be skipped. They were shown just to talk about what's happening behind the scenes.
    * To use a different port try `docfx --serve --port=8090`.
    * Hosting the pages is not strictly necessary. If you ran `docfx metadata` and `docfx build`, then at this point you can navigate to the **_site** folder and just open the index.html file without creating a local web server to host.
1. Copy all content from **\docbuild\ _site** to **\docs**.
    * The copy operation is because DocFx is not integrated with GitHub Pages ([issue][issue3284]).
    * The downside of this is that we end up pushing a lot of files (HTML and supporting files).
1. Commit, push changes to GitHub.
    * Don't forget to check you .gitignore file and make sure **_site** is excluded because we'll be publishing the docs in the **\docs** folder.


The directory structure at the root of Scrapbook101Core looks like this:

```
├───docbuild
│   ├───api
│   ├───articles
│   ├───images
│   ├───obj
│   └───_site
│       ├───api
│       ├───articles
│       ├───fonts
│       └───styles
├───docs
│   ├───api
│   ├───articles
│   ├───fonts
│   ├───images
│   └───styles
└───Scrapbook101core
    ├───Assets
    ├───bin
    ├───Controllers
    ├───Models
    ├───obj
    ├───Properties
    ├───Views
    └───wwwroot
```

### Gotchas

Some things we ran into:

* When testing the [walkthroughs][walk], we used the suggested folder naming **\docfx_walkthrough\docfx_project**. Then, in the context of the walkthrough,
we pointed to Scrapbook101core code just to see what would happen and it built fine. However, when we then created the **\docbuild** folder inside of Scrapbook101core and tried to run `docfx metadata`, it was still looking for the **\docfx_project**. The culprit: **\Scrapbook101core\obj\xdoc\cache**. We deleted the cache folder and it started working.

* In the process of ensuring we can always build everything from scratch, we often wipe out the .yml files in the **\api** folder. But be careful not to wipe out the index.md file if that is what you are pointing to in root toc.yml.

* Pay attention to warnings in docfx output, they are useful.

* Using the Jekyll process, we could define global site variables like {{site.sn}}, where site.sn = "Scrapbook101". With the docfx process, we haven't figured out how to do that.

## Next steps

### Learn about templates

Starting with the [advanced walkthrough][awalk] as guidance, we did the following as a test:

1. See what templates are available with `docfx template list`. We are interested in the default template.

1. Export the default template with `docfx export default`. 
    * This will created a folder called **_exported_templates**.  Either add this to .gitignore or delete after done.

1. In our **\docbuild** folder create a **templates\cust-template** folder.

1. Decide to change the footer template of the site to add the name of our site. So in **_exported_templates\default\partials** copy the footer.tmpl.partial file to **docbuild\templates\cust-template\partials**.

1. Edit the footer.tmpl.partial file to add "Scrapbook101core".

1. In the **build** key of docfx.json reference the custom template.
   
   ```
    "template": [
        "default",
        "templates/cust-template"
    ],
    ```

1. Build site and verify changes.

### Learn about links and cross references via UID. 

For example, here is a link to <xref:code-discussion> file using its UID. Here is a link to a class in the API documentation using its UID: <xref:Scrapbook101core.Models.Item>. Or we can change the text for the API link as so [The Item Class](xref:Scrapbook101core.Models.Item). Here's the [help page][linkhelp] on linking.

Here are those links in markdown:

```
<xref:code-discussion>
<xref:Scrapbook101core.Models.Item>
[The Item Class](xref:Scrapbook101core.Models.Item)
```

## Future

Create a Azure pipeline process to build docs automatically. The flow would be then:

1. Author comments in code.
2. Check in code changes.
3. Kick off or set up pipeline to automatically build upon changes to code. TBD.


## References

* [DocFx: Metadata Format for .NET Language][ref2]

* [DocFX: Triple Slash Comments Support][ref3]

* [XML Documentation Comments (C#)][ref4]

* [Documenting your code with XML comments][ref1]

* [Customizing template](https://stackoverflow.com/questions/56458435/docfx-how-to-suppress-certain-info-about-type-inheritance-constructors-assem)



[docfx]: https://dotnet.github.io/docfx/
[walk]: https://dotnet.github.io/docfx/tutorial/walkthrough/walkthrough_overview.html
[awalk]: https://dotnet.github.io/docfx/tutorial/walkthrough/advanced_walkthrough.html#create-a-new-template
[ref1]: https://docs.microsoft.com/dotnet/csharp/codedoc
[ref2]: https://dotnet.github.io/docfx/spec/metadata_dotnet_spec.html
[ref3]: https://dotnet.github.io/docfx/spec/triple_slash_comments_spec.html
[ref4]: https://docs.microsoft.com/dotnet/csharp/programming-guide/xmldoc/
[site1]: https://travelmarx.github.io/scrapbook101/
[issue3284]: https://github.com/dotnet/docfx/issues/3284
[linkhelp]: https://dotnet.github.io/docfx/tutorial/links_and_cross_references.html