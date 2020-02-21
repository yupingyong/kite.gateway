/// <reference path="config.js" />
//基于jQuery的文件上传插件 
//v0.1.20170302
(function ($) {
 $.FlyUpload = function (config) {
    var cfg = {
        //上传控件Id(需要$("#id")获取的对象)
        id: null,
        //是否自动上传
        auto: true,
        //上传按钮
        upbutton: null,
        //预览事件
        //preview: function (url) {
        //    alert("预览处理");
        //},
        //成功事件
        success: function (url) {
            alert("上传成功,图片地址:" + url);
        },
        //异常事件
        error: function (ex) {
            console.log(ex);
        },
        //文件列表
        filelist: new Array()
    };

    //模拟构造函数
    (function () {
        //初始化配置项
        if (config.id != null && config.id != undefined) {
            cfg.id = config.id;
        }
        //是否自动上传
        if (config.auto != null && config.auto != undefined) {
            cfg.auto = config.auto;
        }
        //上传按钮id
        if (config.upbutton != null && config.upbutton != undefined) {
            cfg.upbutton = config.upbutton;
        }
        //预览事件
        if (config.preview != null && config.preview != undefined) {
            cfg.preview = config.preview;
        }
        //成功事件
        if (config.success != null && config.success != undefined) {
            cfg.success = config.success;
        }
        //异常事件
        if (config.error != null && config.error != undefined) {
            cfg.error = config.error;
        }
        //获取文件上传输入对象
        var input = document.getElementById(cfg.id);
        if (typeof FileReader === 'undefined') {
            alert('当前浏览器不支持H5上传');
            input.setAttribute('disabled', 'disabled');
        }
        else {
            //绑定文件选择事件
            input.addEventListener('change', function () {
                var max = this.files.length;
                if (max > cfg.maxcount) {
                    max = cfg.maxcount;
                }
                //验证文件允许的格式
                for (var i = 0; i < max; i++) {
                    //文件格式不符合的不允许出现在预览中
                    if (!VerificationFormat(input['value'])) {　　//判断上传文件格式
                        console.log("上传的图片格式不正确,请重新选择");
                    }
                    else {
                        if (this.files[i].size <= commonConfig.maxSize) {
                            //文件读取预览
                            var reader = new FileReader();
                            reader.readAsDataURL(this.files[i]);
                            reader.onload = function (event) {
                                //调用外部图片预览事件
                                cfg.preview(this.result);
                            }
                            //加入上传队列
                            cfg.filelist.unshift({
                                name: input['value'],
                                content: this.files[i]
                            });
                        }
                        else {
                            alert("上传的文件不能超过指定大小");
                        }
                    }
                }
                //处理上传事件
                if (cfg.auto) {
                    FileUpload();
                }
                else {
                    //当上传按钮为空对象时则采用自动上传
                    if (cfg.upbutton != null && cfg.upbutton == undefined) {
                        //添加按钮事件
                        document.getElementById(cfg.upbutton).addEventListener('click', function () {
                            FileUpload();
                        }, false);
                    }
                    else {
                        FileUpload();
                    }
                }
            }, false);
        }
    })();
     //文件上传事件(这个项目统一上传到又拍云)
     function FileUpload() {
        var file = null;
        var count = 0;
        while (cfg.filelist.length > 0) {
            file = cfg.filelist.pop();
            //当上传队列中包含文件时上传
            if (file != null && file != undefined) {
                if (commonConfig.ossName == 'local') {
                    //上传到本地服务器
                    var form = new FormData();
                    form.append('file', file.content);
                    form.append('filename', file.name);
                    //上传文件
                    $.ajax({
                        type: 'post',
                        url: commonConfig.serverUrl,
                        data: form,
                        processData: false,
                        contentType: false,
                        success: function (res) {
                            cfg.success(json.Path);
                        },
                        error: function (ex) {
                            cfg.error(ex);
                        }
                    });
                }
                else if (commonConfig.ossName == 'upyun') {
                    //上传到又拍云
                    $.ajax({
                        type: 'post',
                        url: '/Main/UPYun/' + file.name,
                        success: function (result) {
                            if (result.code==0) {
                                var form = new FormData();
                                form.append('file', file.content);
                                form.append('authorization', result.data.signature);
                                form.append('policy', result.data.policy);
                                //上传文件
                                $.ajax({
                                    type: 'post',
                                    url: commonConfig.serverUrl,
                                    data: form,
                                    processData: false,
                                    contentType: false,
                                    success: function (res) {
                                        cfg.success(result.data.path);
                                    },
                                    error: function (ex) {
                                        cfg.error(ex);
                                    }
                                });
                            }
                            else {
                                alert("文件上传失败");
                            }
                        }
                    });
                }
                count++;
            }
        }
        if (count == 0) {
            alert("当前文件队列中无可上传的文件!");
        }
     }
     //文件格式验证
     function VerificationFormat(fileName) {
         var result = false;
         for (var i = 0; i < commonConfig.format.length; i++) {
             if (fileName.indexOf('.'+commonConfig.format[i]) > -1) {
                 result = true;
                 break;
             }
         }
         return result;
     }
 };
})(jQuery);