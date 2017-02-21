module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        // concat: {
        //     dist: {
        //         src: [
        //             'Scripts/Custom/commonObjects/*.js',
        //             'Scripts/Custom/*OBJ.js'
        //         ],
        //         dest: 'Scripts/common.js'
        //     }
        // },
        //uglify: {
        //    build: {
        //        src: 'Scripts/common.js',
        //        dest: 'Scripts/common.min.js'
        //    }
        //},
        less: {
            development: {
                options: {
                    paths: ['assets/css'],
                    compress: true
                },
                files: {
                    'Content/styles.css': ['src/less/*.less', 'src/less/common/*.less', 'src/less/manager/*.less', 'src/less/manager/*.less', 'src/less/mentor/*.less', 'src/less/scrum/*.less', 'src/less/trainee/*.less', 'src/less/media/*.less']
                }
            }
        },
        // imagemin: {
        //     dynamic: {
        //         files: [{
        //             expand: true,
        //             cwd: '/images/',
        //             src: ['/images/*.{png,jpg,gif}'],
        //             dest: '/images/'
        //         }]
        //     }
        // },
        watch: {
            concat: {
                files: ['src/less/*.less', 'src/less/common/*.less', 'src/less/manager/*.less', 'src/less/manager/*.less', 'src/less/mentor/*.less', 'src/less/scrum/*.less', 'src/less/trainee/*.less', 'src/less/media/*.less'],
                tasks: ['less'],
                options: {
                    spawn: false
                }
            }
        }
        // watch: {
        //     scripts: {
        //         files: ['src/js/*.js'],
        //         tasks: ['concat'],
        //         options: {
        //             spawn: false,
        //         },
        //     }
        // }
    });
    // grunt.loadNpmTasks('grunt-contrib-concat');
    //grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-less');
    // grunt.loadNpmTasks('grunt-contrib-imagemin');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('default', ['less', 'watch']);

};