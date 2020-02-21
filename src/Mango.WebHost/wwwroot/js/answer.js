//提交回答
$("#btn_PostAnswer").click(function () {
    $("#contents").val(tinymce.get('contents').getContent());
    //加载验证
    var config = new Array();
    config.push({
        id: $("#contents"),
        required: {},
        length: { min: 1, max: 20000 }
    });
    var v = $.Validator({
        items: config
    });
    if (v.Create()) {
        var tipIndex = layer.load(2);
        $("#AnswerForm").ajaxSubmit({
            success: function (result) {
                layer.close(tipIndex);
                if (result) {
                    layer.msg("发布成功", function () {
                        window.location.href = window.location.href;
                    });
                }
            }
        });
    }
    else {
        return false;
    }
});

//定义一个全部变量
var cmtCinfig = new Array();
function ShowAnswerList(answerId) {
    //判断是否第一次点击评论列表
    //当不是第一次加载时 则判断该评论是否为显示状态
    var cfg = cmtCinfig['key_' + answerId];
    if (cfg == null || cfg == undefined) {

        //并且将信息存储到字典中
        cmtCinfig['key_' + answerId] = {
            p: 1,
            show: true,
            total: 0
        };
        $("#showCommentsPageList_" + answerId).show();
        //第一次则加载数据
        LoadAnswerComments(answerId, 1);
    }
    else {
        if (cfg.show) {
            $("#showCommentsPageList_" + answerId).hide();
            cfg.show = false;
        }
        else {
            $("#showCommentsPageList_" + answerId).show();
            cfg.show = true;
        }
    }
}
//添加评论
function AddAnswerComment(answerId) {
    //加载验证
    var config = new Array();
    config.push({
        id: $("#AnswerCommentsContents_" + answerId),
        required: {tip:'请输入评论内容'},
        length: { min: 1, max: 500 }
    });
    var v = $.Validator({
        items: config
    });
    if (v.Create()) {
        var tipIndex = layer.load(2);
        $("#commentCommentsForm_" + answerId).ajaxSubmit({
            type: 'post',
            url: '/Posts/AddAnswerComments',
            success: function (result) {
                layer.close(tipIndex);
                if (result) {
                    var tip = new $.zui.Messager('评论成功', {
                        icon: 'ok-sign',
                        type: 'success',
                        placement: 'center', // 定义显示位置
                        time: 0
                    }).show();
                    setTimeout(function () {
                        tip.hide();
                    }, 1500);
                    //
                    $("#commentCommentsForm_" + answerId).resetForm(); //提交后重置表单

                    $("#commentCommentsForm_" + answerId).find("textarea[name='contents']").attr("placeholder", "撰写评论...");
                    $("#commentCommentsForm_" + answerId).find("input[name='toUserId']").val(0);
                    //加载最后一页数据
                    //加载分页总记录
                    $.ajax({
                        type: 'get',
                        url: '/Posts/LoadCommentsByTotal',
                        data: 'answerId=' + answerId,
                        success: function (result) {
                            var totalCount = parseInt(result);
                            var pageIndex = 1;
                            //计算总页码
                            var PageSize = 6;
                            //得到总页码数
                            if (totalCount % PageSize == 0) {
                                pageIndex = totalCount / PageSize;
                            }
                            else {
                                pageIndex = parseInt(totalCount / PageSize) + 1;
                            }
                            //将总记录条数存储到字典中
                            cmtCinfig['key_' + answerId].total = totalCount;
                            LoadAnswerComments(answerId, pageIndex);
                        }
                    });
                }
            }
        });
    }
    else {
        return false;
    }
    

}
//加载评论回复数据
function LoadAnswerComments(answerId, pageIndex) {
    //加载数据前清空原有数据记录
    $("#showCommentsPageList_" + answerId).find('.comment').remove();
    $.ajax({
        type: 'get',
        url: '/Posts/LoadComments',
        data: 'answerId=' + answerId + '&pageIndex=' + pageIndex,
        success: function (result) {
            console.log(result);
            var res = JSON.parse(result);
            $.each(res, function (i) {
                CreateCommentsHtml(res[i]);
            });
            if (cmtCinfig['key_' + answerId].total == 0) {
                //加载分页总记录
                $.ajax({
                    type: 'get',
                    url: '/Posts/LoadCommentsByTotal',
                    data: 'answerId=' + answerId,
                    success: function (result) {
                        var totalCount = parseInt(result);
                        //创建分页标签
                        CreatePageHtml(answerId, totalCount, pageIndex);
                        //将总记录条数存储到字典中
                        cmtCinfig['key_' + answerId].total = totalCount;
                    }
                });
            }
            else {
                //创建分页标签
                CreatePageHtml(answerId, cmtCinfig['key_' + answerId].total, pageIndex);
            }
        }
    });
}
//分页处理
function CreatePageHtml(answerId, totalCount, pageIndex) {
    //计算总页码
    var PageCount = 0, PageSize = 6;
    //得到总页码数
    if (totalCount % PageSize == 0) {
        PageCount = totalCount / PageSize;
    }
    else {
        PageCount = parseInt(totalCount / PageSize) + 1;
    }
    //中间页码计算
    var BeginCount = 1;//开始页码
    if (pageIndex > 6) {
        BeginCount = pageIndex - 6;
    }
    var EndCount = PageCount;
    if (PageCount - pageIndex > 6) {
        EndCount = pageIndex + 6;
    }
    var url = '';
    var pageHtml ='<ul class="pager" id="showCommentsPage_' + answerId + '">';
    for (var i = BeginCount; i <= EndCount; i++) {
        if (pageIndex == i) {
            pageHtml += '<li><span style="font-weight:Bold;color:red;">' + i + '</span></li>';
        }
        else {
            pageHtml += '<li><a href="javascript:LoadAnswerComments(' + answerId + ',' + i + ')">' + i + '</a></li>';
        }
    }
    pageHtml += '</ul>';
    //清空原有分页记录
    $("#showCommentsPage_" + answerId).remove();
    //重新填充分页信息
    $("#showCommentsPageList_" + answerId).append(pageHtml);
}
//创建回复Html
function CreateCommentsHtml(d) {
    var html = '<div class="comment">';
   
    html += '<div class="content">';
    html += '<div class="pull-right text-muted">' + d.PostDate + '</div>';
    html += '<div><img src="' + d.HeadUrl + '" width="25px" height="25px" class="img-rounded" /><strong style="margin-left:10px;">' + d.NickName + '</strong>' + (d.ToUserId != 0 ? '<span class="text-muted" style="margin-left:5px;">回复</span> <strong>' + d.ToUserNickName + '</strong>' : '') + '</div>';
    html += '<div class="text">' + (d.IsShow ? d.Contents : '<div class="text bg-info">该内容已经失联,我们在火星发现了Ta的踪迹...</div>') + '</div>';
    html += '<div class="actions">';
    html += '<button onclick="AddCommentsPlus(' + d.CommentId + ');"  class="btn btn-link" style="padding:1px;color:#808080;"><i class="icon icon-thumbs-up"></i><span id="commentsPlus_'+d.CommentId+'">' + d.Plus + '<span></button>';
    html += '<button data-cmd="comment" data-user="' + d.UserId + '" data-name="' + d.NickName + '"  class="btn btn-link" style="margin-left:10px;padding:1px;color:#808080;"><i class="icon icon-reply"></i>回复</button>';
    html += '</div>';
    html += '</div>';
    html += '</div >';
    html = $(html);
    //评论回复处理
    $(html).find("button[data-cmd='comment']").click(function () {
        if (IsLogin) {
            var uid = $(this).attr("data-user");
            var objForm = $("#commentCommentsForm_" + d.AnswerId);
            $(objForm).find("input[name='toUserId']").val(uid);
            $(objForm).find("textarea[name='contents']").attr("placeholder", "回复[" + $(this).attr('data-name') + "]:");
        }
        else {
            var tip = new $.zui.Messager('请您先登录.', {
                icon: 'ok-sign',
                type: 'success',
                placement: 'center', // 定义显示位置
                time: 0
            }).show();
            setTimeout(function () {
                tip.hide(function () {

                });
            }, 1500);
        }
    });
    $("#showCommentsPageList_" + d.AnswerId).append(html);
}


