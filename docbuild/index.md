---
uid: index
title: Scrapbook101core
---

This is the technical documentation set for Scrapbook101core. Go to [Introduction][intro] article to get started.

In case you are wondering how this documenation site is different from our [GitHub pages site][site1] for Scrapbook101:

* This site was created with [DocFx][docfx] and not the Jekyll process.

* As a docfx-generated site it includes parsing comments from code (under **\Scrapbook101core**)and creating API documentation together with conceptual content (under **\articles**). The conceptual documentation from the GitHub pages site was reused here with few changes. 

* In the previous [site][site1], we used Jekyll to create the website, which contained only conceptual content. We used Jekyll to run local builds to speed up the authoring process and find problems before going live. After building locally, we then pushed the .md files to GitHub into a **\docs** folder, which we defined as a GitHub pages website. There, the Jekyll process would kick off automatically and create the HTML for the [site][site1] (essentially doing what we were doing locally).

* This site relies only on DocFx. We (currently) run DocFx locally to produce the website with both conceptual and API documentation, and then push to Github **\docs** folder. The difference with our Jekyll site is that with the DocFx process we are pushing the HTML files and are not relying on Jekyll to build HTML for us. We have plans for automate the process.

For more information about how this site was put together, see the [about][about] page.


[site1]: https://travelmarx.github.io/scrapbook101/
[about]: articles/about-this-site.md
[intro]: articles/index.md
[docfx]: https://dotnet.github.io/docfx/
