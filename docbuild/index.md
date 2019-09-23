---
uid: index
title: Homepage
---

This is the technical documentation set for Scrapbook101core. Go to [Introduction][intro] article to get started.

In case you are wondering how this site different from our [GitHub pages site][site1] for Scrapbook101:

* This site was created with [DocFx][docfx] and not the Jekyll process.

* This site included parsing comments from code (under **\Scrapbook101core**)and creating API documentation  together with conceptual content (under **\articles**). The conceptual documentation from the GitHub pages site was reused here with few changes. 

* In the previous [site][site1], we used Jekyll to create the website (and only conceptual content. We used Jekyll to run local builds and then pushed the .md files to GitHub into a **\docs** folder, which we defined as a GitHub pages website. There, the Jekyll process would kick off automatically and create the [site][site1] live.

* This site relies only DocFx. We still work offline (currently), run DocFx to produce the website with both conceptual and API documentation, and then push to Github **\docs** folder. The difference is that we are pushing the HTML files and are not relying on Jekyll to build HTML for us.

For more information about how this site was put together, see the [about][about] page.


[site1]: https://travelmarx.github.io/scrapbook101/
[about]: articles/about-this-site.md
[intro]: articles/index.md
[docfx]: https://dotnet.github.io/docfx/
