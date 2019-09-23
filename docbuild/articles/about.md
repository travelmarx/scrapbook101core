---
uid: about
title: About this site
---
# About this site

## Overview

This site was created with [DocFx][docfx].

How is this site different from our [GitHub pages site][site1] for Scrapbook101? 

* This site included parsing comments from code (under **\Scrapbook101core**)and creating API documentation  together with conceptual content (under **\articles**). The conceptual documentation from the GitHub pages site was reused here with few changes. 

* In the previous [site][site1`, we used Jekyll to create the website (and only conceptual content. We used Jekyll to run local builds and then pushed the .md files to GitHub into a **\docs** folder, which we defined as a GitHub pages website. There, the Jekyll process would kick off automatically and create the [site][site1] live.

* This site relies only DocFx. We still work offline (currently), run DocFx to produce the website with both conceptual and API documentation, and then push to Github **\docs** folder. The difference is that we are pushing the HTML files and are not relying on Jekyll to build HTML for us.

## Initial steps

We started by running `docfx init` to create a simple documentation website. We played around with that for a bit, following the [walkthroughs][walk].

After that initial simple documentation website, we started reading up on using [C# XML documentation][ref3]. 

Then, we merged the simple documentation website structure with ScrapbookCore101. Specifically, we created a **\docs** and **\docbuild** folder parallel to code folder. In the **\docbuild** folder, we run `docfx` to gather metadata and build HTML. Then as a final step, we copy the **\doc\build\ _site content** over to **\docs**.

Local process:

1. Start in root folder as shown below.
1. Change directory to **\docbuild**.
1. Run `docfx metadata`
1. Run `docfx --serve --port=8090`
1. Copy all content from **\docbuild\ _site** to **\docs**.
1. Commit, push to GitHub.

If port 8080 is not in use, you can leave off the port specification. The copy operation is because DocFx is not integrated with GitHub Pages ([issue][issue3284]).

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
[ref1]: https://docs.microsoft.com/dotnet/csharp/codedoc
[ref2]: https://dotnet.github.io/docfx/spec/metadata_dotnet_spec.html
[ref3]: https://dotnet.github.io/docfx/spec/triple_slash_comments_spec.html
[ref4]: https://docs.microsoft.com/dotnet/csharp/programming-guide/xmldoc/
[site1]: https://travelmarx.github.io/scrapbook101/
[issue3284]: https://github.com/dotnet/docfx/issues/3284