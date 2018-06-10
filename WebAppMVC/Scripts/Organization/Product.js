var cm = new CommonJS.Common();

var pageJS = {};

(function (context) {
    context.ItemProduct = function () {
        var productCategoryId = 0;
        var pageIndex = 0;

        function initialize(params) {
            getLoadContent(0);

            $(params.btnAdd).click(function () {
                productCategoryId = $(params.ddlListProductCategory).val() == "" ? 0 : $(params.ddlListProductCategory).val();

                if (productCategoryId <= 0) {
                    cm.DialogWarning("Bạn cần chọn mục sản phẩm!");
                    return;
                }

                var options = {
                    title: 'Thêm sản phẩm',
                    Url: params.urlAddProduct,
                    width: 600,
                    data: {
                        productCategoryId: productCategoryId
                    },
                    buttons: [
                        {
                            text: 'Thêm mới',
                            class: 'btn btn-submit',
                            eventHandle: function () {
                                $(params.addForm).submit();
                            }
                        },
                        {
                            text: 'Hủy bỏ',
                            class: 'btn btn-cancel',
                            eventHandle: function () {
                                $('#dialogPopup').modal('hide');
                            }
                        }
                    ]
                };

                cm.ShowDialog(options);
            });

            $(params.ddlListProductCategory).change(function () {
                productCategoryId = $(this).val() == "" ? 0 : $(this).val();
                pageIndex = 0;

                getLoadContent(productCategoryId);
            });
        }

        function initializeContent() {
            $(params.btnPaging).click(function () {
                pageIndex = $(this).data("value");

                getLoadContent(productCategoryId, pageIndex);
            });

            $(params.btnEdit).click(function () {
                var productId = $(this).data("value");

                var options = {
                    title: 'Sửa sản phẩm',
                    Url: params.urlEditProduct,
                    width: 600,
                    data: {
                        productId: productId
                    },
                    buttons: [
                        {
                            text: 'Hoàn thành',
                            class: 'btn btn-submit',
                            eventHandle: function () {
                                $(params.addForm).submit();
                            }
                        },
                        {
                            text: 'Hủy bỏ',
                            class: 'btn btn-cancel',
                            eventHandle: function () {
                                $('#dialogPopup').modal('hide');
                            }
                        }
                    ]
                };

                cm.ShowDialog(options);
            });

            $(params.btnDelete).click(function () {
                var productId = $(this).data("value");

                var options = {
                    style: 'static',
                    title: "Xóa dữ liệu",
                    width: 450,
                    Content: "<h5 class=\"delete\"><div class=\"text\"><span>Bạn thực sự muốn xóa bản ghi này?</span></div></h5>",
                    buttons: [{
                        text: "Đồng ý",
                        class: 'btn btn-submit',
                        eventHandle: function () {
                            var options2 = {
                                type: 'GET',
                                url: params.urlDeleteProduct,
                                data: {
                                    productId: productId
                                },
                                success: function (data) {
                                    createdSuccess();
                                }
                            };

                            cm.AjaxWrapper(options2);
                        }
                    },
                    {
                        text: "Hủy",
                        class: 'btn btn-cancel',
                        eventHandle: function () {
                            $('#dialogPopup').modal('hide');
                        }
                    }]
                };

                cm.ShowDialog(options);
            });

            $(params.btnUpdatePrice).click(function () {
                var productId = $(this).data("value");

                var options = {
                    title: 'Cập nhật giá',
                    Url: params.urlUpdatePrice,
                    width: 600,
                    data: {
                        productId: productId,
                        productCategoryId: productCategoryId
                    },
                    buttons: [
                        {
                            text: 'Cập nhật',
                            class: 'btn btn-submit',
                            eventHandle: function () {
                                $(params.addForm).submit();
                            }
                        },
                        {
                            text: 'Hủy bỏ',
                            class: 'btn btn-cancel',
                            eventHandle: function () {
                                $('#dialogPopup').modal('hide');
                            }
                        }
                    ]
                };

                cm.ShowDialog(options);
            });
        }

        function getLoadContent(productCategoryId) {
            var options = {
                type: 'GET',
                url: params.urlLoadContent,
                data: {
                    productCategoryId: productCategoryId,
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
            getLoadContent(productCategoryId);
        }

        return {
            CreatedSuccess: createdSuccess,
            Initialize: initialize,
            InitializeContent: initializeContent
        }
    }
})(pageJS);