"use strict";

var gulp = require("gulp"),
    newer = require("gulp-newer"),
    imagemin = require("gulp-imagemin"),
    sass = require("gulp-sass"),
    sourcemaps = require("gulp-sourcemaps"),
    autoprefixer = require("gulp-autoprefixer"),
    cleanCSS = require('gulp-clean-css'),
    rename = require("gulp-rename"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    lodash = require("lodash"),
    browsersync = require("browser-sync"),
    fileinclude = require('gulp-file-include'),
    log = require('fancy-log');
  
var folder = {
    src: "wwwroot/src/", // source files
    dist: "wwwroot/dist/", // build files
    dist_assets: "wwwroot/dist/assets/" //build assets files
};

function copyAssets (done) {
  var assets = {
    js: [
      "./node_modules/jquery/dist/jquery.js",
      "./node_modules/bootstrap/dist/js/bootstrap.bundle.js",
      "./node_modules/pace-js/pace.min.js"
    ],
    scss: [
      "./node_modules/animate.css/animate.css",
      "./node_modules/pace-js/themes/orange/pace-theme-minimal.css"
    ] 
  };

  var plugin_assets = {
    js_css: [
      { "name": "sweetalert2", "assets": ["./node_modules/sweetalert2/dist/sweetalert2.all.min.js"] },
      { "name": "jstree", "assets": ["./node_modules/jstree/dist/jstree.min.js", "./node_modules/jstree/dist/themes/default/style.min.css"] },
      { "name": "fuelux", "assets": ["./node_modules/fuelux/dist/js/fuelux.min.js"] },
      { "name": "parsleyjs", "assets": ["./node_modules/parsleyjs/dist/parsley.min.js"] },
      { "name": "jquery-ui", "assets": ["./node_modules/components-jqueryui/jquery-ui.min.js", "./node_modules/components-jqueryui/themes/base/jquery-ui.min.css"] },
      { "name": "nestable2", "assets": ["./node_modules/nestable2/dist/jquery.nestable.min.js", "./node_modules/nestable2/dist/jquery.nestable.min.css"] },
      { "name": "justgage", "assets": ["./node_modules/justgage/dist/justgage.min.js", "./node_modules/justgage/raphael.min.js"] },
      { "name": "dropzone", "assets": ["./node_modules/dropzone/dist/min/dropzone.min.js", "./node_modules/dropzone/dist/min/dropzone.min.css"] },
      { "name": "bootstrap-social", "assets": ["./node_modules/bootstrap-social/bootstrap-social.css"] },
      { "name": "pwstrength-bootstrap", "assets": ["./node_modules/pwstrength-bootstrap/dist/pwstrength-bootstrap.min.js"] },
      { "name": "hideshowpassword", "assets": ["./node_modules/hideshowpassword/hideshowpassword.min.js"] },
      { "name": "jquery.maskedinput", "assets": ["./node_modules/jquery.maskedinput/src/jquery.maskedinput.js"] },
      { "name": "switchery-latest", "assets": ["./node_modules/switchery-latest/dist/switchery.min.js", "./node_modules/switchery-latest/dist/switchery.min.css"] },
      { "name": "parsleyjs", "assets": ["./node_modules/parsleyjs/dist/parsley.min.js"] },
      { "name": "bootstrap-multiselect", "assets": ["./node_modules/bootstrap-multiselect/dist/js/bootstrap-multiselect.min.js", "./node_modules/bootstrap-multiselect/dist/css/bootstrap-multiselect.css"] },
      { "name": "bootstrap-datepicker", "assets": ["./node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js", "./node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker3.min.css"] },
      { "name": "bootstrap-colorpicker", "assets": ["./node_modules/bootstrap-colorpicker/dist/js/bootstrap-colorpicker.min.js", "./node_modules/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css"] },
      { "name": "clockpicker", "assets": ["./node_modules/clockpicker/dist/bootstrap-clockpicker.min.js", "./node_modules/clockpicker/dist/bootstrap-clockpicker.min.css"] },
      { "name": "nouislider", "assets": ["./node_modules/nouislider/distribute/nouislider.min.js", "./node_modules/wnumb/wNumb.js", "./node_modules/nouislider/distribute/nouislider.min.css"] },
      { "name": "bootstrap-slider", "assets": ["./node_modules/bootstrap-slider/dist/bootstrap-slider.min.js", "./node_modules/bootstrap-slider/dist/css/bootstrap-slider.min.css"] },
      { "name": "select2", "assets": ["./node_modules/select2/dist/js/select2.min.js", "./node_modules/select2/dist/css/select2.min.css"] },
      { "name": "jquery-jeditable", "assets": ["./node_modules/jquery-jeditable/dist/jquery.jeditable.min.js",
                                                "./node_modules/jquery-jeditable/dist/jquery.jeditable.autogrow.min.js",
                                                "./node_modules/jquery-jeditable/dist/jquery.jeditable.charcounter.min.js",
                                                "./node_modules/jquery-jeditable/dist/jquery.jeditable.datepicker.min.js",
                                                "./node_modules/jquery-jeditable/dist/jquery.jeditable.masked.min.js",
                                                "./node_modules/jquery-jeditable/dist/jquery.jeditable.time.min.js",
                                                "./node_modules/jquery-jeditable/src/jquery.jeditable.checkbox.js",
                                                "./node_modules/jquery-jeditable/demos/js/jquery.autogrowtextarea.js",
                                                "./node_modules/jquery-jeditable/demos/js/jquery.charcounter.js",
                                              ]},
      { "name": "jquery-jeditable/php-demo", "assets": ["./src/php-demo/*"] },
      { "name": "dropify/js", "assets": ["./node_modules/dropify/dist/js/dropify.min.js"] },
      { "name": "dropify/css", "assets": ["./node_modules/dropify/dist/css/dropify.min.css",] },
      { "name": "dropify/fonts", "assets": ["./node_modules/dropify/dist/fonts/*"] },
      { "name": "summernote", "assets": ["./node_modules/summernote/dist/summernote-bs4.min.js", "./node_modules/summernote/dist/summernote-bs4.css"] },
      { "name": "summernote/font", "assets": ["./node_modules/summernote/dist/font/*"] },
      { "name": "summernote/lang", "assets": ["./node_modules/summernote/dist/lang/*"] },
      { "name": "bootstrap-markdown", "assets": ["./node_modules/bootstrap-markdown/js/bootstrap-markdown.js", "./node_modules/bootstrap-markdown/css/bootstrap-markdown.min.css"] },
      { "name": "markdown", "assets": ["./node_modules/markdown/lib/markdown.js"] },
      { "name": "to-markdown", "assets": ["./node_modules/to-markdown/dist/to-markdown.js"] },
      { "name": "datatables.net", "assets": ["./node_modules/datatables.net/js/jquery.dataTables.min.js"] },
      { "name": "datatables.net-bs4", "assets": ["./node_modules/datatables.net-bs4/js/dataTables.bootstrap4.min.js", "./node_modules/datatables.net-bs4/css/dataTables.bootstrap4.min.css"] },
      { "name": "datatables.net-buttons/js", "assets": ["./node_modules/datatables.net-buttons/js/*"] },
      { "name": "datatables.net-buttons/swf", "assets": ["./node_modules/datatables.net-buttons/swf/*"] },
      { "name": "datatables.net-buttons-bs4", "assets": ["./node_modules/datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js", "./node_modules/datatables.net-buttons-bs4/css/buttons.bootstrap4.min.css"] },
      { "name": "datatables.net-colreorder", "assets": ["./node_modules/datatables.net-colreorder/js/dataTables.colReorder.min.js"] },
      { "name": "datatables.net-colreorder-bs4", "assets": ["./node_modules/datatables.net-colreorder-bs4/js/colReorder.bootstrap4.min.js", "./node_modules/datatables.net-colreorder-bs4/css/colReorder.bootstrap4.min.css"] },
      { "name": "jszip", "assets": ["./node_modules/jszip/dist/jszip.min.js"] },
      { "name": "pdfmake", "assets": ["./node_modules/pdfmake/build/pdfmake.min.js", "./node_modules/pdfmake/build/vfs_fonts.js"] },
      { "name": "chart.js", "assets": ["./node_modules/chart.js/dist/Chart.min.js", "./node_modules/chart.js/dist/Chart.min.css"] },
      { "name": "chartist", "assets": ["./node_modules/chartist/dist/chartist.min.js", "./node_modules/chartist/dist/chartist.min.css"] },
      { "name": "flot", "assets": ["./node_modules/flot/dist/es5/jquery.flot.js", "./node_modules/flot/source/*"] },
      { "name": "jquery.flot.tooltip", "assets": ["./node_modules/jquery.flot.tooltip/js/jquery.flot.tooltip.min.js"] },
      { "name": "jquery-sparkline", "assets": ["./node_modules/jquery-sparkline/jquery.sparkline.min.js"] },
      { "name": "jquery.appear", "assets": ["./node_modules/jquery.appear/jquery.appear.js"]},
      { "name": "easy-pie-chart", "assets": ["./node_modules/easy-pie-chart/dist/jquery.easypiechart.min.js"]},
      { "name": "toastr", "assets": ["./node_modules/toastr/build/toastr.min.js", "./node_modules/toastr/build/toastr.min.css"]},
      { "name": "jqvmap", "assets": ["./node_modules/jqvmap/dist/jquery.vmap.min.js", "./node_modules/jqvmap/dist/jqvmap.min.css"]},
      { "name": "jqvmap/maps", "assets": ["./node_modules/jqvmap/dist/maps/*"]},
      { "name": "raphael", "assets": ["./node_modules/raphael/raphael.min.js"]},
      { "name": "jquery-mousewheel", "assets": ["./node_modules/jquery-mousewheel/jquery.mousewheel.js"]},
      { "name": "jquery-mapael", "assets": ["./node_modules/jquery-mapael/js/jquery.mapael.min.js"]},
      { "name": "jquery-mapael/maps", "assets": ["./node_modules/jquery-mapael/js/maps/*"]},
      { "name": "slick-carousel", "assets": ["./node_modules/slick-carousel/slick/slick.min.js", "./node_modules/slick-carousel/slick/slick.css", "./node_modules/slick-carousel/slick/slick-theme.css", "./node_modules/slick-carousel/slick/ajax-loader.gif"]},
      { "name": "slick-carousel/fonts", "assets": ["./node_modules/slick-carousel/slick/fonts/*"]},
      { "name": "bootstrap-tour", "assets": ["./node_modules/bootstrap-tour/build/js/*", "./node_modules/bootstrap-tour/build/css/*"]},
    ]
  }

  //copying plugin assets
  lodash(plugin_assets).forEach(function (assets, type) {
    if (type == "js_css") {
        lodash(assets).forEach(function (plugin) {
            var name = plugin['name'];
            var assetlist = plugin['assets'];
            lodash(assetlist).forEach(function (asset) {
                gulp.src(asset).pipe(gulp.dest(folder.dist_assets + "plugins/" + name));
            });
        });
    }
  });

  //copying required assets
  lodash(assets).forEach(function (assets, type) {
    if (type == "scss") {
        gulp
            .src(assets)
            .pipe(
                rename({
                    prefix: "_",
                    extname: ".scss"
                })
            )
            .pipe(gulp.dest(folder.src + "scss/vendor"));
    } else {
        gulp.src(assets).pipe(gulp.dest(folder.src + "js/vendor"));
    }
  });

  //copying data files
  gulp.src(folder.src + "data/**").pipe(gulp.dest(folder.dist_assets + "/data"));
  done();
}

function imageMin() {
  var output = folder.dist_assets + "images";
  return gulp
      .src(folder.src + "images/**/*")
      .pipe(newer(output))
      .pipe(imagemin())
      .pipe(gulp.dest(output));
}

function fonts() {
  var output = folder.dist_assets + "fonts/";

  return gulp.src([folder.src + "fonts/**/*"]).pipe(gulp.dest(output));
}

function html() {
  var out = folder.dist;
  return gulp
    .src([
        folder.src + "html/*.html",
    ])
    .pipe(fileinclude({
        prefix: '@@',
        basepath: '@file',
        indent: true
    }))
    .pipe(gulp.dest(out));
}

function css() {
  gulp
  .src([folder.src + "/scss/bootstrap-custom.scss"])
    .pipe(sourcemaps.init())
    .pipe(sass())
    .pipe(
      autoprefixer({
        overrideBrowserslist: ["last 2 versions"]
      })
    )
    .pipe(gulp.dest(folder.dist_assets + "css/"))
    .pipe(cleanCSS())
    .pipe(
        rename({
            suffix: ".min"
        })
    )
    .pipe(sourcemaps.write("./"))
    .pipe(gulp.dest(folder.dist_assets + "css/"));

  gulp
  .src([folder.src + "/scss/skins/*.scss"])
    .pipe(sourcemaps.init())
    .pipe(sass())
    .pipe(
      autoprefixer({
        overrideBrowserslist: ["last 2 versions"]
      })
    )
    .pipe(gulp.dest(folder.dist_assets + "css/skins"))
    .pipe(cleanCSS())
    .pipe(
        rename({
            suffix: ".min"
        })
    )
    .pipe(sourcemaps.write("./"))
    .pipe(gulp.dest(folder.dist_assets + "css/skins"));
  
  return gulp
    .src([folder.src + "/scss/app.scss"])
    .pipe(sourcemaps.init())
    .pipe(sass())
    .pipe(
        autoprefixer({
            overrideBrowserslist: ["last 2 versions"]
        })
    )
    .pipe(gulp.dest(folder.dist_assets + "css/"))
    .pipe(cleanCSS())
    .pipe(
      rename({
          suffix: ".min"
      })
    )
    .pipe(sourcemaps.write("./"))
    .pipe(gulp.dest(folder.dist_assets + "css/"));
}

function js() {
  var output = folder.dist_assets + "js/";

  var demo_pages_assets = {
    js: [
      folder.src + "js/pages/sweetalert2.init.js",
      folder.src + "js/pages/jstree.init.js",
      folder.src + "js/pages/wizard.init.js",
      folder.src + "js/pages/ui-dragdroppanel.init.js",
      folder.src + "js/pages/nestable2.init.js",
      folder.src + "js/pages/gauge.init.js",
      folder.src + "js/pages/panels.init.js",
      folder.src + "js/pages/forms-inputs.init.js",
      folder.src + "js/pages/forms-validations.init.js",
      folder.src + "js/pages/forms-multiselect.init.js",
      folder.src + "js/pages/forms-input-pickers.init.js",
      folder.src + "js/pages/forms-input-sliders.init.js",
      folder.src + "js/pages/forms-select2.init.js",
      folder.src + "js/pages/forms-inplace-editing.init.js",
      folder.src + "js/pages/forms-dragdropupload.init.js",
      folder.src + "js/pages/forms-texteditor.init.js",
      folder.src + "js/pages/tables-dynamic.init.js",
      folder.src + "js/pages/charts-chartjs.init.js",
      folder.src + "js/pages/charts-chartist.init.js",
      folder.src + "js/pages/charts-flot.init.js",
      folder.src + "js/pages/charts-sparkline.init.js",
      folder.src + "js/pages/widgets.init.js",
      folder.src + "js/pages/notifications.init.js",
      folder.src + "js/pages/maps-jqvmap.init.js",
      folder.src + "js/pages/maps-mapael.init.js",
      folder.src + "js/pages/dashboard.init.js",
      folder.src + "js/pages/dashboard2.init.js",
      folder.src + "js/pages/dashboard3.init.js",
      folder.src + "js/pages/dashboard4.init.js",
      folder.src + "js/pages/appviews-projects.init.js",
    ]
  };

  lodash(demo_pages_assets).forEach(function (assets, type) {
    gulp.src(assets)
    .pipe(gulp.dest(output + "pages"))
    .pipe(
      rename({
          suffix: ".min"
      })
    )
    .pipe(uglify())
    .on("error", function (err) {
      log.error(err.toString());
    })
    .pipe(gulp.dest(output + "pages"));
  });

  gulp
    .src([
      folder.src + "js/vendor/jquery.js",
      folder.src + "js/vendor/bootstrap.bundle.js",
      folder.src + "js/vendor/pace.min.js",
    ])
    .pipe(sourcemaps.init())
    .pipe(concat("vendor.js"))
    .pipe(gulp.dest(output))
    .pipe(
      rename({
          suffix: ".min"
      })
    )
    .pipe(uglify())
    .on("error", function (err) {
        log.error(err.toString());
    })
    .pipe(sourcemaps.write("./"))
    .pipe(gulp.dest(output));

    return gulp
      .src([
        folder.src + "js/app.js"
      ])
      .pipe(sourcemaps.init())
      .pipe(concat("app.js"))
      .pipe(gulp.dest(output))
      .pipe(
        rename({
            suffix: ".min"
        })
      )
      .pipe(uglify())
      .on("error", function (err) {
        log.error(err.toString());
      })
      .pipe(sourcemaps.write("./"))
      .pipe(gulp.dest(output));
}

// live browser loading
function browserSync(done) {
  browsersync.init({
      server: {
          baseDir: folder.dist,
          middleware: [
              function (req, res, next) {
                  req.method = 'GET';
                  next();
              }
          ]
      }
  });
  done();
}

function reloadBrowserSync(done) {
  browsersync.reload();
  done();
}

function watchFiles(done) {
  gulp.watch(folder.src + "html/**", gulp.series(html, reloadBrowserSync));
  gulp.watch(folder.src + "assets/images/**/*", gulp.series(imageMin, reloadBrowserSync));
  gulp.watch(folder.src + "assets/fonts/**/*", gulp.series(fonts, reloadBrowserSync));
  gulp.watch(folder.src + "scss/**/*", gulp.series(css, reloadBrowserSync));
  gulp.watch(folder.src + "js/**/*", gulp.series(js, reloadBrowserSync));
  done();
}
// watch all changes
gulp.task("watch", gulp.parallel(watchFiles, browserSync));

// default task
gulp.task(
  "default",
  gulp.series(
      copyAssets,
      html,
      imageMin,
      fonts,
      css,
      js
  ),
  function(done) {done();}
);

