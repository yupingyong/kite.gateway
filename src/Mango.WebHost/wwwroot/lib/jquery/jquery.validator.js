//验证插件开始
(function ($) {
    $.Validator = function (config) {
        //创建验证规则
        (function () {
            //这个是失去焦点时启动验证
            for (var i = 0; i < config.items.length; i++) {
                $(config.items[i].id).attr("data-validate", i);
                //$(config.items[i].id).blur(function () {
                //    Start(parseInt($(this).attr("data-validate")));
                //});
            }
        })();
        //创建验证并且获取验证结果
        $.Validator.Create = function () {
            var r = [], result = false;
            for (var i = 0; i < config.items.length; i++) {
                //r.push({
                //    tf: Start(parseInt($(config.items[i].id).attr("data-validate")))
                //});
                result = Start(parseInt($(config.items[i].id).attr("data-validate")));
                if (!result) {
                    break;
                }
            }
            //var count = 0;
            //for (var j = 0; j < r.length; j++) {
            //    if (r[j].tf == false) {
            //        count += 1;
            //    }
            //}
            //if (count == 0)
            //    result = true
            
            return result;
        };
        //启动验证
        var Start = function (index) {
            var item = config.items[index];//获取验证项
            var result = true, tip,tipid;
            var value = $(item.id).val();
            for (var name in item){
                if (name == 'required') {
                    result = ValidatorRequired(value);//验证是否为必填
                    tip = "请输入该项内容"
                }
                else if (name == 'question') {
                    //是否包含问号
                    var last = value.substr(value.length - 1, 1);
                    if (last == "?" || last == "？") {
                        result = true;
                    }
                    else {
                        result = false;
                    }
                    tip = "请输入指定的内容";
                }
                else if (name == 'number') {
                    result = ValidatorIsNaN(value);//验证是否为数字
                    tip = "请输入正确的数字";
                }
                else if (name == 'email') {
                    result = ValidatorEmail(value);//验证是否为电子邮箱
                    tip = "请输入正确的电子邮箱";
                }
                else if (name == 'tel') {
                    result = ValidatorTel(value);//验证电话号码
                    tip = "请输入正确的电话号码";
                }
                else if (name == 'phone') {
                    result = ValidatorPhone(value);//验证手机号码
                    tip = "请输入正确的手机号码";
                }
                else if (name=="length") {
                    result = ValidatorLength(value, item[name].min, item[name].max);//验证范围
                    tip = "请输入"+item[name].min+"到"+item[name].max+"之间的内容长度";
                }
                else if (name=="compare") {
                    result = ValidatorCompare(value, $("#" + item[name].id).val());//验证两个值是否相等
                    tip = "请输入两次相同的内容";
                }
                else if (name == "regular") {
                    result = ValidatorRegExp(value, item[name].rtext);
                    tip = "请输入正确的内容";
                }
                else if (name == "custom") {
                    result = item[name].result;
                    tip = "请输入与该项要求符合的内容";
                }
                
                if (!result) {
                    if ((item[name].tip != undefined && item[name].tip != null && item[name].tip != "")){
                        tip = item[name].tip;
                    }
                    if ((item[name].tipid != undefined && item[name].tipid != null && item[name].tipid != "")) {
                        tipid = item[name].tipid;
                    }
                    Tips(item.id,tip, tipid, false);
                    break;
                }
            }
            if(result){
                //Tips($(vobject),vjson[vname][0].bymsg,true);
                //Tips($(item.id), "<img src='/tools/jquery/1.png' alt=''/>", tipid, true);
            }
            return result;
        };
        return $.Validator;
    };
    

    //根据正则验证
    function ValidatorRegExp(value, regExp) {
        var r=regExp;
        if (!r.test(value)) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证两个文本值是否相同
    function ValidatorCompare(value, comparevalue) {
        if (value != comparevalue) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证字符长度的范围
    function ValidatorLength(value, min,max) {
        if (value.length < min || value.length > max) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证电话号码(如010-123456789)
    function ValidatorTel(value) {
        var regExp = /^\d{3,4}-\d{7,8}(-\d{3,4})?$/;
        if (!regExp.test(value)) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证手机号
    function ValidatorPhone(value) {
        var regExp = /^(((13[0-9]{1})|(15[0-9]{1})|(16[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
        if (!regExp.test(value)) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证邮箱
    function ValidatorEmail(value) {
        var regExp = /^[A-Za-z\d]+([-_\.\+]*[A-Za-z\d]+)*@(([A-Za-z\d]-?){0,62}[A-Za-z\d]\.)+[A-Za-z\d]{2,6}$/;
        if (!regExp.test(value)) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证数字
    function ValidatorIsNaN(value) {
        if (isNaN(value)) {
            return false;
        }
        else {
            return true;
        }
    }
    //验证是否为空
    function ValidatorRequired(value) {
        if (value == "") {
            return false;
        } else {
            return true;
        }
    }
    //输出提示信息
    function Tips(obj, tip, tipid, tf) {
        layer.msg(tip);
    }
    function Trim(str)    
    {    
        return str.replace(/(^\s*)|(\s*$)/g, "");    
    }  
})(jQuery);
