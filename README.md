Ignite
======

Ignite is a framework that allows for management of JavaScript, style sheets (CSS, LESS) and JavaScript 
templates for ASP.NET MVC3.

Quick Start
-----------

1. Install using NuGet: `PM> Install-Package Ignite`
2. Configure your `Global.asax.cs`:

    ```csharp
    public static void RegisterRoutes(RouteCollection routes)
    {
        Ignite.Create()                             // Create a new package container.
            .JavaScript("core", new[]               
            { 
                "js/vendor/underscore-1.3.1.js",    // Add individual files
                "js/vendor/backbone*.js",           // or using wildcards
                "js/app/**/*.js",                   // or recursive search.
                "js/templates/*.jst"                // You can even add your templates!
            })
            .StyleSheet("core", new[]
            {
                "style/**/*.less"                   // The default stylesheet compressor uses DotLess.
            })
            .Map(routes);                           // Let MVC handle requests to these routes.

        // Other routes would go here...
    }
    ```

3. Tell the view to render these tags:

    ```html
    @using Ignite
    <!DOCTYPE html>
    <html>
      <head>
        <title>Testing Ignite</title>
        @Ignite.JavaScriptTag("core")
        @Ignite.StyleSheetTag("core")
    ```
4. Run the application in **Debug** mode (or call `EnableDebugging()` on the container) and view the source:

    ```html
    <!DOCTYPE html>
    <html>
      <head>
      <title>Testing Ignite</title>
        <script type="text/javascript" src="/assets/core.js?debug=js/vendor/underscore-1.3.1.js"></script>
        <script type="text/javascript" src="/assets/core.js?debug=js/vendor/backbone-0.9.1.js"></script>
        <script type="text/javascript" src="/assets/core.js?debug=js/app/1.js"></script>
        <script type="text/javascript" src="/assets/core.js?debug=js/app/2.js"></script>
        <script type="text/javascript" src="/assets/core.js?debug=js/app/3.js"></script>
        <script type="text/javascript" src="/assets/core.js?debug=templates/f1e24613-dc84-4653-bb02-244702e86c17.js"></script>
        <link rel="stylesheet" type="text/css" href="/assets/core.css?debug=style/lib.less" />
        <link rel="stylesheet" type="text/css" href="/assets/core.css?debug=style/test.less" />
    ```

    Ignite is nice enough to render individual tags for each asset to aid in easy debugging.
    
5. Run the application in **Release** mode (or call `DisableDebugging()` on the container) and view the source:

    ```html
    <!DOCTYPE html>
    <html>
      <head>
        <title>Testing Ignite</title>
        <script type="text/javascript" src="/assets/core.js?v=3fa224672904"></script>
        <link rel="stylesheet" type="text/css" href="/assets/core.css?v=9b6e23712485" />
    ```

6. Check out [the wiki](https://github.com/TheCloudlessSky/Ignite/wiki) for detailed configuration such as templates and caching!