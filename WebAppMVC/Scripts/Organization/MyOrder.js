var cm = new CommonJS.Common();

var pageJS = {};

(function (context) {
    context.ItemMyOrder = function () {
        var pageIndex = 0;

        function initialize(params) {
            getLoadContent();
        }

        function initializeContent() {
            $(params.btnPaging).click(function () {
                pageIndex = $(this).data("value");

                getLoadContent();
            });
        }

        function getLoadContent() {
            var options = {
                type: 'GET',
                url: params.urlLoadContent,
                data: {
                    pageIndex: pageIndex
                },
                success: function (data) {
                    $("#ResultContent").html(data);
                }
            };

            cm.AjaxWrapper(options);
        }

        function createdSuccess() {
            closeDialogNotReload();
            getLoadContent();
        }

        return {
            CreatedSuccess: createdSuccess,
            Initialize: initialize,
            InitializeContent: initializeContent
        }
    }
})(pageJS);