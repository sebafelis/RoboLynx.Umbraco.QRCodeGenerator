/// <binding ProjectOpened='watch-less' />
"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    less = require("gulp-less"),
    merge = require("merge-stream"),
    cleanCSS = require('gulp-clean-css'),
    rename = require("gulp-rename"),
    compilerconfig = require("./compilerconfig.json"); // make sure bundleconfig.json doesn't contain any comments

// Watch LESS files changes
gulp.task("watch-less", function() {
    var tasks = getCompiler(".less").map(function (bundle) {
        return gulp.watch(bundle.inputFiles, gulp.series("compile-and-minify-less"));
    });
    return merge(tasks);
});

// Compile LESS
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

// Minify CSS
gulp.task('minify-css', () => {
    var tasks = getCompiler(".less").map(function (bundle) {
        console.log("Minify css file " + bundle.outputFile);
        return gulp.src(bundle.outputFile, { base: "." })
            .pipe(cleanCSS({ compatibility: 'ie8' }))
            .pipe(rename({ suffix: ".min" }))
            .pipe(gulp.dest("."));
    });

    return merge(tasks);
});

// Compile LESS and minify CSS
gulp.task('compile-and-minify-less', gulp.series('less-compile', 'minify-css'));

function getCompiler(extension) {
    return compilerconfig.filter(function (bundle) {
        return new RegExp(extension).test(bundle.inputFile);
    });
}