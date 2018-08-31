
function formCheck(){
    $("#addForm").validate({
        errorPlacement: function(error, element) {
            // Append error affter input field
            $( element )
                .closest( "form" )
                .find( "label[for='" + element.attr( "id" ) + "']").addClass("error").parent().parent()
                .after( error );

            $( element )
                .closest( "form" )
                .find( "label[for='" + element.attr( "id" ) + "']").addClass("error").parent()
                .siblings(".form-content .weui-cell__ft").addClass("weui-cell_warn").show();
        },
        errorElement: "div",
        rules: {
            name: "required",
            phone: {
                required: true,
                phone_number:true
            },
            papers: {
                required: true,
                isIdCardNo:true
            },
            pass:{
                required:true,
                phone:true
            }
        },
        messages: {
            name: "*请输入您的名字",
            phone: {
                required: "*请输入手机号",
                phone_number:"*请输入正确的手机号码"
            },
            papers: {
                required: "*请输入证件号",
                isIdCardNo:"*请正确输入您的身份证号"
            },
            pass:{
                required:"请输入信息",
                phone:"请输入正确"
            }
        }
    });

  // addEventListener
    $("#addForm input").keydown(function(){

        $(this).parent().parent().find(".weui-label").removeClass("error");
        $(this).parent().parent().find(".weui-cell__ft").hide();
    });
}
