---
uid: azure-pipeline
title: Azure Pipeline
---
# Pipeline setup

## Overview

Our goal is to create a Azure pipeline process to build docs (and run other tasks) automatically upon check in of code. To do this, we need to look into Devops. The term is a composed of the terms development (Dev) and operations (Ops) and represents a new way of thinking about development and operation processes as tightly connected together. For more information, see [What is DevOps?][devops-def] In particular, we'll be using Azure Devops, which contains ways to implement continuous integration (CI) and continuous delivery (CD) processes using a *pipeline*. A pipeline in its simplest sense is a series of tasks you want to run. In our case, we want to compile code, optionally run any tests like checking quality, and create documentation with [DocFx][docfx].

Our desired authoring / developer flow is:

1. Code or author.

2. Check in changes.

3. A pipeline automatically builds upon detected changes to code or content.

## Starting setup

To approach the ideal process flow described above, our first steps in devopsland went something like this:

1. Go to [Azure devops][devops].

1. Create a project "Scrapbook101core" in the organization that was created when we signed up.

1. Go to Pipelines section of the project, and create a new Build pipeline.

1. Connect to GitHub, select the Scrapbook101core repo, and Run.

1. Configure the pipeline as "ASP.NET Core". For more information see [Build, test, and deploy .NET Core apps][devops-core]
      
   This will create am azure-pipeline.yml file. Save and run it. (You can commit directly into master or create a branch that you'll have to merge later.) The config file is at the root of the GitHub project.

    ```yaml
    trigger:
    - master

    pool:
    vmImage: 'ubuntu-latest'

    variables:
    buildConfiguration: 'Release'

    steps:
    - script: dotnet build --configuration $(buildConfiguration)
      displayName: 'dotnet build $(buildConfiguration)'
    ```

Notes:

* Do I pay? There is a [free tier][freetier] Azure Dev Ops options to get started. For small tests and limited use, you probably won't pay. For more information, see [Build GitHub repositories][pipeline-github].

* What happened when running a pipeline with the YML file above? The steps built the site as if you ran build in Visual Studio. 

* If you make a changes to any file in the repo, the build process will kick off again because the **trigger** parameter in the pipeline config file.

## Custom build task

So far, so good. Our next step was to play around with the YML config file. First, we read up on [jobs][jobs] and [agents][agents]. Remember, our goal is to build the docs in the pipeline. But before doing that, we need to know about running tasks.

1. Create a simple job. Modify the azure-pipelines.yml file to say "Hello World". Check in the changed file. Build the pipeine, or if you have followed this far, the pipeline will build itself because you changed the YML file and checked in the changes.

    ```yaml
    steps:
    - bash: echo "Hello World"
    - script: dotnet build --configuration $(buildConfiguration)
      displayName: 'dotnet build $(buildConfiguration)'
    ```

1. Create different agents. Add this to config and run.

    ```yaml
    jobs:
    - job: Linux
      pool:
        vmImage: 'ubuntu-latest'
      steps:
      - script: echo hello from Linux
      - bash: echo "Hello World"
      - script: dotnet build --configuration $(buildConfiguration)
        displayName: 'dotnet build $(buildConfiguration)'
    - job: macOS
      pool:
        vmImage: 'macOS-latest'
      steps:
      - script: echo hello from macOS
    - job: Windows
      pool:
        vmImage: 'windows-latest'
      steps:
      - script: echo hello from Windows
    ```

    In the screenshot below, clicking on the **cmdline** task would show the output from the echo commands in the YML config file.

    ![Build pipeline example with three agents](../images/build-pipeline-test-three-agents.jpg "Build pipeline example with three agents")


1. Go back to simpler config file based on **vmImage** = 'windows-latest'. Save  and run.

    ```yaml
    trigger:
    - master

    pool:
    vmImage: 'windows-latest'

    variables:
    buildConfiguration: 'Release'

    steps:
    - script: echo "hello from Windows"
    - script: dotnet build --configuration $(buildConfiguration)
    displayName: 'dotnet build $(buildConfiguration)'
    ```

## PowerShell task

Our next step was to figure out how to run a PowerShell script with the idea that we will create a PowerShell build script. So, let's start with "Hello World".

1. Create a simple PowerShell in the repo at this location: **.\scripts\builddocs.ps1**

    ```ps1
    write-host "Hello World from PowerShell!"
    ```
1. Modify the config above to use this script. (Here's a [help page][pstask].)

    ```yaml
    steps:
    - pwsh: .\scripts\builddocs.ps1
    - script: dotnet build --configuration $(buildConfiguration)
    displayName: 'dotnet build $(buildConfiguration)'
    ```

1. Check both changes in to kick of a pipeline build.

The [hosted Windows 2019][vmImage] we are using has a pre-installed software, including:

* [Powershell Core][ps-core], a cross-platform (Windows, Linux, and macOS) version of PowerShell.

* [Chocolatey][choco], a Windows package manager. We'll use Chocolately to install [DocFx][docfx].

These two components together will be enough to run our builddocs.ps1 script.

## Build task

After a bit of trial and error we were able to build a simplistic build script [builddocs.ps1][build-script].
Here are approximate steps taken:

1. Install Chocolately with ``choco install docfx -y``.

1. Run docfx with ``docfx metadata`` and ``docfx build``.

   Note that we don't need to serve the docs like we might do building locally. Also, we have to make sure we are in the right directory to run these commands so that the docfx.json file is found.

1. Copy all files from **docbuild\_site** to **docs**.

Looking at the task log, you should see that the script path on the agent is: "D:\a\1\s". By default, code is checked out into a directory called "s". Inside the build script, we can change directory for example to: "D:\a\1\s\docbuild". For more information, see [Pipeline options for Git repositories][pipeline-git].


To do:

* What does it mean to be headless Git?


[docfx]: https://dotnet.github.io/docfx/
[devops-def]: https://azure.microsoft.com/en-us/overview/what-is-devops/
[freetier]: https://azure.microsoft.com/en-us/pricing/details/devops/azure-devops-services/
[jobs]: https://docs.microsoft.com/en-us/azure/devops/pipelines/process/phases
[agents]: https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/hosted
[devops]: https://dev.azure.com
[pstask]: https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/powershell?view=azure-devops
[vmImage]: https://github.com/Microsoft/azure-pipelines-image-generation/blob/master/images/win/Vs2019-Server2019-Readme.md
[choco]: https://chocolatey.org
[build-script]: https://github.com/travelmarx/scrapbook101core/blob/master/docbuild/builddocs.ps1
[devops-core]: https://docs.microsoft.com/azure/devops/pipelines/languages/dotnet-core
[ps-core]: https://github.com/powershell/powershell
[pipeline-git]: https://docs.microsoft.com/en-us/azure/devops/pipelines/repos/pipeline-options-for-git
[pipeline-github]: https://docs.microsoft.com/en-us/azure/devops/pipelines/repos/github