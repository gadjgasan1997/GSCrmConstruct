#pragma checksum "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bb34397051b7a2c24546d9be23d281a3509d8dd5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Employee_Partial_EmployeeContactsList), @"mvc.1.0.view", @"/Views/Employee/Partial/EmployeeContactsList.cshtml")]
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
#nullable restore
#line 1 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
using static GSCrm.CommonConsts;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
using GSCrm.Data.ApplicationInfo;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
using GSCrm.Helpers;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bb34397051b7a2c24546d9be23d281a3509d8dd5", @"/Views/Employee/Partial/EmployeeContactsList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af9277d557923f57a3a7522d76ddc19e27ae9212", @"/Views/_ViewImports.cshtml")]
    public class Views_Employee_Partial_EmployeeContactsList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<EmployeeViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/default-empty.svg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("employeeContactsList"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 7 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
  
    int contactsCount = Model.EmployeeContactViewModels.Count;

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"tab-pane fade\" id=\"contacts\" role=\"tabpanel\" aria-labelledby=\"contact-tab\">\r\n");
#nullable restore
#line 12 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
     if (contactsCount > 0)
    {
        await Html.RenderPartialAsync($"{EMP_VIEWS_REL_PATH}Partial/EmployeeContactsFilter.cshtml");
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <div class=\"form-group\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "bb34397051b7a2c24546d9be23d281a3509d8dd55863", async() => {
                WriteLiteral("\r\n            <div class=\"table-wrapper\">\r\n                <table");
                BeginWriteAttribute("class", " class=\"", 654, "\"", 712, 2);
                WriteAttributeValue("", 662, "fl-table", 662, 8, true);
#nullable restore
#line 20 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
WriteAttributeValue(" ", 670, contactsCount > 0 ? "" : "empty-table", 671, 41, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                    <thead>\r\n");
#nullable restore
#line 22 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                         if (contactsCount > 0)
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <tr>\r\n                                <th>");
#nullable restore
#line 25 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                               Write(resManager.GetString("Type"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                                <th>");
#nullable restore
#line 26 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                               Write(resManager.GetString("PhoneNumber"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                                <th>");
#nullable restore
#line 27 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                               Write(resManager.GetString("Email"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                                <th class=\"action-column\"></th>\r\n                                <th class=\"action-column\"></th>\r\n                            </tr>\r\n");
#nullable restore
#line 31 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <tr><th></th><th></th></tr>\r\n");
#nullable restore
#line 35 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
}

#line default
#line hidden
#nullable disable
                WriteLiteral("                    </thead>\r\n                    <tbody>\r\n");
#nullable restore
#line 38 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                         if (contactsCount == 0)
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <tr>\r\n                                <td>");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "bb34397051b7a2c24546d9be23d281a3509d8dd59153", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 42 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                               Write(resManager.GetString("ESEmployeeContacts"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                            </tr>\r\n");
#nullable restore
#line 44 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 45 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                         foreach (EmployeeContactViewModel contactViewModel in Model.EmployeeContactViewModels)
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <tr>\r\n                                <td class=\"tooltip-cell-src\">");
#nullable restore
#line 48 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                                                        Write(contactViewModel.ContactType);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                <td class=\"tooltip-cell-src\">");
#nullable restore
#line 49 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                                                        Write(contactViewModel.PhoneNumber);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                <td class=\"tooltip-cell-src\">");
#nullable restore
#line 50 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                                                        Write(contactViewModel.Email);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                <td class=\"remove-item-btn\">\r\n                                    <div class=\"remove-item-url\" hidden=\"hidden\">\r\n                                        ");
#nullable restore
#line 53 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                                   Write(Html.ActionLink(contactViewModel.Id.ToString(), "Delete", EMP_CONTACT, new { id = contactViewModel.Id }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"""
                                    </div>
                                    <span class=""icon-bin""></span>
                                </td>
                                <td class=""edit-item-btn"" data-toggle=""modal"" data-target=""#empContactUpdateModal"">
                                    <div class=""edit-item-url"" hidden=""hidden"">
                                        ");
#nullable restore
#line 59 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                                   Write(Html.ActionLink(contactViewModel.Id.ToString(), CONTACT, EMP_CONTACT, new { id = contactViewModel.Id }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n                                    </div>\r\n                                    <span class=\"icon-pencil\"></span>\r\n                                </td>\r\n                            </tr>\r\n");
#nullable restore
#line 64 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
                        }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    </tbody>\r\n                </table>\r\n            </div>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 69 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Employee\Partial\EmployeeContactsList.cshtml"
          
            await Html.RenderPartialAsync("Partial/Navigation", new NavbarRenderSettings
            {
                ItemsCount = contactsCount,
                ViewInfo = viewsInfo.Get(EMP_CONTACTS),
                ControllerName = EMP_CONTACT,
                ActionName = CONTACTS
            });
        

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IViewsInfo viewsInfo { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ResManager resManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<EmployeeViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
