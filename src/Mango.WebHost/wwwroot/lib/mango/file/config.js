//通用配置信息
var commonConfig = {
    //文件存储名字,目前支持 upyun(又拍云存储),local(本地存储)
    ossName: 'upyun'
    //服务器上传地址
    , serverUrl: 'https://v0.api.upyun.com/51core'
    //文件访问根慕名
    , domainName: 'https://file.51core.net'
    //"最大文件上传数"
    , maxCount: 0 
    //允许的文件格式
    , format:['bmp','png','jpg','jpeg','gif']
    //允许上传的文件大小
    ,maxSize:1024*1024*10
};