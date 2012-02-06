(function(root) {

  $(function() {
    var $foo = root.JST["tmpl1"]();
    $("#main > p").append($foo);

    var $foo2 = root.JST["tmpl2"]();
    $("#main > p").append($foo2);

  });

})(this);