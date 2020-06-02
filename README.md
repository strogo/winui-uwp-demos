# Windows UI UWP Demos

This repository contains the DevExpress UWP demo applications that use [WinUI](https://microsoft.github.io/microsoft-ui-xaml/about.html).

## Requirements

Windows 10 April 2018 Update (version 1803 - build 17134) or newer
Visual Studio 2019, version 16.7 Preview 1 or newer

## Getting started

Clone the repository to a working folder, navigate to './src'.

Open the FeatureDemo solution in Visual Studio.

If prompted, install the required NuGet packages.

### DevExpress NuGet Packages

Follow the steps below to add the packages to a solution:

1. [Obtain your NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url).
2. In Visual Studio, go to **Tools | NuGet Package Manager | Manage NuGet Packages for Solution**
3. Open "Settings"...

    ... and add a new NuGet feed with the following credentials:

    **Name:** _DevExpress_  
    **Source:** `https://nuget.devexpress.com/{your feed authorization key}/api`

4. Select the **DevExpress** package source.

5. In the "Browse" tab, search for the _'WindowsDesktop.Wpf'_ keyword and install the **DevExpress.WinUI.Uwp** package for the current project. Read and accept the license agreement.

## Feedback

We'd like to hear from you: wpfteam@devexpress.com

## Copyright

Developer Express Inc. All rights reserved.
