using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Routing;

using Kite.Gateway.Admin.Core;
using Kite.Gateway.Application.Contracts.Dtos.Administrator;
using Microsoft.AspNetCore.Components;


namespace Kite.Gateway.Admin.Shared;

/// <summary>
/// 
/// </summary>
public sealed partial class MainLayout
{
    private bool UseTabSet { get; set; } = true;

    private string Theme { get; set; } = "";

    private bool IsFixedHeader { get; set; } = true;

    private bool IsFixedFooter { get; set; } = true;

    private bool IsFullSide { get; set; } = true;

    private bool ShowFooter { get; set; } = false;

    private List<MenuItem>? Menus { get; set; }

    [Inject]
    private AuthorizationServerStorage ServerStorage { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private AdministratorDto Administrator { get; set; }
    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        //如果没有登录则跳转到登录页面
        if (ServerStorage.IsLogin())
        {
            Administrator = ServerStorage.GetServerStorage();
        }
        else
        {
            //
            Administrator = new AdministratorDto() 
            {
                AdminName="未登录",
                NickName="未登录",
                Id=0
            };
        }
        Menus = GetIconSideMenuItems();
    }

    private static List<MenuItem> GetIconSideMenuItems()
    {
        var menus = new List<MenuItem>
            {
               new MenuItem()
                        {
                            Text = "首页",
                            Icon = "fa fa-home",
                            Url = "/",
                            IsActive = true
                        },
               new MenuItem()
               {
                   Text ="系统设置",
                   Icon ="fa fa-gear",
                   Url = "",
                   Id = "1",
                   Items = new List<MenuItem>()
                   {
                       
                       new MenuItem()
                        {
                            Text = "账号管理",
                            Icon = "fa fa-user",
                            Url = "/Administrator"
                        },
                        new MenuItem()
                        {
                            Text = "节点管理",
                            Icon = "fa fa-chain",
                            Url = "/Node"
                        }
                   }
               },         
               new MenuItem()
               {
                   Text="网关配置",
                   Icon = "fa fa-fw fa-database",
                   Items=new List<MenuItem>()
                   {
                       new MenuItem() { Text = "服务治理配置", Icon = "fa fa-cogs", Url = "/ServiceGovernance" },
                       new MenuItem() { Text = "身份认证配置(Jwt)", Icon = "fas fa-user-secret", Url = "/Authentication" },
                       new MenuItem() { Text = "白名单管理", Icon = "fas fa-paper-plane", Url = "/WhiteList" },
                       new MenuItem() { Text = "路由管理", Icon = "fa fa-external-link", Url = "/Route" },
                       new MenuItem(){ Text="中间件管理", Icon="fa fa-plug",Url="/Middleware"}
                   }
               } 
            };

        return menus;
    }
}
