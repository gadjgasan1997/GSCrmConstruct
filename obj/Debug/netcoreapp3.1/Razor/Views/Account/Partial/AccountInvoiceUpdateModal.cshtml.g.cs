#pragma checksum "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e9cde4c657770b67eefc687f561831171e635cc9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_Partial_AccountInvoiceUpdateModal), @"mvc.1.0.view", @"/Views/Account/Partial/AccountInvoiceUpdateModal.cshtml")]
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
#line 3 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
using static GSCrm.CommonConsts;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e9cde4c657770b67eefc687f561831171e635cc9", @"/Views/Account/Partial/AccountInvoiceUpdateModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"af9277d557923f57a3a7522d76ddc19e27ae9212", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_Partial_AccountInvoiceUpdateModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AccountViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Update", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
<div id=""accInvoiceUpdateModal"" class=""modal fade"" tabindex=""-1"" role=""dialog"" aria-hidden=""true"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h5 class=""modal-title"">");
#nullable restore
#line 9 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
                                   Write(resManager.GetString("InvoiceUpdate"));

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
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e9cde4c657770b67eefc687f561831171e635cc95327", async() => {
                WriteLiteral("\r\n                    <div class=\"mt-3\">\r\n                        <input id=\"updateAccInvoiceBankName\" class=\"form-control\" type=\"text\"");
                BeginWriteAttribute("placeholder", " placeholder=\"", 863, "\"", 910, 1);
#nullable restore
#line 17 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 877, resManager.GetString("BankName"), 877, 33, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""updateAccInvoiceBankNameError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""updateAccInvoiceCity"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 1190, "\"", 1237, 1);
#nullable restore
#line 21 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 1204, resManager.GetString("BankCity"), 1204, 33, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""updateAccInvoiceCityError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""updateAccInvoiceChecking"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 1517, "\"", 1571, 1);
#nullable restore
#line 25 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 1531, resManager.GetString("CheckingAccount"), 1531, 40, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""updateAccInvoiceCheckingError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""updateAccInvoiceCorrespondent"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 1860, "\"", 1919, 1);
#nullable restore
#line 29 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 1874, resManager.GetString("CorrespondentAccount"), 1874, 45, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""updateAccInvoiceCorrespondentError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""updateAccInvoiceBIC"" class=""form-control form-control-number"" type=""number""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 2225, "\"", 2267, 1);
#nullable restore
#line 33 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 2239, resManager.GetString("BIC"), 2239, 28, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                        <ul id=""updateAccInvoiceBICError"" class=""d-none under-field-error label-sm mt-2""></ul>
                    </div>
                    <div class=""mt-3"">
                        <input id=""updateAccInvoiceSWIFT"" class=""form-control"" type=""text""");
                BeginWriteAttribute("placeholder", " placeholder=\"", 2543, "\"", 2587, 1);
#nullable restore
#line 37 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 2557, resManager.GetString("SWIFT"), 2557, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                        <ul id=\"updateAccInvoiceSWIFTError\" class=\"d-none under-field-error label-sm mt-2\"></ul>\r\n                    </div>\r\n                ");
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
#line 15 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
                                        WriteLiteral(ACC_INVOICE);

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
            WriteLiteral("\r\n            </div>\r\n            <div class=\"modal-footer\">\r\n                <input id=\"updateAccInvoiceBtn\" type=\"submit\" class=\"btn btn-primary\"");
            BeginWriteAttribute("value", " value=\"", 2905, "\"", 2949, 1);
#nullable restore
#line 43 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 2913, resManager.GetString("UpdateLabel"), 2913, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                <input id=\"cancelUpdateAccInvoiceBtn\" type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\"");
            BeginWriteAttribute("value", " value=\"", 3069, "\"", 3113, 1);
#nullable restore
#line 44 "C:\Users\gadjg\source\repos\GSCrm\GSCrm\Views\Account\Partial\AccountInvoiceUpdateModal.cshtml"
WriteAttributeValue("", 3077, resManager.GetString("CancelLabel"), 3077, 36, false);

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
