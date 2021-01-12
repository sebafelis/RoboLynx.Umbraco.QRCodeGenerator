/// <binding ProjectOpened='watch-folder' />
"use strict";

var gulp = require("gulp"),
    newer = require('gulp-newer'),
    concat = require("gulp-concat"),
    less = require("gulp-less"),
    merge = require("merge-stream"),
    compilerconfig = require("./compilerconfig.json"); // make sure bundleconfig.json doesn't contain any comments

gulp.task("watch-less", function () {
    getCompiler(".less").forEach(function (bundle) {
        gulp.watch([bundle.inputFiles], gulp.series("less-compile"));
    });
});

gulp.task("watch-plugin", gulp.parallel((done) => {
    gulp.watch(["./App_Plugins/**/*", "!./App_Plugins/**/*.less"], gulp.series("copy-plugin"));

    done();
}));

gulp.task("copy-plugin", () => {
    var dest = "./../TestWebsite/App_Plugins";
    return gulp.src(["./App_Plugins/**/*", "!./App_Plugins/**/*.less"], { base: "App_Plugins" })
        .pipe(newer(dest))
        .pipe(gulp.dest(dest));
});

// Compile less

gulp.task('less-compile', function () {
    var tasks = getCompiler(".less").map(function (bundle) {
        console.log("Compile less file " + bundle.inputFile + " to " + bundle.outputFile);
        return gulp.src(bundle.inputFile, { base: "." })
            .pipe(less())
            .pipe(concat(bundle.outputFile))
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

function getCompiler(extension) {
    return compilerconfig.filter(function (bundle) {
        return new RegExp(extension).test(bundle.inputFile);
    });
}