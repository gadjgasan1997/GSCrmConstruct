#pragma checksum "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0eeb96f7be9bada324686ad882b0da00be01d70c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_Partial_AccountContactCreateModal), @"mvc.1.0.view", @"/Views/Account/Partial/AccountContactCreateModal.cshtml")]
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
#line 3 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
using static GSCrm.CommonConsts;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0eeb96f7be9bada324686ad882b0da00be01d70c", @"/Views/Account/Partial/AccountContactCreateModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af9277d557923f57a3a7522d76ddc19e27ae9212", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_Partial_AccountContactCreateModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AccountViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div id=""accContactCreateModal"" class=""modal fade"" tabindex=""-1"" role=""dialog"" aria-hidden=""true"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h5 class=""modal-title"">");
#nullable restore
#line 9 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
                                   Write(resManager.GetString("ContactCreate"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h5>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0eeb96f7be9bada324686ad882b0da00be01d70c5327", async() => {
                WriteLiteral("\r\n                    <div class=\"mt-3\">\r\n                        <input id=\"createAccContactFName\" class=\"form-control\" type=\"text\"");
                BeginWriteAttribute("placeholder", " placeholder=\"", 860, "\"", 913, 1);
#nullable restore
#line 17 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 874, resManager.GetString("FirstNameLabel"), 874, 39, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""createAccContactFNameError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""createAccContactLName"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 1191, "\"", 1243, 1);
#nullable restore
#line 21 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 1205, resManager.GetString("LastNameLabel"), 1205, 38, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""createAccContactLNameError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""createAccContactMName"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 1521, "\"", 1572, 1);
#nullable restore
#line 25 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 1535, resManager.GetString("MidNameLabel"), 1535, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""createAccContactMNameError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""row mt-3"">
                        <div class=""col"">
                            <div class=""block-center"">
                                <p class=""label-md label-non-select"">");
#nullable restore
#line 31 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
                                                                Write(resManager.GetString("ContactType"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</p>
                            </div>
                        </div>
                        <div class=""col dropdown-area"">
                            <input id=""createAccContactType"" class=""current-dropdown-value"" hidden=""hidden"" />
                            <span class=""dropdown-el"">
                                <input type=""radio"" name=""contactType"" value=""None"" checked=""checked"" id=""createContactTypeNone"">
                                <label for=""createContactTypeNone"">");
#nullable restore
#line 38 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
                                                              Write(resManager.GetString("None"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <input type=\"radio\" name=\"contactType\" value=\"Personal\" id=\"createContactTypePersonal\">\r\n                                <label for=\"createContactTypePersonal\">");
#nullable restore
#line 40 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
                                                                  Write(resManager.GetString("Personal"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <input type=\"radio\" name=\"contactType\" value=\"Work\" id=\"createContactTypeWork\">\r\n                                <label for=\"createContactTypeWork\">");
#nullable restore
#line 42 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
                                                              Write(resManager.GetString("Work"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</label>
                            </span>
                        </div>
                        <ul id=""createAccContactTypeError"" class=""under-field-error label-sm mt-3 text-center d-none"" style=""width: 100%""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""createAccContactPhone"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 3330, "\"", 3380, 1);
#nullable restore
#line 48 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 3344, resManager.GetString("PhoneNumber"), 3344, 36, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""createAccContactPhoneError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""createAccContactEmail"" class=""form-control"" type=""email""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 3659, "\"", 3703, 1);
#nullable restore
#line 52 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 3673, resManager.GetString("Email"), 3673, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                        <ul id=\"createAccContactEmailError\" class=\"d-none under-field-error label-sm mt-2\"></ul>\r\n                    </div>\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 15 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
                                        WriteLiteral(ACC_CONTACT);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-controller", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </div>\r\n            <div class=\"modal-footer\">\r\n                <input id=\"createAccContactBtn\" type=\"submit\" class=\"btn btn-primary\"");
            BeginWriteAttribute("value", " value=\"", 4021, "\"", 4065, 1);
#nullable restore
#line 58 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 4029, resManager.GetString("CreateLabel"), 4029, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                <input id=\"cancelCreationAccContactBtn\" type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\"");
            BeginWriteAttribute("value", " value=\"", 4187, "\"", 4231, 1);
#nullable restore
#line 59 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountContactCreateModal.cshtml"
WriteAttributeValue("", 4195, resManager.GetString("CancelLabel"), 4195, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
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
