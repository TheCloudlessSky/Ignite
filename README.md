Ignite
======

Ignite is a simple to use framework that combines JavaScript, style sheet (CSS, LESS) and JavaScript template management.

Quick Start
-----------

1. Create your packages inside of `Global.asax.cs`:

    public static void RegisterRoutes(RouteCollection routes)
    {
        var container = Ignite.Create("assets");
        container.JavaScript("core", new[]
        { 
            "scripts/vendor/underscore*.js",
            "scripts/vendor/backbone*.js",
            "scripts/a/*.js",
            "scripts/b/*.js",
            "scripts/templates/*.jst"
        });

        container.StyleSheet("core", new[]
        {
            "content/**/*.less"
        });
        container.Build(routes);

        // Other routes here.
    }

2. 