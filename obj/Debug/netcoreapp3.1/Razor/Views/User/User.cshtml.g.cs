#pragma checksum "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\User\User.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d6f64f0d8aeb2f365de9b2688e256f1bde019621"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_User), @"mvc.1.0.view", @"/Views/User/User.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\_ViewImports.cshtml"
using GSCrm;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\_ViewImports.cshtml"
using GSCrm.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\_ViewImports.cshtml"
using GSCrm.Models.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\_ViewImports.cshtml"
using GSCrm.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d6f64f0d8aeb2f365de9b2688e256f1bde019621", @"/Views/User/User.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af9277d557923f57a3a7522d76ddc19e27ae9212", @"/Views/_ViewImports.cshtml")]
    public class Views_User_User : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<UserViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\User\User.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div id=\"userAccountForm\">\r\n    <div class=\"row\">\r\n        <div class=\"col\">\r\n            <div class=\"user-avatar\">\r\n                <img");
            BeginWriteAttribute("src", " src=\"", 191, "\"", 214, 1);
#nullable restore
#line 9 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\User\User.cshtml"
WriteAttributeValue("", 197, Model.AvatarPath, 197, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                <span class=\"icon-search\"></span>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>  ");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<UserViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
