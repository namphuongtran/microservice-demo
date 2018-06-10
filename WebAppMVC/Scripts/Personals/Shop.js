var cm = new CommonJS.Common();

var pageJS = {};

(function (context) {
    context.ItemShop = function () {
        var productCategoryId = 0;
        var pageIndex = 0;
        var curBox;
        var isRemove = false;

        function initialize(params) {
            getLoadContent(0);

            $(params.ddlListProductCategory).change(function () {
                productCategoryId = $(this).val() == "" ? 0 : $(this).val();
                pageIndex = 0;

                getLoadContent(productCategoryId);
            });

            $(params.btnShopCart).click(function () {
                isRemove = false;

                var options = {
                    title: 'Giỏ hàng của bạn',
                    Url: params.urlShopCart,
                    width: 800,
                    buttons: [
                        {
                            text: 'Thanh toán',
                            class: 'btn btn-submit',
                            eventHandle: function () {
                                $(params.addForm).submit();
                                closeDialog();
                            }
                        },
                        {
                            text: 'Đóng',
                            class: 'btn btn-cancel',
                            eventHandle: function () {
                                $('#dialogPopup').modal('hide');
                                if (isRemove) {
                                    closeDialog();
                                }
                            }
                        }
                    ]
                };

                cm.ShowDialog(options);
            });

            $(params.btnHistory).click(function () {
                var options = {
                    title: 'Lịch sử thanh toán của bạn',
                    Url: params.urlViewHistory,
                    width: 800,
                    buttons: [
                        {
                            text: 'Đóng',
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

        function initializeContent() {
            $(params.btnPaging).click(function () {
                pageIndex = $(this).data("value");

                getLoadContent(productCategoryId, pageIndex);
            });

            $(params.btnShopAdd).click(function () {
                var organizationId = $(this).data("organizationid");
                var productId = $(this).data("productid");
                var name = $(this).data("name");
                var price = $(this).data("price");
                curBox = $(this).parent("p");

                var options = {
                    type: 'GET',
                    url: params.urlAddShop,
                    data: {
                        OrganizationId: organizationId,
                        ProductId: productId,
                        Name: name,
                        Quantity: 1,
                        Price: price
                    },
                    success: function (data) {
                        $(params.lblCountCart).text(data);
                        curBox.html('<button type="button" class="btn btn-default btn-sm"><span class="glyphicon glyphicon-ok"></span> Đã mua </button>');
                    }
                };

                cm.AjaxWrapper(options);
            });
        }

        function initializeCart() {

            $(params.txtQuantity).focusout(function () {
                var productId = $(this).data("productid");
                var quantity = $(this).val();

                var options = {
                    type: 'GET',
                    url: params.urlEditQuantity,
                    data: {
                        productId: productId,
                        quantity: quantity
                    },
                    success: function () {
                    }
                };

                cm.AjaxWrapper(options);
            });

            $(params.btnRemove).click(function () {
                var productId = $(this).data("productid");

                var options = {
                    type: 'GET',
                    url: params.urlRemoveItem,
                    data: {
                        productId: productId
                    },
                    success: function (data) {
                        $("#addNewForm").html(data);
                        isRemove = true;
                    }
                };

                cm.AjaxWrapper(options);
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
            InitializeContent: initializeContent,
            InitializeCart: initializeCart
        }
    }
})(pageJS);