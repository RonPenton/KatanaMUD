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
        // Add this JSON object:
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
                    rename: function(dest, src) { return dest + src.replace('.less', '.css') },
                    dest: ''
                }]
            },
        }
    });

    grunt.registerTask("default", ["bower:install"]);

    grunt.loadNpmTasks("grunt-bower-task");
    // Add this line:
    grunt.loadNpmTasks("grunt-contrib-less");
};
