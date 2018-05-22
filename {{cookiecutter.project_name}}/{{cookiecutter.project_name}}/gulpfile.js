"use strict";

var gulp = require("gulp"),
    del = require("del");

gulp.task("clean", function (cb) {
    del(['wwwroot/lib/**/*'], cb);
});

gulp.task('copy', function () {    
    gulp.src('bower_components/bitadmin-core/**/*')
        .pipe(gulp.dest('wwwroot/lib'));
});


