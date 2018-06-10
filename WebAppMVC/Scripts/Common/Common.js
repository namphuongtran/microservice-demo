var CommonJS = {};

(function (context) {
    context.Common = function () {
        var builderDialog = '';

        function createButton(buttons) {
            $.each(buttons, function (index, value) {
                var button = $('<button type="button" class="' + value.class + '">' + value.text + '</button>')
                    .click(value.eventHandle);

                $('.modal-footer').append(button);
            });
        }

        function showDialog(params) {
            $('.modal-content').html('');
            builderDialog = '';

            var width = $(window).width() - 50;

            var style = 'dynamic';

            if (params.style != "undifine") {
                style = params.style;
            }

            if (params.width != "undifine") {
                width = params.width;
            }

            if (params.height != "undifine") {
                height = params.height;
            }

            builderDialog = builderDialog + '<div class="modal-header form-login">'
                + '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
                + '<h4 class="modal-title">' + params.title + '</h4>'
                + '</div>';
            // updating height for dialog
            if (params.height != "undifine") {
                builderDialog = builderDialog + '<form class="form-horizontal"><div class="modal-body" style="min-height:' + params.height + 'px !important;"></div></form>';
            } else {
                builderDialog = builderDialog + '<form class="form-horizontal"><div class="modal-body"></div></form>';
            }

            builderDialog = builderDialog + '<div class="modal-footer form-login"></div></div>';

            $('.modal-content').append(builderDialog);

            $("#dialogPopup").find('.modal-dialog').css({ width: width + 'px' });
            
            if (style !== 'static') {
                var options;
                if (params.data == "undifine") {

                    options = {
                        url: params.Url,
                        type: "GET",
                        success: function (result) {
                            $('.modal-body').html(result);
                        }
                    };
                }
                else {
                    options = {
                        url: params.Url,
                        type: "GET",
                        data: params.data,
                        success: function (result) {
                            $('.modal-body').html(result);
                        }
                    };
                }

                AjaxWrapper(options);
            }
            else {
                $('.modal-body').html(params.Content);
            }

            createButton(params.buttons);

            $('#dialogPopup').modal('show');

        }

        function AjaxWrapper(options) {
            $("#btnSearchForm").removeClass("active");
            $("#bg-mobile").css("display", "none");
            $.ajax(options);
        }

        function dialogWarning(message) {
            var options = {
                style: 'static',
                title: 'Hộp thoại cảnh cáo',
                width: 450,
                Content: "<h5 class=\"warning\"><div class=\"text\"><span>" + message + "</span></div></h5>",
                buttons: [{
                    text: 'Thoát',
                    class: 'btn btn-cancel',
                    eventHandle: function () {
                        $('#dialogPopup').modal('hide');
                    }
                }]
            };
            cm.ShowDialog(options);
        }

        function dialogWaiting(message) {
            var options = {
                style: 'static',
                title: 'Hộp thoại thông báo',
                width: 450,
                Content: "<h5 class=\"waiting\"><div class=\"text\"><span>" + message + "</span></div></h5>",
                buttons: [{
                    text: 'Thoát',
                    class: 'btn btn-cancel',
                    eventHandle: function () {
                        $('#dialogPopup').modal('hide');
                    }
                }]
            };
            cm.ShowDialog(options);
        }

        function dialogSuccess(message) {
            var options = {
                style: 'static',
                title: 'Hộp thoại thông báo',
                width: 450,
                Content: "<h5 class=\"success\"><div class=\"text\"><span>" + message + "</span></div></h5>",
                buttons: [{
                    text: 'Thoát',
                    class: 'btn btn-cancel',
                    eventHandle: function () {
                        $('#dialogPopup').modal('hide');
                    }
                }]
            };
            cm.ShowDialog(options);
        }

        return {
            DialogWarning: dialogWarning,
            DialogWaiting: dialogWaiting,
            DialogSuccess: dialogSuccess,
            ShowDialog: showDialog,
            AjaxWrapper: AjaxWrapper
        }
    }

})(CommonJS);

function showWaitingProcessing(isShow) {
    var pleaseWaitDiv = $('#pleaseWaitDialog');
    if (isShow)
        pleaseWaitDiv.modal('show');
    else
        pleaseWaitDiv.modal('hide');
}

function reinitForm(formId) {
    var form = $(formId);
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(formId);
}

function closeDialog() {
    if (!$('.validation-summary-errors').length > 0) {
        $('#dialogPopup').modal('hide');
        location.reload(true);
    }
}

function closeDialogNotReload() {
    if (!$('.validation-summary-errors').length > 0) {
        $('#dialogPopup').modal('hide');
        $(".modal-backdrop").css("display", "none");
    }
}

function closeDialogNotReloadWithMessage(messageDisplay) {
    if (!$('.validation-summary-errors').length > 0) {
        alert(messageDisplay);
        $('#dialogPopup').modal('hide');
        $(".modal-backdrop").css("display", "none");
    }
}

$(document).ajaxSend(function (event, request, settings) {
    $("#bg-load").removeClass("hide");
});
$(document).ajaxComplete(function (event, request, settings) {
    $("#bg-load").addClass("hide");
});

//$(document).ready(function () {
//    $('.nav.navbar-nav li a').click(function () {
//        $('.nav.navbar-nav li a').removeClass("actived");
//        $(this).addClass("actived");
//    });
//});