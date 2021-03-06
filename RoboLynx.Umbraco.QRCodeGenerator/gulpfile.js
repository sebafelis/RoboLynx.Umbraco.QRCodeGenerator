/// <binding ProjectOpened='watch-less' />
"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    less = require("gulp-less"),
    merge = require("merge-stream"),
    cleanCSS = require('gulp-clean-css'),
    rename = require("gulp-rename"),
    compilerconfig = require("./compilerconfig.json"),
    debug = require("gulp-debug"); // make sure bundleconfig.json doesn't contain any comments

// Watch LESS files changes
function watchLess(cb) {
    getCompiler(".less").map(function (bundle) {
        return gulp.watch(bundle.inputFile, compileAndMinifyLess);
    });

    cb();
}

gulp.task('watch-less', watchLess);

// Compile LESS
function compileLess() {
    var tasks = getCompiler(".less").map(function (bundle) {
        var stream = gulp.src(bundle.inputFile, { base: "." });
        if (typeof filePath !== 'string' || bundle.inputFile == filePath) {
            console.log("Compile less file " + bundle.inputFile + " to " + bundle.outputFile);
            return stream
                .pipe(less())
                .pipe(concat(bundle.outputFile))
                .pipe(gulp.dest("."));
        }
        return stream.pipe(gulp.dest('.'));;
    });

    return merge(tasks);
}

gulp.task("compile-less", compileLess);

// Minify CSS
function minifyCss() {
    var tasks = getCompiler(".less").map(function (bundle) {
        var stream = gulp.src(bundle.outputFile, { base: "." });
        if (typeof filePath !== 'string' || bundle.inputFile == filePath) {
            console.log("Minify css file " + bundle.outputFile);
            return stream
                .pipe(cleanCSS({ compatibility: 'ie8' }))
                .pipe(rename({ suffix: ".min" }))
                .pipe(gulp.dest("."));
        }
        return stream.pipe(gulp.dest('.'));;
    });

    return merge(tasks);
}

gulp.task('minify-css', minifyCss);

// Compile LESS and minify CSS
function compileAndMinifyLess(cb) {
    return gulp.series(compileLess, minifyCss)(cb);
}

gulp.task('compile-and-minify-less', compileAndMinifyLess);

function getCompiler(extension) {
    return compilerconfig.filter(function (bundle) {
        return new RegExp(extension).test(bundle.inputFile);
    });
}