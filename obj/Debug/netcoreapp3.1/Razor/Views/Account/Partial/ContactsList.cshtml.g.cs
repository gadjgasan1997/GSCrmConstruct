#pragma checksum "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aaf4b72831a3c4335443746b292a1ec76130ea87"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_Partial_ContactsList), @"mvc.1.0.view", @"/Views/Account/Partial/ContactsList.cshtml")]
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
#line 4 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\_ViewImports.cshtml"
using GSCrm.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
using GSCrm.Data.ApplicationInfo;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
using GSCrm.Models.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
using static GSCrm.CommonConsts;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aaf4b72831a3c4335443746b292a1ec76130ea87", @"/Views/Account/Partial/ContactsList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af9277d557923f57a3a7522d76ddc19e27ae9212", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_Partial_ContactsList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AccountViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("row justify-content-end m-auto"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ChangePrimaryContact", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("hidden", new global::Microsoft.AspNetCore.Html.HtmlString("hidden"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/default-empty.svg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("table-wrapper"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 7 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
  
    ViewInfo accContactsView = viewsInfo.Get(ACC_CONTACTS);
    int contactsCount = Model.AccountContacts.Count;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"changePrimaryContact\" class=\"mb-3\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "aaf4b72831a3c4335443746b292a1ec76130ea876684", async() => {
                WriteLiteral("\r\n        <input id=\"changePrimaryContactBtn\" type=\"submit\" class=\"btn btn-outline-primary\"");
                BeginWriteAttribute("value", " value=\"", 571, "\"", 624, 1);
#nullable restore
#line 14 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
WriteAttributeValue("", 579, resManager.GetString("ChangePrimaryContact"), 579, 45, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 13 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                                                   WriteLiteral(ACCOUNT);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-controller", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "aaf4b72831a3c4335443746b292a1ec76130ea879627", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 16 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.PrimaryContactId);

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n");
#nullable restore
#line 20 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
 if (contactsCount > 0)
{
    await Html.RenderPartialAsync($"{ACC_VIEWS_REL_PATH}Partial/AccountContactsManagement.cshtml");
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"accContactsList\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "aaf4b72831a3c4335443746b292a1ec76130ea8711531", async() => {
                WriteLiteral("\r\n        <table");
                BeginWriteAttribute("class", " class=\"", 951, "\"", 1009, 2);
                WriteAttributeValue("", 959, "fl-table", 959, 8, true);
#nullable restore
#line 27 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
WriteAttributeValue(" ", 967, contactsCount > 0 ? "" : "empty-table", 968, 41, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n            <thead>\r\n");
#nullable restore
#line 29 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                 if (contactsCount > 0)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <tr>\r\n                        <th class=\"d-none\"></th>\r\n                        <th>");
#nullable restore
#line 33 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                       Write(resManager.GetString("FullName"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                        <th>");
#nullable restore
#line 34 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                       Write(resManager.GetString("ContactType"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                        <th>");
#nullable restore
#line 35 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                       Write(resManager.GetString("PhoneNumber"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                        <th>");
#nullable restore
#line 36 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                       Write(resManager.GetString("Email"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                        <th>");
#nullable restore
#line 37 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                       Write(resManager.GetString("Primary"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</th>\r\n                        <th class=\"action-column\"></th>\r\n                        <th class=\"action-column\"></th>\r\n                    </tr>\r\n");
#nullable restore
#line 41 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <tr><th></th><th></th></tr>\r\n");
#nullable restore
#line 45 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
}

#line default
#line hidden
#nullable disable
                WriteLiteral("            </thead>\r\n            <tbody>\r\n");
#nullable restore
#line 48 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                 if (contactsCount == 0)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <tr>\r\n                        <td>");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "aaf4b72831a3c4335443746b292a1ec76130ea8715174", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("</td>\r\n                        <td>\r\n                            <input type=\"button\" class=\"btn btn-outline-dark\" data-toggle=\"modal\" data-target=\"#accContactCreateModal\"");
                BeginWriteAttribute("value", " value=\"", 2131, "\"", 2181, 1);
#nullable restore
#line 53 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
WriteAttributeValue("", 2139, resManager.GetString("ESAccountContacts"), 2139, 42, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                        </td>\r\n                    </tr>\r\n");
#nullable restore
#line 56 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 57 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                 foreach (AccountContactViewModel accountContact in Model.AccountContacts)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <tr>\r\n                        <td class=\"contact-id d-none\">");
#nullable restore
#line 60 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                                 Write(accountContact.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td class=\"tooltip-cell-src\">");
#nullable restore
#line 61 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                                Write(accountContact.FullName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 62 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                         switch (accountContact.ContactType)
                        {
                            case "None":

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <td>");
#nullable restore
#line 65 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                               Write(resManager.GetString("NotSpecify"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 66 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                break;
                            case "Personal":

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <td>");
#nullable restore
#line 68 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                               Write(resManager.GetString("Personal"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 69 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                break;
                            case "Work":

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <td>");
#nullable restore
#line 71 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                               Write(resManager.GetString("Work"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 72 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                break;
                        }

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td class=\"tooltip-cell-src\">");
#nullable restore
#line 74 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                                Write(accountContact.PhoneNumber);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td class=\"tooltip-cell-src\">");
#nullable restore
#line 75 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                                                Write(accountContact.Email);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n");
#nullable restore
#line 76 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                         if (!accountContact.IsPrimary)
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <td>\r\n                                <div>\r\n                                    <div class=\"oval-mark mark-table-cell\"></div>\r\n                                </div>\r\n                            </td>\r\n");
#nullable restore
#line 83 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                        }
                        else if (Model.AccountType == "Individual")
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                            <td>
                                <div>
                                    <div class=""oval-mark-readonly mark-table-cell"">
                                        <div class=""icon-checkmark""></div>
                                    </div>
                                </div>
                            </td>
");
#nullable restore
#line 93 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                            <td>
                                <div>
                                    <div class=""oval-mark-check mark-table-cell"">
                                        <div class=""icon-checkmark""></div>
                                    </div>
                                </div>
                            </td>
");
#nullable restore
#line 103 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 104 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                         if (accountContact.IsPrimary && Model.AccountType == "Individual")
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <td class=\"hide-remove-item-btn\">\r\n                                <div class=\"remove-item-url\" hidden=\"hidden\">\r\n                                    ");
#nullable restore
#line 108 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                               Write(Html.ActionLink(accountContact.Id.ToString(), "Delete", ACC_CONTACT, new { id = accountContact.Id }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n                                </div>\r\n                                <span class=\"icon-bin\"></span>\r\n                            </td>\r\n");
#nullable restore
#line 112 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                            <td class=\"remove-item-btn\">\r\n                                <div class=\"remove-item-url\" hidden=\"hidden\">\r\n                                    ");
#nullable restore
#line 117 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                               Write(Html.ActionLink(accountContact.Id.ToString(), "Delete", ACC_CONTACT, new { id = accountContact.Id }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n                                </div>\r\n                                <span class=\"icon-bin\"></span>\r\n                            </td>\r\n");
#nullable restore
#line 121 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                        }

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <td class=\"edit-item-btn\" data-toggle=\"modal\" data-target=\"#accContactUpdateModal\">\r\n                            <div class=\"edit-item-url\" hidden=\"hidden\">\r\n                                ");
#nullable restore
#line 124 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                           Write(Html.ActionLink(accountContact.Id.ToString(), CONTACT, ACC_CONTACT, new { id = accountContact.Id }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n                            </div>\r\n                            <span class=\"icon-pencil\"></span>\r\n                        </td>\r\n                    </tr>\r\n");
#nullable restore
#line 129 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </tbody>\r\n        </table>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 133 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\ContactsList.cshtml"
      
        await Html.RenderPartialAsync("Partial/Navigation", new NavbarRenderSettings
        {
            ItemsCount = Model.AccountContacts.Count(),
            ViewInfo = accContactsView,
            ControllerName = ACC_CONTACT,
            ActionName = CONTACTS
        });
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AccountViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
