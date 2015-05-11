module.exports = function (grunt) {
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: "wwwroot/lib",
                    layout: "byComponent",
                    cleanTargetDir: true
                }
            }
        },
        less: {
            development: {
                options: {
                    paths: ["Assets"],
                },
                files: [{
                    src: [
                        'wwwroot/css/*.less'
                    ],
                    expand: true,
                    rename: function (dest, src) { return dest + src.replace('.less', '.css') },
                    dest: ''
                }]
            },
        },
        typescript: {
            base: {
                src: ['Scripts/**/*.ts'],
                dest: 'wwwroot/app.js',
                options: {
                    module: 'amd',
                    target: 'es5'
                }
            }
        },
    });

    grunt.registerTask("default", ["bower:install"]);

    grunt.loadNpmTasks("grunt-bower-task");
    grunt.loadNpmTasks("grunt-contrib-less");
    grunt.loadNpmTasks("grunt-typescript");
};
