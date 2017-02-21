$(document).ready(function () {
    //RegExp for e-mail validation
    //    var RV_EMAIL = /^([\w-]+\.)*[\w\.-]+@[\w-]+(\.[\w-]+)*\.[a-zA-Z]{2,6}$/i;
    var RV_EMAIL = /^\s*([\wА-ЯЁ-]+\.)*[\wА-ЯЁ\.-]+@[\wА-ЯЁ-]+(\.[\wА-ЯЁ-]+)*\.[a-zA-ZА-ЯЁ]{2,6}\s*$/i;
    var RV_DATE = /(0[1-9]|1[012])\/[0-9]{4} – (0[1-9]|1[012])\/[0-9]{4}/;
    var RV_SKYPE = /[А-ЯЁ]/i;
    var RV_LANGUAGE = /^\s*[А-ЯЁ]*\s*$/i;
    var RV_SOFTSKILL = /^\s*[А-ЯЁ\w@)(,\s-#$~`%^&*'+=\\<>\|\/\}\{.\]\[":;?\!]*\s*$/i;
    var RV_URL = /^\s*(https?:\/\/){1}(\S*[^@]){6,}\s*$/i;

    var MAX_EDUCATION = 3;
    var MAX_COURSES = 7;
    var MAX_LANGUAGES = 7;
    var MAX_SOFTSKILLS = 5;

    var MAX_SIZE = {};
    MAX_SIZE.ppt = 2.5 * 1024 * 1024;
    MAX_SIZE.table = 200 * 1024;
    MAX_SIZE.image = 500 * 1024;
    MAX_SIZE.other = 300 * 1024;
    MAX_SIZE.total = 4 * 1024 * 1024;

    var leftSidebar = $('#leftSidebar');
    var profileId = $('.prf-questionary-up').attr("traineeId");
    console.log('profileId= ' + profileId);

    var beginForm = {};

    var $errorMessage = $('<span class="prf-error-box">Поле заполнено неверно. Введите данные.</span>');
    var $skillsCountMessage = $('<span class="prf-error-box">Количество навыков не должно превышать ' + MAX_SOFTSKILLS + '.</span>');

    //function to get Inputs values by ID in Object
    function getInputs2(formIn, outObject) {
        formIn.find('input, textarea').each(function () {
            var inputId = $(this).attr("id");
            var inputVal = $(this).val();
            outObject[inputId] = inputVal;
        });
    }

    //function to get Inputs values by Data in Object
    function getInputs4(formIn, outObject, data) {
        formIn.find('input, textarea, select').each(function () {
            var inputData = $(this).data(data);
            var inputVal = ($(this).val() == null) ? 0 : $(this).val();
            outObject[inputData] = inputVal;
        });
    }

    //function to disable Form  
    function disabledForm(formIn) {
        formIn.find('.prf-edit-now').removeClass('prf-edit-now').removeAttr("disabled");
        $('.prf-edit').removeAttr("disabled").css('color', '');
        formIn.find('input, textarea, select').attr("disabled", "disabled");
        formIn.find('button').not('.prf-edit').hide();
    }

    //function to update Form 
    function formUpdate(Form) {
        var elemId, newVal;
        for (var key in Form) {
            if (key !== 'profileId') {
                elemId = '#' + key;
                newVal = Form[key];
                $(elemId).val(newVal);
            };
        };
    }

    //function to update Education Form values
    function formUpdateEducation(dataObject) {
        var dataObject = dataObject;
        var universities = dataObject['universities'];
        var courses = dataObject['courses'];

        $('form[name="traineeEducation"]').find('.prf-university').each(function (index) {
            var itemEducation = $(this).filter(function () {
                return $(this).data("university-id") == universities[index]['universityId']
            });
            var inputsEducation = itemEducation.find('input, textarea');
            inputsEducation.each(function () {
                var dataName = $(this).data('university');
                $(this).val(universities[index][dataName]);
            });
        });

        $('form[name="traineeEducation"]').find('.prf-course').each(function (index) {
            var itemCourse = $(this).filter(function () {
                return $(this).data("course-id") == courses[index]['courseId']
            });
            var inputsCourse = itemCourse.find('input, textarea');
            inputsCourse.each(function () {
                var dataName = $(this).data('course');
                $(this).val(courses[index][dataName]);
            });
        });
    }

    //function to update Skills Form values
    function formUpdateSkills(dataObject) {
        var dataObject = dataObject;
        var mainSkills = dataObject['MainSkills'];
        var softSkills = dataObject['SoftSkills'];
        console.log('formUpdateSkills!!!');
        console.log('mainSkills= ' + JSON.stringify(mainSkills));
        //Update Main Skills
        $('form[name="traineeSkills"]').find('.prf-edit-skill').each(function (index) {
            var itemMainSkills = $(this).filter(function () {
                return $(this).data("spec-id") == mainSkills[index]['SpecId']
            });
            var inputsMainSkills = itemMainSkills.find('input[type="checkbox"]');
            console.log('SkillsIdArray= ' + mainSkills[index]['SkillsId']);

            inputsMainSkills.each(function () {
                $(this).prop('checked', false);
            });

            var checkedArray = mainSkills[index]['SkillsId'];
            for (var i = 0; i < checkedArray.length; i++) {
                inputsMainSkills.each(function () {
                    if ($(this).data('main-skill-id') == checkedArray[i]) {
                        $(this).prop('checked', true);
                    };
                });
            }
        });


        console.log('softSkills= ' + JSON.stringify(softSkills));
        //Update Soft Skills
        $('form[name="traineeSkills"]').find('.prf-soft-skill').each(function (index) {
            var itemSoftSkills = $(this).filter(function () {
                return $(this).data("soft-skill-id") == softSkills[index]['idSoftSkill']
            });
            var inputsSoftSkills = itemSoftSkills.find('input');
            inputsSoftSkills.each(function () {
                var dataName = $(this).data('soft-skill');
                $(this).val(softSkills[index][dataName]);
            });
        });
    }

    //function to update Language Form values
    function formUpdateLanguage(dataObject) {
        var dataObject = dataObject;
        var languages = dataObject['languages'];

        $('form[name="traineeLanguage"]').find('.prf-language').each(function (index) {
            var itemLanguage = $(this).filter(function () {
                return $(this).data("language-id") == languages[index]['languageId']
            });
            var inputsLanguage = itemLanguage.find('input, select');
            inputsLanguage.each(function () {
                var dataName = $(this).data('language');
                $(this).val(languages[index][dataName]);
            });
        });
    }

    //function to get Education Form  Data for output
    function formDataEducation(outData, inForm) {
        var outData = outData;
        var inForm = inForm;
        outData['profileId'] = profileId;

        /* output data for Educations */
        outData['universities'] = [];
        var $university = inForm.find('.prf-university');
        console.log('$university=' + $university.length);
        $university.each(function () {
            var help = {};
            var universityId = $(this).data('university-id');
            help['universityId'] = universityId;
            getInputs4($(this), help, 'university');
            outData['universities'].push(help);
        });

        /* output data for Courses */
        outData['courses'] = [];
        var $course = inForm.find('.prf-course');
        console.log('$course=' + $course.length);
        $course.each(function () {
            var help = {};
            var courseId = $(this).data('course-id');
            help['courseId'] = courseId;
            getInputs4($(this), help, 'course');
            outData['courses'].push(help);
        });
    }

    //function to get Skills Form  Data for output
    function formDataSkills(outData, inForm) {
        var outData = outData;
        var inForm = inForm;
        outData['profileId'] = profileId;

        /* output data for MainSkills */
        outData['MainSkills'] = [];
        var $mainSkills = inForm.find('.prf-edit-skill');
        console.log('$mainSkills=' + $mainSkills.length);
        $mainSkills.each(function () {
            var help = {};
            var specId = $(this).data('spec-id');
            help['SpecId'] = specId;
            help['SkillsId'] = [];
            $(this).find('input[type="checkbox"]').each(function () {
                if ($(this).is(":checked")) {
                    var mainSkillId = $(this).data('main-skill-id');
                    help['SkillsId'].push(mainSkillId);
                }
            });
            outData['MainSkills'].push(help);
        });

        /* output data for Soft Skills */
        outData['SoftSkills'] = [];
        var $softSkills = inForm.find('.prf-soft-skill');
        console.log('$softSkills=' + $softSkills.length);
        $softSkills.each(function () {
            var help = {};
            var softSkillId = $(this).data('soft-skill-id');
            help['idSoftSkill'] = softSkillId;
            getInputs4($(this), help, 'soft-skill');
            outData['SoftSkills'].push(help);
        });
    }

    //function to get Language Form  Data for output
    function formDataLanguage(outData, inForm) {
        var outData = outData;
        var inForm = inForm;
        outData['profileId'] = profileId;

        /* output data for Languages */
        outData['languages'] = [];
        var $language = inForm.find('.prf-language');
        console.log('$languagey=' + $language.length);
        $language.each(function () {
            var help = {};
            var languageId = $(this).data('language-id');
            help['languageId'] = languageId;

            getInputs4($(this), help, 'language');
            outData['languages'].push(help);
        });
    }

    //function to show/hide AddButtons according maxCount
    function showHideAddButtons(button, elments, maxCount) {
        if ($(elments).length > maxCount) {
            $(button).hide();
        } else {
            $(button).show();
        };
    }

    //function to show/hide AddButtons according maxCount
    function showHideMessage(message, elments, maxCount) {
        if ($(elments).length <= maxCount) {
            $(message).hide();
        } else {
            $(message).show();
        };
    }

    //function to clone by Shablon
    function cloneShablon(idShablon, idOut) {
        $(idShablon).clone()
                    .removeAttr('id')
                    .removeClass('hidden')
                    .appendTo($(idOut));
    }

    //function to create New Education Form from JSON-object
    function formNewEducation(dataObject) {
        var dataObject = dataObject;
        var universities = dataObject['universities'];
        var courses = dataObject['courses'];

        $('#University').children().remove();
        for (var i = 0; i < universities.length; i++) {
            cloneShablon('#blankUniversity', '#University');
            var $el = $('#University').find('.prf-university').last();
            console.log(universities[i]);
            $el.attr('data-university-id', universities[i]["universityId"]);
            var inputsUniversity = $el.find('input, textarea');
            inputsUniversity.each(function () {
                var dataName = $(this).data('university');
                $(this).val(universities[i][dataName]);
                $(this).attr("disabled", "disabled");
            });
        };

        $('#Course').children().remove();
        for (var i = 0; i < courses.length; i++) {
            cloneShablon('#blankCourse', '#Course');
            var $el = $('#Course').find('.prf-course').last();
            console.log(courses[i]);
            $el.attr('data-course-id', courses[i]["courseId"]);
            var inputsCourse = $el.find('input, textarea');
            inputsCourse.each(function () {
                var dataName = $(this).data('course');
                $(this).val(courses[i][dataName]);
                $(this).attr("disabled", "disabled");
            });
        };
    }

    //function to create New Skills Form from JSON-object
    function formNewSkills(dataObject) {
        var dataObject = dataObject;
        var mainSkills = dataObject['MainSkills'];
        var softSkills = dataObject['SoftSkills'];

        //Update Main Skills
        $('form[name="traineeSkills"]').find('.prf-edit-skill').each(function (index) {
            var itemMainSkills = $(this).filter(function () {
                return $(this).data("spec-id") == mainSkills[index]['SpecId']
            });
            var inputsMainSkills = itemMainSkills.find('input[type="checkbox"]');
            console.log('SkillsIdArray= ' + mainSkills[index]['SkillsId']);

            inputsMainSkills.each(function () {
                $(this).prop('checked', false);
            });

            var checkedArray = mainSkills[index]['SkillsId'];
            for (var i = 0; i < checkedArray.length; i++) {
                inputsMainSkills.each(function () {
                    if ($(this).data('main-skill-id') == checkedArray[i]) {
                        $(this).prop('checked', true);
                    };
                });
            }
        });

        //Create new Soft Skills
        $('#SoftSkills').children().remove();
        for (var i = 0; i < softSkills.length; i++) {
            cloneShablon('#blankSoftSkill', '#SoftSkills');
            var $el = $('#SoftSkills').find('.prf-soft-skill').last();
            console.log(softSkills[i]);
            $el.attr('data-soft-skill-id', softSkills[i]["idSoftSkill"]);
            var inputsSoftSkills = $el.find('input[type="text"]');
            inputsSoftSkills.each(function () {
                var dataName = $(this).data('soft-skill');
                $(this).val(softSkills[i][dataName]);
                $(this).attr("disabled", "disabled");
            });
        };
    }

    //function to create New Language Form from JSON-object
    function formNewLanguage(dataObject) {
        var dataObject = dataObject;
        var languages = dataObject['languages'];

        $('#Language').children().remove();
        for (var i = 0; i < languages.length; i++) {
            cloneShablon('#blankLanguage', '#Language');
            var $el = $('#Language').find('.prf-language').last();
            console.log(languages[i]);
            $el.attr('data-language-id', languages[i]["languageId"]);
            var inputsLanguage = $el.find('input, select');
            inputsLanguage.each(function () {
                var dataName = $(this).data('language');
                $(this).val(languages[i][dataName]);

                if ($(this).hasClass('prf-language-datalist')) {
                    var newList = 'language_list_' + languages[i]["languageId"];
                    $(this).attr('list', newList);
                    $(this).next('datalist').attr('id', newList);
                };

                $(this).attr("disabled", "disabled");
            });
        };
    }

    //function to create New File from JSON-object
    function createNewFile(dataObject) {
        var data = dataObject;
        var $el = $('#blankFile').clone()
                    .removeAttr('id')
                    .removeClass('hidden')
                    .attr('data-file-id', data.id)
                    .attr('data-file-name', data.name)
                    .attr('data-file-size', data.size);
        $el.find('.prf-signature').text(data.description);
        changeImgFileByType($el);

        var $files = $('#FilesLinks').find('.prf-file');
        if ($files.length) {
            $files.last().after($el);
        } else {
            $('#FilesLinks').prepend($el);
        }
    }

    //function to create New Link from JSON-object
    function createNewLink(dataObject) {
        var data = dataObject;
        var $el = $('#blankLink').clone()
                    .removeAttr('id')
                    .removeClass('hidden')
                    .attr('data-link-id', data.id)
                    .attr('data-link', data.link);
        $el.find('.prf-signature').text(data.description);

        var $links = $('#FilesLinks').find('.prf-link');
        if ($links.length) {
            $links.last().after($el);
        } else {
            $('#FilesLinks').append($el);
        }
    }

    //function to remove Object From Array by Key    
    function removeObjectFromArrayByKey(Array, keyName, keyVal) {
        for (var i = 0; i < Array.length; i++) {
            if (Array[i][keyName] == keyVal) {
                Array.splice(i, 1);
            }
        };
    }

    //function to restrict File according MAX_SIZE
    function restrictFile(newFile) {
        var fileName = newFile[0].name;
        var nameArr = fileName.split('.');
        var extension = nameArr[nameArr.length - 1];
        var type = fileType.appointType(extension);
        var fileSize = newFile[0].size;

        switch (type) {
            case 'ppt':
            case 'xls':
            case 'table':
            case 'image':
                extension = type;
                break;

            default:
                extension = 'other';
                break;
        };

        if (fileSize > MAX_SIZE[extension]) {
            $('.prf-restrictions').find('ul').addClass('prf-blink');
            var restrictSize = MAX_SIZE[extension] / (1024 * 1024);  //Convert to Mb
            restrictSize = restrictSize.toFixed(1);
            $('.prf-attention').text('файл > ' + restrictSize + ' Мб');
            return false;
        } else {
            $('.prf-restrictions').find('ul').removeClass('prf-blink');
            $('.prf-attention').text('');
        };

        var allFileSize = 0;
        console.log('allFileSize= ' + allFileSize);

        //Work with max total size
        $('.prf-file').each(function () {
            allFileSize += Number($(this).attr('data-file-size'));
            console.log('allFileSize= ' + allFileSize);
        });

        allFileSize += fileSize;
        console.log('allFileSize= ' + allFileSize);

        if (allFileSize > MAX_SIZE.total) {
            $('.prf-restrictions').find('p').last().addClass('prf-blink');
            return false;
        } else {
            $('.prf-restrictions').find('p').last().removeClass('prf-blink');
        };
    }

    //function to clear inputs of form
    function clearInputsForm($form) {
        $form.find('input').val('');
    }

    //function to indicate Ajax in submit button and disabled form buttons
    function indicateAjax($formButtons, $submitMessage, changeText) {
        if (changeText === undefined) { changeText = 'Сохраняется' };
        $formButtons.attr("disabled", "disabled");
        $submitMessage.text(changeText).addClass('prf-blink');
    }

    //function to recover form buttons after Ajax
    function endAjax($formButtons, $submitMessage, changeText) {
        if (changeText === undefined) { changeText = 'Сохранить' };
        $formButtons.removeAttr("disabled");
        $submitMessage.text(changeText).removeClass('prf-blink');
    }

    /// ******************** MAIN ACTIONS ********************* ///

    leftSidebar.hide();

    // Set handler for Left Sidebar
    $('#toggleSidebar').on('click', function () {
        leftSidebar.toggle();
    });

    //Set tooltip
    $('[data-toggle="tooltip"]').tooltip();

    // Call print
    $("#callPrint").on("click", function (e) {
        e.preventDefault();
        window.print();
    });

    // Init Form Blocks 
    $('input, textarea, select').removeAttr('required');
    $('input, textarea, select').attr("disabled", "disabled");

    $('form, #artifacts').find('button').not('.prf-edit').hide();
    $('#countSoftSkills, #addArtifacts').hide();

    $('#blankUniversity, #blankCourse, #blankLanguage, #blankSoftSkill').find('input, textarea, select').removeAttr("disabled");

    $('#loadFileModal, #loadLinkModal').find('button').show();
    $('#loadFileModal, #loadLinkModal').find('input').removeAttr("disabled");

    // Edit of Forms
    $('form').on('click', '.prf-edit', function () {
        var $form = $(this).parents('form');
        var formName = $form.attr('name');
        console.log('formName= ' + formName);

        $(this).addClass('prf-edit-now');
        if ($(this).hasClass('prf-edit-now')) {
            $form.find('input, textarea, select').removeAttr("disabled");
            $('.prf-edit').attr("disabled", "disabled").not('.prf-edit-now').css('color', '#ccc');
            $form.find('button').not('.prf-edit-now').removeAttr("disabled");
        } else {
            $form.find('input, textarea, select').attr("disabled", "disabled");
        };
        $form.find('button').not('.prf-button-edit').show();

        //all information in inputs
        beginForm = {};

        switch (formName) {
            case 'traineeEducation':
                //all information in inputs in Education form
                formDataEducation(beginForm, $form);
                showHideAddButtons('.prf-add-university', '.prf-university', MAX_EDUCATION);
                showHideAddButtons('.prf-add-course', '.prf-course', MAX_COURSES);
                break;

            case 'traineeSkills':
                //all information in inputs in Language form
                formDataSkills(beginForm, $form);
                showHideAddButtons('.prf-add-soft-skill', '.prf-soft-skill', MAX_SOFTSKILLS);
                showHideMessage('#countSoftSkills', '.prf-soft-skill', MAX_SOFTSKILLS);
                break;

            case 'traineeLanguage':
                //all information in inputs in Language form
                formDataLanguage(beginForm, $form);
                showHideAddButtons('.prf-add-language', '.prf-language', MAX_LANGUAGES);
                break;

            default:
                //all information in inputs by ID              
                getInputs2($form, beginForm);

        };
        console.log('beginForm= ' + JSON.stringify(beginForm));

    });

    //Fileselect trigger
    $(document).on('change', '.btn-file :file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });

    //Fileselect trigger
    $('.btn-file :file').on('fileselect', function (event, numFiles, label) {
        var input = $(this).parents('.input-group').find(':text'),
            log = numFiles > 1 ? numFiles + ' files selected' : label;

        if (input.length) {
            input.val(log);
        } else {
            if (log) alert(log);
        }
    });

    // Set handler for File Input Change
    $('#newFile').change(function () {
        //Work with Image restrictions
        var newFile = this.files;
        if (newFile.length) {
            restrictFile(newFile);
        };
    });

    // Edit of Artifacts
    $('#artifacts').on('click', '.prf-edit', function () {
        var $artifacts = $('#artifacts');
        console.log('Artifacts!!');
        $(this).addClass('prf-edit-now');
        $('.prf-edit').attr("disabled", "disabled").not('.prf-edit-now').css('color', '#ccc');
        $artifacts.find('button').not('.prf-edit-now').removeAttr("disabled");
        $artifacts.find('button').not('.prf-button-edit').not('prf-trash').show();
        $('#addArtifacts').show();

        $('#FilesLinks').removeClass('prf-block-anchor');
    });

    //Set handlers for Modal hide
    $('#loadFileModal').on('hidden.bs.modal', function (e) {
        var $form = $(this).find('form');
        clearInputsForm($form);
        $('.prf-blink').removeClass('prf-blink');
        $('.prf-attention').text('');
        $form.find('.prf-error-box').remove();

        $('#loadFileModal').find('#superForm').attr('name', 'traineeFile').attr('data-file-id', '0');
    });

    $('#confirmationModal').on('hidden.bs.modal', function (e) {
        $('#confirmationModal').off('click.delete', '#confirmation');
    });

    $('#loadLinkModal').on('hidden.bs.modal', function (e) {
        var $form = $(this).find('form');
        clearInputsForm($form);
        $form.find('.prf-error-box').remove();

        $('#loadLinkModal').find('form[name^="traineeLink"]').attr('name', 'traineeLink').attr('data-link-id', '0');
    });

    // Cancel of Forms Editing
    $('form').on('click', '.prf-cansel', function (e) {
        e.preventDefault;
        var $form = $(this).parents('form');
        disabledForm($form);
        var formName = $form.attr('name');

        switch (formName) {
            case 'traineeEducation':
                $form.find('.prf-university').each(function () {
                    console.log($(this).data('university-id'));
                    if ($(this).data('university-id') == 0) {
                        $(this).remove();
                    };
                });
                $form.find('.prf-course').each(function () {
                    console.log($(this).data('course-id'));
                    if ($(this).data('course-id') == 0) {
                        $(this).remove();
                    };
                });
                // Update Education forms from begin by object 
                formUpdateEducation(beginForm);
                break;

            case 'traineeSkills':
                $form.find('.prf-soft-skill').each(function () {
                    console.log($(this).data('soft-skill-id'));
                    if ($(this).data('soft-skill-id') == 0) {
                        $(this).remove();
                    };
                });
                // Update Skills forms from begin by object 
                formUpdateSkills(beginForm);
                break;

            case 'traineeLanguage':
                $form.find('.prf-language').each(function () {
                    console.log($(this).data('language-id'));
                    if ($(this).data('language-id') == 0) {
                        $(this).remove();
                    };
                });
                // Update Language forms from begin by object 
                formUpdateLanguage(beginForm);
                break;

            default:
                // Update forms from begin by ID 
                formUpdate(beginForm);
        }

        $form.find('.prf-error-box').remove();
        $form.find('.prf-attention').hide();

    });

    // Cancel of Artifacts Editing
    $('#artifacts').on('click', '.prf-cansel', function () {
        var $artifacts = $('#artifacts');
        $artifacts.find('.prf-edit-now').removeClass('prf-edit-now').removeAttr("disabled");
        $('.prf-edit').removeAttr("disabled").css('color', '');
        $artifacts.find('button').not('.prf-edit').hide();
        $('#addArtifacts').hide();
        $('#FilesLinks').addClass('prf-block-anchor');
    });

    //Add new University
    $('form[name="traineeEducation"]').on('click', '.prf-add-university', function () {
        cloneShablon('#blankUniversity', '#University');

        $('.prf-time-period').mask("99/9999 – 99/9999");
        $('#University').find('.prf-university').last().find('.prf-time-period').focus();

        showHideAddButtons('.prf-add-university', '.prf-university', MAX_EDUCATION);
    });

    //Add new Course
    $('form[name="traineeEducation"]').on('click', '.prf-add-course', function () {
        cloneShablon('#blankCourse', '#Course');

        $('.prf-time-period').mask("99/9999 – 99/9999");
        $('#Course').find('.prf-course').last().find('.prf-time-period').focus();

        showHideAddButtons('.prf-add-course', '.prf-course', MAX_COURSES);
    });

    //Add new Soft Skill //new
    $('form[name="traineeSkills"]').on('click', '.prf-add-soft-skill', function () {
        cloneShablon('#blankSoftSkill', '#SoftSkills');
        var SoftSkills = $('#SoftSkills').find('.prf-soft-skill');
        SoftSkills.last().find('input').focus();

        showHideAddButtons('.prf-add-soft-skill', '.prf-soft-skill', MAX_SOFTSKILLS);
        showHideMessage('#countSoftSkills', '.prf-soft-skill', MAX_SOFTSKILLS);
    });

    //Add new Language
    $('form[name="traineeLanguage"]').on('click', '.prf-add-language', function () {
        cloneShablon('#blankLanguage', '#Language');
        $('#Language').find('.prf-language').last().find('input').focus();

        showHideAddButtons('.prf-add-language', '.prf-language', MAX_LANGUAGES);
    });

    //Delete University
    $('form[name="traineeEducation"]').on('click', '.prf-delete-university', function () {
        var delElem = $(this).closest('.prf-university');
        var universityId = delElem.data('university-id');
        if (universityId == 0) {
            delElem.remove();
            showHideAddButtons('.prf-add-university', '.prf-university', MAX_EDUCATION);
        } else {
            var deleteInfo = {};
            deleteInfo['profileId'] = profileId;
            deleteInfo['universityId'] = universityId;
            console.log('deleteInfo ' + JSON.stringify(deleteInfo));
            var $formButtons = $(this).closest('form').find('button');
            //Saving...
            var $submitMessage = $(this).closest('form').find('button[type="submit"]').find('span');

            $.ajax({
                url: '/Education/RemoveUniversity',
                method: 'DELETE',
                async: true,
                contentType: 'application/json',
                data: JSON.stringify(deleteInfo),
                success: function () {
                    delElem.remove();
                    var correction = beginForm['universities'];
                    removeObjectFromArrayByKey(correction, 'universityId', universityId);
                    console.log('Delete! beginForm=' + JSON.stringify(beginForm));
                },
                error: function () {
                    alert('Извините на сервере произошла ошибка');
                },
                beforeSend: function () {
                    indicateAjax($formButtons, $submitMessage, 'Удаляется');
                },
                complete: function () {
                    endAjax($formButtons, $submitMessage);
                    showHideAddButtons('.prf-add-university', '.prf-university', MAX_EDUCATION);
                }
            });
        };
    });

    //Delete Course
    $('form[name="traineeEducation"]').on('click', '.prf-delete-course', function () {
        var delElem = $(this).closest('.prf-course');
        var courseId = delElem.data('course-id');
        if (courseId == 0) {
            delElem.remove();
            showHideAddButtons('.prf-add-course', '.prf-course', MAX_COURSES);
        } else {
            var deleteInfo = {};
            deleteInfo['profileId'] = profileId;
            deleteInfo['courseId'] = courseId;
            console.log('deleteInfo ' + JSON.stringify(deleteInfo));
            var $formButtons = $(this).closest('form').find('button');
            //Saving...
            var $submitMessage = $(this).closest('form').find('button[type="submit"]').find('span');

            $.ajax({
                url: '/Education/RemoveCourse',
                method: 'DELETE',
                async: true,
                contentType: 'application/json',
                data: JSON.stringify(deleteInfo),
                success: function () {
                    delElem.remove();
                    var correction = beginForm['courses'];
                    removeObjectFromArrayByKey(correction, 'courseId', courseId);
                    console.log('Delete! beginForm=' + JSON.stringify(beginForm));
                },
                error: function () {
                    alert('Извините на сервере произошла ошибка');
                },
                beforeSend: function () {
                    indicateAjax($formButtons, $submitMessage, 'Удаляется');
                },
                complete: function () {
                    endAjax($formButtons, $submitMessage);
                    showHideAddButtons('.prf-add-course', '.prf-course', MAX_COURSES);
                }
            });
        };
    });

    //Delete Soft Skill
    $('form[name="traineeSkills"]').on('click', '.prf-delete-soft-skill', function () {
        var delElem = $(this).closest('.prf-soft-skill');
        var softSkillId = delElem.data('soft-skill-id');
        if (softSkillId == 0) {
            delElem.remove();
            showHideAddButtons('.prf-add-soft-skill', '.prf-soft-skill', MAX_SOFTSKILLS);
            showHideMessage('#countSoftSkills', '.prf-soft-skill', MAX_SOFTSKILLS);
        } else {
            var deleteInfo = {};
            deleteInfo['profileId'] = profileId;
            deleteInfo['idSoftSkill'] = softSkillId;
            console.log('deleteInfo ' + JSON.stringify(deleteInfo));
            var $formButtons = $(this).closest('form').find('button');
            //Saving...
            var $submitMessage = $(this).closest('form').find('button[type="submit"]').find('span');

            $.ajax({
                url: '/Skill/RemoveSoftSkill',
                method: 'DELETE',
                async: true,
                contentType: 'application/json',
                data: JSON.stringify(deleteInfo),
                success: function () {
                    delElem.remove();
                    var correction = beginForm['SoftSkills'];
                    removeObjectFromArrayByKey(correction, 'idSoftSkill', softSkillId);
                    console.log('Delete! beginForm=' + JSON.stringify(beginForm));
                },
                error: function () {
                    alert('Извините на сервере произошла ошибка');
                },
                beforeSend: function () {
                    indicateAjax($formButtons, $submitMessage, 'Удаляется');
                },
                complete: function () {
                    endAjax($formButtons, $submitMessage);
                    showHideAddButtons('.prf-add-soft-skill', '.prf-soft-skill', MAX_SOFTSKILLS);
                    showHideMessage('#countSoftSkills', '.prf-soft-skill', MAX_SOFTSKILLS);
                }
            });

        };

    });

    //Delete Language
    $('form[name="traineeLanguage"]').on('click', '.prf-delete-language', function () {
        var delElem = $(this).closest('.prf-language');
        var languageId = delElem.data('language-id');
        if (languageId == 0) {
            delElem.remove();
            showHideAddButtons('.prf-add-language', '.prf-language', MAX_LANGUAGES);
        } else {
            var deleteInfo = {};
            deleteInfo['profileId'] = profileId;
            deleteInfo['languageId'] = languageId;
            console.log('deleteInfo ' + JSON.stringify(deleteInfo));
            var $formButtons = $(this).closest('form').find('button');
            //Saving...
            var $submitMessage = $(this).closest('form').find('button[type="submit"]').find('span');

            $.ajax({
                url: '/Language/RemoveLanguage',
                method: 'DELETE',
                async: true,
                contentType: 'application/json',
                data: JSON.stringify(deleteInfo),
                success: function () {
                    delElem.remove();
                    var correction = beginForm['languages'];
                    removeObjectFromArrayByKey(correction, 'languageId', languageId);
                    console.log('Delete! beginForm=' + JSON.stringify(beginForm));
                },
                error: function () {
                    alert('Извините на сервере произошла ошибка');
                },
                beforeSend: function () {
                    indicateAjax($formButtons, $submitMessage, 'Удаляется');
                },
                complete: function () {
                    endAjax($formButtons, $submitMessage);
                    showHideAddButtons('.prf-add-language', '.prf-language', MAX_LANGUAGES);
                }
            });

        };

    });

    //Delete of Artifact
    $('#artifacts').on('click', '.prf-trash', function () {

        var delElem = $(this).closest('.prf-artifact');
        if (delElem.is('.prf-file')) {
            var fileId = delElem.data('file-id');
            var url = '/File/Remove/' + fileId;
            console.log('url= ' + url);
        } else if (delElem.is('.prf-link')) {
            var linkId = delElem.data('link-id');
            var url = '/Link/Remove/' + linkId;
            console.log('url= ' + url);
        };

        $('#confirmationModal').modal('show');

        $('#confirmationModal').on('click.delete', '#confirmation', function () {

            var $formButtons = $('#confirmationModal').find('button');
            var $submitMessage = $('#confirmation').find('span');

            $.ajax({
                url: url,
                method: 'DELETE',
                async: true,
                contentType: 'application/json',
                //data: JSON.stringify(deleteInfo),
                success: function () {
                    delElem.remove();
                    console.log('Delete Artifact!!');
                },
                error: function () {
                    alert('Извините на сервере произошла ошибка');
                },
                beforeSend: function () {
                    indicateAjax($formButtons, $submitMessage, 'Удаляется');
                },
                complete: function () {
                    $('#confirmationModal').modal('hide');
                    endAjax($formButtons, $submitMessage, 'Да');
                }
            });

        });

    });

    //Edit of Artifacts
    $('#artifacts').on('click', '.prf-file', function (e) {
        if ((e.target.className != 'glyphicon glyphicon-trash') && (e.target.className != 'prf-trash btn-link')) {
            e.preventDefault;
            console.log('Editing Artifact!!!');
            var $form = $('#loadFileModal').find('form[name="traineeFile"]');
            $form.attr('name', 'traineeFileEdit');
            $form.attr('data-file-id', $(this).attr('data-file-id'));
            var fileDescription = $(this).find('.prf-signature').text();
            $('#fileDescription').val(fileDescription);
            $('#newFile').val('');
            $('#newFileName').val($(this).attr('data-file-name'));
            $('#loadFileModal').modal('show');
        };
    });

    $('#artifacts').on('click', '.prf-link', function (e) {
        if ((e.target.className != 'glyphicon glyphicon-trash') && (e.target.className != 'prf-trash btn-link')) {
            e.preventDefault;
            console.log('Editing Artifact!!!');
            var $form = $('#loadLinkModal').find('form[name="traineeLink"]');
            $form.attr('name', 'traineeLinkEdit');
            $form.attr('data-link-id', $(this).attr('data-link-id'));
            var linkDescription = $(this).find('.prf-signature').text();
            $('#linkDescription').val(linkDescription);
            $('#newLink').val($(this).attr('data-link'));
            $('#loadLinkModal').modal('show');
        };
    });

    $('form').on('submit', function (e) {
        e.preventDefault;
        var currentForm = {};
        var $allInputs = $(this).find('input, textarea, select');
        var formName = $(this).attr('name');
        var $form = $(this);
        console.log('formName= ' + formName);

        $allInputs.on('keyup', function () {
            $(this).parents('.prf-required').find('.prf-error-box').remove();
        });

        $(this).find('.prf-error-box').remove();

        switch (formName) {
            case 'traineeEducation':
                // Validate inputs 
                $allInputs.not('.prf-not-required').each(function () {
                    var valEl = $(this).val();
                    if (!$.trim(valEl)) {
                        console.log('поле пустое');
                        var $cloneError = $errorMessage.clone();
                        $(this).after($cloneError);
                    };
                });
                break;

            case 'traineeSkills':
                $allInputs.on('click', function () {
                    $(this).parents('.prf-required').find('.prf-error-box').remove();
                });

                // Validate Soft Skills inputs  
                $allInputs.not('input[type="checkbox"]').each(function () {
                    var valEl = $(this).val();
                    if (RV_SOFTSKILL.test(valEl) == false || !$.trim(valEl)) {
                        var $cloneError = $errorMessage.clone();
                        $(this).after($cloneError);
                    };
                });
                break;

            case 'traineeLanguage':
                $allInputs.on('click', function () {
                    $(this).parents('.prf-required').find('.prf-error-box').remove();
                });

                // Validate Language inputs  
                $allInputs.not('select').each(function () {
                    var valEl = $(this).val();
                    if (RV_LANGUAGE.test(valEl) == false || !$.trim(valEl)) {
                        var $cloneError = $errorMessage.clone();
                        $(this).after($cloneError);
                    };
                });

                // Validate Language selects  
                $allInputs.not('input').each(function () {
                    var valEl = $(this).val();
                    if (!$.trim(valEl)) {
                        var $cloneError = $errorMessage.clone();
                        $(this).after($cloneError);
                    };
                });
                break;

            case 'traineeFile':
                $allInputs.on('click', function () {
                    $(this).next('.prf-required').find('.prf-error-box').remove();
                    $(this).closest('.input-group').next('.prf-required').find('.prf-error-box').remove();
                });

                // Validate Inputs for File  
                var valEl = $('#fileDescription').val();
                if (!$.trim(valEl)) {
                    var $cloneError = $errorMessage.clone();
                    $('#fileDescription').append($cloneError);
                    $('#fileDescription').next('.prf-required').append($cloneError);
                };

                valEl = $('#newFile').val();
                if (!$.trim(valEl)) {
                    var $cloneError = $errorMessage.clone();
                    $('#newFile').closest('.input-group').next('.prf-required').append($cloneError);
                    return false;
                };

                var newFile = $('#newFile')[0].files;
                var help = restrictFile(newFile);
                if (help === false) {
                    var $cloneError = $errorMessage.clone();
                    $('#newFile').closest('.input-group').next('.prf-required').append($cloneError);
                };
                break;

            case 'traineeLink':
                $allInputs.on('click', function () {
                    $(this).next('.prf-required').find('.prf-error-box').remove();
                    $(this).closest('.input-group').next('.prf-required').find('.prf-error-box').remove();
                });

                // Validate Inputs for Link  
                var valEl = $('#linkDescription').val();
                if (!$.trim(valEl)) {
                    var $cloneError = $errorMessage.clone();
                    $('#linkDescription').append($cloneError);
                    $('#linkDescription').next('.prf-required').append($cloneError);
                };

                valEl = $('#newLink').val();
                if (RV_URL.test(valEl) == false || !$.trim(valEl)) {
                    var $cloneError = $errorMessage.clone();
                    $('#newLink').next('.prf-required').append($cloneError);
                };
                break;

            case 'traineeFileEdit':
                $allInputs.on('click', function () {
                    $(this).next('.prf-required').find('.prf-error-box').remove();
                    $(this).closest('.input-group').next('.prf-required').find('.prf-error-box').remove();
                });

                // Validate Inputs for File  
                var valEl = $('#fileDescription').val();
                if (!$.trim(valEl)) {
                    var $cloneError = $errorMessage.clone();
                    $('#fileDescription').append($cloneError);
                    $('#fileDescription').next('.prf-required').append($cloneError);
                };

                valEl = $('#newFile').val();
                if ($.trim(valEl)) {
                    var newFile = $('#newFile')[0].files;
                    var help = restrictFile(newFile);
                    if (help === false) {
                        var $cloneError = $errorMessage.clone();
                        $('#newFile').closest('.input-group').next('.prf-required').append($cloneError);
                    }
                };
                break;

            default:

                // Validate unputs by ID 
                $allInputs.not('#Linkedin').each(function () {
                    var valEl = $(this).val();
                    var idEl = $(this).attr('id');

                    switch (idEl) {

                        // E-mail validation
                        case 'Email':
                            if (RV_EMAIL.test(valEl) == false || !$.trim(valEl)) {
                                var $cloneError = $errorMessage.clone();
                                $(this).after($cloneError);
                            };
                            break;

                        // EmploymentDuration validation
                        case 'EmploymentDuration':
                            if (RV_DATE.test(valEl) == false || !$.trim(valEl)) {
                                var $cloneError = $errorMessage.clone();
                                $(this).after($cloneError);
                            };
                            break;

                        // Skype validation
                        case 'Skype':
                            if (RV_SKYPE.test(valEl) == true) {
                                var $cloneError = $errorMessage.clone();
                                $(this).after($cloneError);
                            };
                            break;

                        default:
                            if (!$.trim(valEl)) {
                                console.log('поле пустое');
                                var $cloneError = $errorMessage.clone();
                                $(this).after($cloneError);
                            };
                    };
                });
        };

        console.log('prf-error-box= ' + $form.has('.prf-error-box').length);
        if ($form.has('.prf-error-box').length == true) {
            return false;
        };

        $allInputs.off('keyup');
        $allInputs.off('click');

        console.log($(this));
        console.log('url: ' + $(this).data('action'));

        var $formButtons = $(this).find('button');

        //Saving...
        var $submitMessage = $(this).find('button[type="submit"]').find('span');

        //Sending data and form updating 
        switch (formName) {
            case 'traineeEducation':
                //create output data for Education
                formDataEducation(currentForm, $(this));
                console.log('currentForm Out= ' + JSON.stringify(currentForm));

                $.ajax({
                    url: $(this).data('action'),
                    method: 'POST',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(currentForm),
                    dataType: 'json',
                    success: function (data) {
                        console.log('data= ' + JSON.stringify(data));
                        var newForm = data;

                        if (newForm['profileId'] == profileId) {
                            formNewEducation(newForm);
                            $('.prf-time-period').mask("99/9999 – 99/9999");
                            disabledForm($form);
                        } else {
                            alert('Ошибка со стажером!');
                        };
                    },
                    error: function () {
                        alert('Извините на сервере произошла ошибка');
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        endAjax($formButtons, $submitMessage);
                    }
                });
                break;

            case 'traineeSkills':
                //create output data for Education
                formDataSkills(currentForm, $(this));
                console.log('currentForm Out= ' + JSON.stringify(currentForm));

                $.ajax({
                    url: $(this).data('action'),
                    method: 'POST',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(currentForm),
                    dataType: 'json',
                    success: function (data) {
                        console.log('data= ' + JSON.stringify(data));
                        var newForm = data;

                        if (newForm['profileId'] == profileId) {
                            formNewSkills(newForm);
                            disabledForm($form);
                        } else {
                            alert('Ошибка со стажером!');
                        };
                    },
                    error: function () {
                        alert('Извините на сервере произошла ошибка');
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        endAjax($formButtons, $submitMessage);
                        $form.find('.prf-attention').hide();
                    }
                });
                break;

            case 'traineeLanguage':
                //create output data for Languages
                formDataLanguage(currentForm, $(this));
                console.log('currentForm Out= ' + JSON.stringify(currentForm));

                $.ajax({
                    url: $(this).data('action'),
                    method: 'POST',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(currentForm),
                    dataType: 'json',
                    success: function (data) {
                        console.log('data= ' + JSON.stringify(data));
                        var newForm = data;

                        if (newForm['profileId'] == profileId) {
                            formNewLanguage(newForm);
                            disabledForm($form);
                        } else {
                            alert('Ошибка со стажером!');
                        };
                    },
                    error: function () {
                        alert('Извините на сервере произошла ошибка');
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        endAjax($formButtons, $submitMessage);
                    }
                });
                break;

            case 'traineeFile':
                /// help
                var helpFile = $('#newFile')[0].files;
                console.log('newFile.name= ' + helpFile[0].name);
                console.log('newFile.size= ' + helpFile[0].size);
                console.log('newFile.type= ' + helpFile[0].type);
                ///////////////

                //create output data for File
                var fileData = $('#newFile').prop('files')[0];
                var fileDescription = $('#fileDescription').val();

                var form = $('form[name="traineeFile"]'); // getting FORM from page
                var formData = new FormData(); // creating instanse of FormData

                // filling FormData with data for sending to server
                formData.append("profileId", profileId); // profile to attach file
                formData.append("file", fileData); // adding file to form data
                formData.append("description", fileDescription); // adding description of form data

                $.ajax({
                    url: "/File/Add",
                    type: 'POST',
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        createNewFile(data);
                    },
                    error: function () {
                        alert("error");
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        var $form = $('#loadFileModal').find('form');
                        clearInputsForm($form);
                        endAjax($formButtons, $submitMessage);
                        $('#loadFileModal').modal('hide');
                    }
                });
                break;

            case 'traineeFileEdit':
                /// help
                var helpFile = $('#newFile')[0].files;
                if (helpFile.length) {
                    console.log('newFile.name= ' + helpFile[0].name);
                    console.log('newFile.size= ' + helpFile[0].size);
                    console.log('newFile.type= ' + helpFile[0].type);
                };

                ///////////////

                //create output data for File
                var fileData = $('#newFile').prop('files')[0];
                var fileDescription = $('#fileDescription').val();

                var form = $('form[name="traineeFileEdit"]'); // getting FORM from page
                var formData = new FormData(); // creating instanse of FormData

                console.log('file-id= ' + form.attr('data-file-id'));

                // filling FormData with data for sending to server
                formData.append("profileId", profileId); // profile to attach file
                formData.append("id", form.attr('data-file-id')); // adding file id
                formData.append("file", fileData); // adding file to form data
                formData.append("description", fileDescription); // adding description of form data

                $.ajax({
                    url: "/File/Update",
                    type: 'PUT',
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        var newFileId = '.prf-file[data-file-id="' + data.id + '"]';
                        var changedFile = $(newFileId);
                        changedFile.attr('data-file-name', data.name)
                                  .attr('data-file-size', data.size);
                        changedFile.find('.prf-signature').text(data.description);
                        changeImgFileByType(changedFile);
                    },
                    error: function () {
                        alert("error");
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        var $form = $('#loadFileModal').find('form');
                        clearInputsForm($form);
                        endAjax($formButtons, $submitMessage);
                        $('#loadFileModal').modal('hide');
                    }
                });
                break;

            case 'traineeLink':
                //create output data for Link
                currentForm['profileId'] = profileId;
                currentForm['Url'] = $('#newLink').val();
                currentForm['Description'] = $('#linkDescription').val();
                console.log('currentForm Out= ' + JSON.stringify(currentForm));

                $.ajax({
                    url: "/Link/Add",
                    method: 'POST',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(currentForm),
                    dataType: 'json',
                    success: function (data) {
                        createNewLink(data);
                    },
                    error: function () {
                        alert("error");
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        var $form = $('#loadLinkModal').find('form');
                        clearInputsForm($form);
                        endAjax($formButtons, $submitMessage);
                        $('#loadLinkModal').modal('hide');
                    }
                });
                break;

            case 'traineeLinkEdit':
                //create output data for Link
                currentForm['profileId'] = profileId;
                currentForm['LinkId'] = $form.attr('data-link-id');
                currentForm['Url'] = $('#newLink').val();
                currentForm['Description'] = $('#linkDescription').val();
                console.log('currentForm Out= ' + JSON.stringify(currentForm));

                $.ajax({
                    url: "/Link/Update",
                    type: 'PUT',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(currentForm),
                    dataType: 'json',
                    success: function (data) {
                        console.log('dataLink= ' + JSON.stringify(data));
                        var newLinkId = '.prf-link[data-link-id="' + data.id + '"]';
                        var changedLink = $(newLinkId);
                        changedLink.attr('data-link', data.link);
                        changedLink.find('.prf-signature').text(data.description);
                    },
                    error: function () {
                        alert("error");
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        var $form = $('#loadLinkModal').find('form');
                        clearInputsForm($form);
                        endAjax($formButtons, $submitMessage);
                        $('#loadLinkModal').modal('hide');
                    }
                });
                break;

            default:
                //create output data in object
                currentForm['profileId'] = profileId;
                getInputs2($(this), currentForm);
                console.log('currentForm Out= ' + JSON.stringify(currentForm));

                $.ajax({
                    url: $(this).data('action'),
                    method: 'POST',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(currentForm),
                    dataType: 'json',
                    success: function (data) {
                        console.log('data= ' + JSON.stringify(data));
                        var newForm = data;

                        if (newForm['profileId'] == profileId) {
                            formUpdate(newForm);
                            disabledForm($form);
                        } else {
                            alert('Ошибка со стажером!');
                        };
                    },
                    error: function () {

                        alert('Извините на сервере произошла ошибка');
                        formUpdate(beginForm);
                    },
                    beforeSend: function () {
                        indicateAjax($formButtons, $submitMessage);
                    },
                    complete: function () {
                        endAjax($formButtons, $submitMessage);
                    }
                });
        };
        console.log(JSON.stringify(currentForm)); 
        return false;
    });

    $("#Phone").mask("+999 99 9999999");

    $("#EmploymentDuration").mask("99/9999 – 99/9999");
    $('.prf-time-period').mask("99/9999 – 99/9999");

});